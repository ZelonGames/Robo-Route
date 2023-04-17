using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public event Action StartedShaking;
    public event Action StoppedShaking;

    public float shakeDuration = 0.5f;
    public float shakeSpeed = 1f;
    public float shakeMagnitude = 1f;
    [SerializeField] private ParticleSystem smokeEffect;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0f;

    private bool invokedStartedShaking = false;
    private bool invokedStoppedShaking = false;

    private void Start()
    {
        initialPosition = gameObject.transform.localPosition;
    }

    private void OnEnable()
    {
        invokedStartedShaking = false;
        invokedStoppedShaking = false;
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
            if (!invokedStartedShaking)
                StartedShaking?.Invoke();

            gameObject.transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * shakeSpeed;
            invokedStartedShaking = true;
        }
        else
        {
            if (!invokedStoppedShaking)
                StoppedShaking?.Invoke();

            currentShakeDuration = 0f;
            gameObject.transform.localPosition = initialPosition;
            invokedStoppedShaking = true;
        }
    }

    public void Shake()
    {
        currentShakeDuration = shakeDuration;
    }
}
