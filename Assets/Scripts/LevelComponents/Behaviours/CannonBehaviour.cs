using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehaviour : MonoBehaviour
{
    public float launchPower = 20;
    public float squashAmount = 0.2f;
    public float squashDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;
    private Collider2D launchingCollider;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Robot"))
        {
            launchingCollider = other;
            Rigidbody2D otherRigidbody = other.attachedRigidbody;
            otherRigidbody.RemoveConstraint(RigidbodyConstraints2D.FreezePositionY);
            otherRigidbody.velocity = Vector3.zero;
            otherRigidbody.velocity = new Vector2(1, 1.5f).normalized * launchPower; // set velocity to 45 degrees to the right and up

            if (squashCoroutine != null)
                StopCoroutine(squashCoroutine);

            squashCoroutine = StartCoroutine(Squash());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (launchingCollider != collision)
            return;
    }

    private IEnumerator Squash()
    {
        float t = 0f;

        while (t < squashDuration)
        {
            float squashFactor = Mathf.Lerp(1f, 1f - squashAmount, t / squashDuration);
            transform.localScale = new Vector3(squashFactor * originalScale.x, originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;

        while (t < squashDuration)
        {
            float stretchFactor = Mathf.Lerp(1f - squashAmount, 1f, t / squashDuration);
            transform.localScale = new Vector3(stretchFactor * originalScale.x, originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        squashCoroutine = null;
    }

}
