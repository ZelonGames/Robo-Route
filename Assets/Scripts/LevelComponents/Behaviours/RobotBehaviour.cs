using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private ParticleSystem groundTrail;
    [SerializeField] private float initialVelocity = 2;
    [SerializeField] private bool useInitialVelocity;
    private bool hasSetInitialVelocity = false;

    public Vector2 Velocity { get; private set; }


    void Start()
    {
        if (GetComponent<SpriteRenderer>().flipX)
            initialVelocity *= -1;

        UpdateInitialVelocity();
    }

    public void FixedUpdate()
    {
        UpdateInitialVelocity();
        Velocity = rigidbody2D.velocity;

        UpdateGroundTrail();
    }

    private void UpdateInitialVelocity()
    {
        if (GameController.hasStartedGame && !hasSetInitialVelocity)
        {
            rigidbody2D.velocity = new Vector2(initialVelocity, 0);
            hasSetInitialVelocity = true;
        }
    }

    private void UpdateGroundTrail()
    {
        if (Mathf.Abs(rigidbody2D.velocity.y) <= 0.05f)
        {
            if (!groundTrail.isPlaying)
                groundTrail.Play();
        }
        else if (groundTrail.isPlaying)
            groundTrail.Stop();
    }
}
