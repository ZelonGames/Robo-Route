using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    public float MaxVelocity { get; private set; }
    public float acceleration = 0.5f;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        MaxVelocity = Mathf.Abs(rigidbody2D.velocity.x);
    }

    public void Update()
    {
        if (Mathf.Abs(rigidbody2D.velocity.x) < MaxVelocity)
        {
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = velocity.x > 0 ? acceleration : -acceleration;
            rigidbody2D.AddForce(velocity);
        }
    }
}
