using System.Collections;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float minDuration = 1f;
    public float maxDuration = 2f;
    public float floatHeight = 0.1f;

    private Vector3 startPos;
    private float floatDuration = 0;
    private int direction;

    private void Start()
    {
        floatDuration = Random.Range(minDuration, maxDuration);
        startPos = transform.position;
        direction = 1;
        StartCoroutine(FloatingCoroutine());
    }

    private void OnEnable()
    {
        floatDuration = Random.Range(minDuration, maxDuration);
        startPos = transform.position;
        direction = 1;
        StartCoroutine(FloatingCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FloatingCoroutine()
    {
        // Wait for a random amount of time before starting the floating motion
        float startTime = Time.time;

        while (true)
        {
            float t = (Time.time - startTime) / floatDuration;

            Vector3 newPos = transform.position;
            newPos.y = startPos.y + direction * floatHeight * Mathf.Sin(t * Mathf.PI);
            transform.position = newPos;

            yield return null;
        }
    }
}
