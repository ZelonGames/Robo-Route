using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private ParticleSystem groundTrail;

    public Vector2 Velocity { get; private set; }

    void Start()
    {
    }

    public void FixedUpdate()
    {
        Velocity = rigidbody2D.velocity;

        UpdateGroundTrail();
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
