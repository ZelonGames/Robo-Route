using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SidewaysMover : MonoBehaviour
{
    public enum MovementState
    {
        Forward,
        Backward,
        Stopped,
    }

    [SerializeField] ItemMover itemMover;
    public float moveSpeed = 0.1f;
    public float distance = 3;
    public bool moveLeft = false;


    private Vector2 previousFollowingObjectVelocity;
    private Vector2 startPosition;
    private Collider2D followingObject;

    private MovementState movementState = MovementState.Stopped;

    private void Start()
    {
        itemMover.FinishedMovingItem += ItemMover_MovedItem;
    }

    private void OnEnable()
    {
        startPosition = transform.position;
    }

    private void OnDestroy()
    {
        itemMover.FinishedMovingItem -= ItemMover_MovedItem;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Robot") && followingObject == null)
        {
            if (movementState == MovementState.Stopped)
            {
                followingObject = other;
                previousFollowingObjectVelocity = followingObject.attachedRigidbody.velocity;
                followingObject.attachedRigidbody.AddConstraint(RigidbodyConstraints2D.FreezePositionY);
                movementState = MovementState.Forward;
            }
        }
    }

    private void ItemMover_MovedItem()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        switch (movementState)
        {
            case MovementState.Stopped:
                break;
            case MovementState.Forward:
                transform.position += (moveLeft ? Vector3.left : Vector3.right) * moveSpeed;
                if (followingObject != null)
                    followingObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    followingObject.transform.position = transform.position;

                if (Vector3.Distance(transform.position, startPosition) >= distance)
                    movementState = MovementState.Backward;
                break;
            case MovementState.Backward:
                if (followingObject != null)
                    ReleaseFollowingObject();

                if (Vector3.Distance(transform.position, startPosition) > moveSpeed)
                    transform.position += (moveLeft ? Vector3.right : Vector3.left) * moveSpeed;

                if (Vector2.Distance(transform.position, startPosition) <= moveSpeed)
                {
                    transform.position = startPosition;
                    movementState = MovementState.Stopped;
                }
                break;
        }
    }

    private void ReleaseFollowingObject()
    {
        previousFollowingObjectVelocity.x = moveLeft ? -Math.Abs(previousFollowingObjectVelocity.x) : Math.Abs(previousFollowingObjectVelocity.x);
        followingObject.attachedRigidbody.velocity = previousFollowingObjectVelocity;
        followingObject.attachedRigidbody.RemoveConstraint(RigidbodyConstraints2D.FreezePositionY);
        followingObject = null;
    }
}

