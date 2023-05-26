using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private ParticleSystem groundTrail;

    void Start()
    {
    }

    public void Update()
    {
        UpdateGroundTrail();
    }

    private void UpdateGroundTrail()
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
