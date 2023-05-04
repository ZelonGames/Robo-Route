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
    private bool isPlaying = false;

    private void Start()
    {
        initialPosition = gameObject.transform.localPosition;
    }

    public void Play()
    {
        isPlaying = true;
        invokedStartedShaking = false;
        invokedStoppedShaking = false;

        initialPosition = gameObject.transform.localPosition;

        if (smokeEffect != null)
            smokeEffect.Play();
        if (!invokedStartedShaking)
            StartedShaking?.Invoke();

        Shake();
    }

    public void Stop()
    {
        
        isPlaying = false;
    }

    void Update()
    {
        if (!isPlaying)
            return;

        if (currentShakeDuration > 0)
        {
            gameObject.transform.localPosition = initialPosition + UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * shakeSpeed;
            invokedStartedShaking = true;
        }
        else
        {
            if (!invokedStoppedShaking && invokedStartedShaking)
                StoppedShaking?.Invoke();

            currentShakeDuration = 0f;
            gameObject.transform.localPosition = initialPosition;
            invokedStoppedShaking = true;
            isPlaying = false;
        }
    }

    public void Shake()
    {
        currentShakeDuration = shakeDuration;
    }
}
