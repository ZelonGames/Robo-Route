using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Stops robots only if they are falling down before they collide
/// </summary>
public class FallingCollision : MonoBehaviour
{
    public static event Action RobotLanded;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float maxHeightDistance = 0.2f;

    // Keeps track of how many platforms each robot collides with
    private static readonly Dictionary<Collider2D, HashSet<FallingCollision>> collidingRobots = new Dictionary<Collider2D, HashSet<FallingCollision>>();

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D robotCollider)
    {
        if (robotCollider.CompareTag("Robot"))
        {
            var robotRigidbody2D = robotCollider.gameObject.GetComponent<Rigidbody2D>();

            if (robotRigidbody2D.velocity.y <= 0)
            {
                if (robotRigidbody2D.velocity.y < -10f)
                    RobotLanded?.Invoke();
                robotRigidbody2D.velocity.Set(robotRigidbody2D.velocity.x, 0);

                var robotBoxCollider = robotCollider.gameObject.GetComponent<BoxCollider2D>();
                if (!collidingRobots.ContainsKey(robotCollider))
                    collidingRobots.Add(robotCollider, new HashSet<FallingCollision>());

                if (!collidingRobots[robotCollider].Contains(this))
                    collidingRobots[robotCollider].Add(this);

                robotBoxCollider.AlignBottomWithTop(boxCollider);
                robotRigidbody2D.AddConstraint(RigidbodyConstraints2D.FreezePositionY);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D robotCollider)
    {
        if (robotCollider.CompareTag("Robot") && collidingRobots.ContainsKey(robotCollider))
        {
            collidingRobots[robotCollider].Remove(this);
            if (collidingRobots[robotCollider].Count == 0)
            {
                collidingRobots.Remove(robotCollider);
                robotCollider.gameObject.GetComponent<Rigidbody2D>().RemoveConstraint(RigidbodyConstraints2D.FreezePositionY);
            }
        }
    }
}
