using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] private float moveSpeed = 2;

    void Start()
    {
    }

}
