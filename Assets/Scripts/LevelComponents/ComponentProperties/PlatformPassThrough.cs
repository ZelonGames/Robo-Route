using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPassThrough : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private new Collider2D collider2D;

    private void Update()
    {
        collider2D.enabled = rigidbody2D.velocity.y <= 0;
    }
}
