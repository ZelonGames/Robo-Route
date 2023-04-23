using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpriteFlipper : MonoBehaviour
{
    public event Action Validated;

    public SpriteRenderer spriteRenderer;
    private LevelComponentSettings levelComponentSettings;

    public bool isFlipped = false;

    void Start()
    {
    }

    private void OnValidate()
    {
        spriteRenderer.flipX = isFlipped;

        if (levelComponentSettings != null)
            levelComponentSettings.UpdateSetting(nameof(isFlipped), isFlipped);

        Validated?.Invoke();
    }
}
