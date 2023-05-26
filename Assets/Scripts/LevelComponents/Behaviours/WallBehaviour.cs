using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D robotCollider)
    {
        if (robotCollider.gameObject.CompareTag("Robot"))
        {
            Debug.Log("Wall");
            var robotRigidbody2D = robotCollider.collider.attachedRigidbody;
            var robotBoxCollider2D = robotCollider.gameObject.GetComponent<BoxCollider2D>();

            bool isRobotMovingRight = robotCollider.relativeVelocity.x > 0;
            bool isWallToTheRight = transform.position.x > robotCollider.gameObject.transform.position.x;

            bool isRobotMovingLeft = robotCollider.relativeVelocity.x < 0;
            bool isWallToTheLeft = transform.position.x < robotCollider.gameObject.transform.position.x;

            if ((isRobotMovingRight && isWallToTheRight ||
                isRobotMovingLeft && isWallToTheLeft) &&
                (robotBoxCollider2D.bounds.GetBottomEdge() <= boxCollider2D.bounds.GetTopEdge() &&
                robotBoxCollider2D.bounds.GetBottomEdge() >= boxCollider2D.bounds.GetBottomEdge() ||
                robotBoxCollider2D.bounds.GetTopEdge() <= boxCollider2D.bounds.GetTopEdge() &&
                robotBoxCollider2D.bounds.GetTopEdge() >= boxCollider2D.bounds.GetBottomEdge()))
            {
                if (isWallToTheRight)
                    robotBoxCollider2D.AlignRightWithLeft(boxCollider2D);
                else if (isWallToTheLeft)
                    robotBoxCollider2D.AlignLeftWithRight(boxCollider2D);

                Vector2 velocity = robotCollider.relativeVelocity;
                velocity.x *= -1;
                robotRigidbody2D.velocity = velocity;
            }
        }
    }
}
