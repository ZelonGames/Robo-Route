using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatformBehaviour : MonoBehaviour
{
    public static event Action LaunchedRobot;
    [SerializeField] private GameObject middleCircle;
    [SerializeField] private GameObject topCircle;
    [SerializeField] private ItemMover itemMover;

    public float launchPower = 20;
    public float squashDuration = 0.1f;

    private BoxCollider2D topBoxCollider2D;
    private readonly HashSet<BoxCollider2D> collidingRobots = new();

    private float middleHomePosition;
    private float topHomePosition;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    private bool isSquashing = false;
    private bool isMoving = false;

    private void Start()
    {
        topBoxCollider2D = topCircle.GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;

        middleHomePosition = middleCircle.transform.position.y;
        topHomePosition = topCircle.transform.position.y;

        itemMover.StartedMovingItem += ItemMover_StartedMovingItem;
        itemMover.FinishedMovingItem += ItemMover_MovedItem;
    }



    private void ItemMover_MovedItem()
    {
        middleHomePosition = middleCircle.transform.position.y;
        topHomePosition = topCircle.transform.position.y;
        isMoving = false;
    }

    private void ItemMover_StartedMovingItem()
    {
        if (itemMover.canMove)
            isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMoving)
            return;

        if (other.CompareTag("Robot"))
        {
            var robotBoxCollider = other.gameObject.GetComponent<BoxCollider2D>();
            if (collidingRobots.Contains(robotBoxCollider))
                return;

            collidingRobots.Add(robotBoxCollider);

            float launchOffset = 0.5f;
            float collisionGravity = other.attachedRigidbody.gravityScale;
            float velocity = Mathf.Abs(other.attachedRigidbody.velocity.y);
            float distanceTop = Mathf.Abs(topHomePosition - transform.position.y) + launchOffset;
            float distanceMiddle = Mathf.Abs(middleHomePosition - transform.position.y);
            float squashDurationTop = distanceTop / velocity;
            float moveUpDuration = distanceTop / launchPower;

            var sequence = DOTween.Sequence()
                .Append(middleCircle.gameObject.transform.DOMoveY(gameObject.transform.position.y, squashDurationTop))
                .Append(middleCircle.gameObject.transform.DOMoveY(middleHomePosition + launchOffset, moveUpDuration))
                .Append(middleCircle.gameObject.transform.DOMoveY(middleHomePosition, squashDurationTop));

            var sequence2 = DOTween.Sequence()
                .Append(topCircle.gameObject.transform.DOMoveY(gameObject.transform.position.y, squashDurationTop))
                .Append(topCircle.gameObject.transform.DOMoveY(topHomePosition + launchOffset, moveUpDuration));

            sequence2.OnUpdate(() =>
            {
                other.attachedRigidbody.AddConstraint(RigidbodyConstraints2D.FreezePositionY);
                other.attachedRigidbody.velocity = new Vector2(other.attachedRigidbody.velocity.x, 0);
                robotBoxCollider.AlignBottomWithTop(topBoxCollider2D);
            });
            
            sequence2.OnPlay(() =>
            {
                LaunchedRobot?.Invoke();
            });

            sequence2.OnComplete(() =>
            {
                collidingRobots.Remove(robotBoxCollider);
                other.attachedRigidbody.RemoveConstraint(RigidbodyConstraints2D.FreezePositionY);
                LaunchRobot(other);
                topCircle.gameObject.transform.DOMoveY(topHomePosition, squashDurationTop);
            });
        }
    }

    private void LaunchRobot(Collider2D robot)
    {
        Vector2 velocity = robot.attachedRigidbody.velocity;
        velocity.y = launchPower;
        velocity.x = robot.attachedRigidbody.velocity.x;
        robot.attachedRigidbody.velocity = velocity;
    }
}



