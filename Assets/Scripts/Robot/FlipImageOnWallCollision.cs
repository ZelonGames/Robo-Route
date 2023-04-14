using UnityEngine;

public class FlipImageOnWallCollision : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Get the current x velocity of the GameObject
        float xVelocity = GetComponent<Rigidbody2D>().velocity.x;

        // Flip the sprite horizontally if the x velocity is negative
        if (xVelocity < 0)
        {
            spriteRenderer.flipX = true;
        }
        // Otherwise, flip it back to its original orientation
        else if (xVelocity > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}
