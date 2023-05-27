using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    private Vector2 velocity;
    public float MaxVelocity { get; private set; }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        MaxVelocity = Mathf.Abs(rigidbody2D.velocity.x);
    }

    public void Update()
    {
        velocity.y = rigidbody2D.velocity.y;
        if (Mathf.Abs(rigidbody2D.velocity.x) < MaxVelocity)
        {
            velocity.x = rigidbody2D.velocity.x < 0 ? -MaxVelocity : MaxVelocity;
            rigidbody2D.velocity = velocity;
        }
    }
}
