using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeSpeed = 1f;
    public float shakeMagnitude = 1f;
    [SerializeField] private ParticleSystem smokeEffect;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0f;

    private void Start()
    {
        initialPosition = gameObject.transform.localPosition;
    }

    private void OnEnable()
    {
        smokeEffect.Play();
        Shake();
    }

    private void OnDisable()
    {
        gameObject.transform.position = initialPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            gameObject.transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            currentShakeDuration -= Time.deltaTime * shakeSpeed;
        }
        else
        {
            currentShakeDuration = 0f;
            gameObject.transform.localPosition = initialPosition;
        }
    }

    public void Shake()
    {
        currentShakeDuration = shakeDuration;
    }
}
