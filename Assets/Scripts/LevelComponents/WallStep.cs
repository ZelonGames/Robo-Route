using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallStep : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D platformCollider;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Robot"))
            return;

        collision.collider.AlignBottomWithTop(platformCollider, 0.05f);
        collision.rigidbody.velocity = collision.gameObject.GetComponent<RobotBehaviour>().Velocity;
    }
}
