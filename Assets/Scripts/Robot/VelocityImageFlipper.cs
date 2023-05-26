using UnityEngine;

public class VelocityImageFlipper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float xVelocity = rigidbody2D.velocity.x;

        if (xVelocity < 0)
            spriteRenderer.flipX = true;
        else if (xVelocity > 0)
            spriteRenderer.flipX = false;
    }
}
