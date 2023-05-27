using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    [SerializeField] private Collider2D boxCollider2D;

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D robotCollider)
    {
        if (!robotCollider.gameObject.CompareTag("Robot"))
            return;

        Vector2 velocity = robotCollider.gameObject.GetComponent<RobotBehaviour>().Velocity;
        
        bool isRobotMovingRight = velocity.x >= 0;
        bool isWallToTheRight = transform.position.x > robotCollider.gameObject.transform.position.x;

        bool isRobotMovingLeft = velocity.x <= 0;
        bool isWallToTheLeft = transform.position.x < robotCollider.gameObject.transform.position.x;

        if (isRobotMovingRight && isWallToTheRight ||
            isRobotMovingLeft && isWallToTheLeft)
            robotCollider.rigidbody.velocity = Vector2.Reflect(velocity, Vector2.right);
    }
}
