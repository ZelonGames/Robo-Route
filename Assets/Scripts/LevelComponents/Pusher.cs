using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    private float maxVelocity;
    private Vector2 velocity;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        maxVelocity = Mathf.Abs(rigidbody2D.velocity.x);
    }

    public void Update()
    {
        velocity.y = rigidbody2D.velocity.y;
        if (Mathf.Abs(rigidbody2D.velocity.x) < maxVelocity)
        {
            velocity.x = rigidbody2D.velocity.x < 0 ? -maxVelocity : maxVelocity;
            rigidbody2D.velocity = velocity;
        }
    }
}
