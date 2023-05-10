using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject middleCircle;
    [SerializeField] private GameObject topCircle;
    [SerializeField] private ItemMover itemMover;

    public float launchPower = 20;
    public float squashDuration = 0.1f;

    private BoxCollider2D topBoxCollider2D;
    private readonly HashSet<BoxCollider2D> collidingRobots = new();

    private Vector2 middleHomePosition;
    private Vector2 topHomePosition;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    private bool isSquashing = false;
    private bool isMoving = false;

    private void Start()
    {
        topBoxCollider2D = topCircle.GetComponent<BoxCollider2D>();
        originalScale = transform.localScale;

        middleHomePosition = middleCircle.transform.position;
        topHomePosition = topCircle.transform.position;

        itemMover.StartedMovingItem += ItemMover_StartedMovingItem;
        itemMover.FinishedMovingItem += ItemMover_MovedItem;
    }

    private void ItemMover_MovedItem(GameObject movedGameObject)
    {
        isMoving = false;
    }

    private void ItemMover_StartedMovingItem(GameObject movedGameObject)
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

            var sequence = DOTween.Sequence();
            sequence.Append(middleCircle.gameObject.transform.DOMoveY(gameObject.transform.position.y, squashDuration))
                .Append(middleCircle.gameObject.transform.DOMoveY(middleHomePosition.y, squashDuration));
            sequence.OnUpdate(() =>
            {
                other.attachedRigidbody.velocity = new Vector2(other.attachedRigidbody.velocity.x, 0);
                robotBoxCollider.AlignBottomWithTop(topBoxCollider2D);
            });

            var sequence2 = DOTween.Sequence();
            sequence2.Append(topCircle.gameObject.transform.DOMoveY(gameObject.transform.position.y, squashDuration))
                .Append(topCircle.gameObject.transform.DOMoveY(topHomePosition.y, squashDuration));
            sequence2.OnComplete(() => 
            { 
                collidingRobots.Remove(robotBoxCollider); 
                LaunchRobot(other);
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



