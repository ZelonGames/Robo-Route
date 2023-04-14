using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class MonoBehaviourContent
{
    public Rigidbody2D rigidbody2D;
    public Collider2D collider2D;
    public BoxCollider2D boxCollider2D;

    public MonoBehaviourContent()
    {

    }

    public MonoBehaviourContent(Rigidbody2D rigidbody2D, BoxCollider2D boxCollider2D)
    {
        this.rigidbody2D = rigidbody2D;
        this.boxCollider2D = boxCollider2D;
    }

    public MonoBehaviourContent(Rigidbody2D rigidbody2D, Collider2D collider2D)
    {
        this.rigidbody2D = rigidbody2D;
        this.collider2D = collider2D;
    }

    public MonoBehaviourContent(Rigidbody2D rigidbody2D, Collider2D collider2D, BoxCollider2D boxCollider2D) : 
        this(rigidbody2D, collider2D)
    {
        this.boxCollider2D = boxCollider2D;
    }
}
