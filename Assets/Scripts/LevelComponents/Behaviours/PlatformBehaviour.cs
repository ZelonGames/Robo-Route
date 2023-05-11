using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    [SerializeField] private FallingCollision fallingCollision;
    [SerializeField] private Shaker shaker;

    private void Start()
    {
        if (shaker != null && fallingCollision != null)
            fallingCollision.RobotLandedSelf += OnStartShaking;
    }

    private void OnDestroy()
    {
        if (fallingCollision != null)
            fallingCollision.RobotLandedSelf -= OnStartShaking;
    }

    private void OnStartShaking(Collider2D other)
    {
        shaker.Play();
    }
}
