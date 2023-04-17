using System.Collections;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public enum Direction
    {
        Vertical,
        Horizontal,
    }

    public float minDuration = 1f;
    public float maxDuration = 2f;
    public float floatHeight = 0.1f;

    private Vector3 startPos;
    private float floatDuration = 0;
    private int startDirection;

    public Direction direction = Direction.Vertical;

    private void Start()
    {
        floatDuration = Random.Range(minDuration, maxDuration);
        startPos = transform.position;
        startDirection = 1;
        StartCoroutine(FloatingCoroutine());
    }

    private void OnEnable()
    {
        floatDuration = Random.Range(minDuration, maxDuration);
        startPos = transform.position;
        startDirection = 1;
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
            switch (direction)
            {
                case Direction.Vertical:
                    newPos.y = startPos.y + startDirection * floatHeight * Mathf.Sin(t * Mathf.PI);
                    break;
                case Direction.Horizontal:
                    newPos.x = startPos.x + startDirection * floatHeight * Mathf.Sin(t * Mathf.PI);
                    break;
                default:
                    break;
            }
            transform.position = newPos;

            yield return null;
        }
    }
}
