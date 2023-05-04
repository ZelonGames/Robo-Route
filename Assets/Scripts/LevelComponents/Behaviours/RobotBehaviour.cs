using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private ParticleSystem groundTrail;

    void Start()
    {
    }

    public void Update()
    {
        if ((rigidbody2D.constraints & RigidbodyConstraints2D.FreezePositionY) != 0)
        {
            if (!groundTrail.isPlaying)
                groundTrail.Play();
        }
        else if (groundTrail.isPlaying)
            groundTrail.Stop();
    }
}
