using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatformBehaviour : MonoBehaviour
{
    [SerializeField] private ItemMover itemMover;

    public float maxJumpForce = 10;
    public float jumpMultiplier = 1.0f;
    public float squashAmount = 0.2f;
    public float squashDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    private bool isMoving = false;

    private void Start()
    {
        originalScale = transform.localScale;

        itemMover.StartedMovingItem += ItemMover_StartedMovingItem;
        itemMover.FinishedMovingItem += ItemMover_MovedItem;
    }

    private void ItemMover_MovedItem(GameObject movedGameObject)
    {
        isMoving = false;
    }

    private void ItemMover_StartedMovingItem(GameObject movedGameObject)
    {
        if (itemMover.canMove)
            isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMoving)
            return;

        if (other.CompareTag("Robot"))
        {
            Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();
            float jumpForce = -otherRigidbody.velocity.y * jumpMultiplier;
            jumpForce = Mathf.Min(maxJumpForce, jumpForce);
            Vector2 velocity = otherRigidbody.velocity;
            velocity.y = jumpForce;
            velocity.x = otherRigidbody.velocity.x;
            otherRigidbody.velocity = velocity;

            if (squashCoroutine != null)
                StopCoroutine(squashCoroutine);

            squashCoroutine = StartCoroutine(Squash());
        }
    }

    private IEnumerator Squash()
    {
        float t = 0f;

        while (t < squashDuration)
        {
            float squashFactor = Mathf.Lerp(1f, 1f - squashAmount, t / squashDuration);
            transform.localScale = new Vector3(originalScale.x, squashFactor * originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;

        while (t < squashDuration)
        {
            float stretchFactor = Mathf.Lerp(1f - squashAmount, 1f, t / squashDuration);
            transform.localScale = new Vector3(originalScale.x, stretchFactor * originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        squashCoroutine = null;
    }
}



