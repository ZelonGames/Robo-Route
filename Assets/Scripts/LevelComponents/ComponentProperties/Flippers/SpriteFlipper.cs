using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpriteFlipper : MonoBehaviour
{
    public event Action Validated;

    public SpriteRenderer spriteRenderer;

    public bool isFlipped = false;

    void Start()
    {
    }

    private void OnValidate()
    {
        spriteRenderer.flipX = isFlipped;


        Validated?.Invoke();
    }
}
