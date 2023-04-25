using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollingCloud : MonoBehaviour
{
    public float speed = 2f;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        // Get the width of the screen in world units
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;

    }

    // Update is called once per frame
    void Update()
    {
        // Move the sprite to the left by the specified speed
        transform.Translate(-speed * Time.deltaTime, 0, 0);

        // If the sprite's right edge is outside the screen's left edge
        float spriteRightEdge = transform.position.x + (transform.localScale.x * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f);
        if (spriteRightEdge < -screenWidth / 2f)
        {
            // Position the sprite so its left edge is outside the screen's right edge
            float newPosX = screenWidth / 2f + (transform.localScale.x * GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f);
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        }
    }
}