using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : MonoBehaviour
{
    public float squashAmount = 0.2f;
    public float squashDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;


    private void Start()
    {
        originalScale = gameObject.transform.localScale;
    }

    public void Play()
    {
        originalScale = gameObject.transform.localScale;
        if (squashCoroutine != null)
            StopCoroutine(squashCoroutine);

        squashCoroutine = StartCoroutine(Squash2());
    }

    private IEnumerator Squash2()
    {
        float t = 0f;

        while (t < squashDuration)
        {
            float squashFactor = Mathf.Lerp(1, 1f - squashAmount, t / squashDuration);
            gameObject.transform.localScale = new Vector3(originalScale.x, squashFactor * originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;

        while (t < squashDuration)
        {
            float stretchFactor = Mathf.Lerp(1f - squashAmount, 1f, t / squashDuration);
            gameObject.transform.localScale = new Vector3(originalScale.x, stretchFactor * originalScale.y, originalScale.z);
            t += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = originalScale;
        squashCoroutine = null;
    }
}
