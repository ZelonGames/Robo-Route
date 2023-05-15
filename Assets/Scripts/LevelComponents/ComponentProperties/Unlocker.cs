using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public event Action Unlock;
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    private bool shouldUnlock = false;

    public bool isLocked { get; private set; }

    private void Start()
    {
        isLocked = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        startColor = spriteRenderer.color;
        spriteRenderer.color = Color.gray;
        KeyBehaviour.UsedAnyKey += OnUnlock;
    }

    private void OnUnlock(Unlocker unlocker)
    {
        if (Input.GetMouseButtonUp(0) && shouldUnlock && unlocker.gameObject.Equals(gameObject))
        {
            isLocked = false;
            shouldUnlock = false;
            spriteRenderer.color = startColor;
            Unlock?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLocked)
            return;

        if (collision.CompareTag("Key"))
            shouldUnlock = true;
        else
            shouldUnlock = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        shouldUnlock = false;
    }
}
