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
    public event Action<Collider2D> RobotLandedSelf;

    [SerializeField] private EdgeCollider2D boxCollider;

    private void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Robot"))
            return;

        var robotRigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();

        if (robotRigidbody2D.velocity.y <= 0)
        {
            if (robotRigidbody2D.velocity.y < -10f)
            {
                RobotLanded?.Invoke();
                RobotLandedSelf?.Invoke(collision.collider);
            }
        }
    }
}
