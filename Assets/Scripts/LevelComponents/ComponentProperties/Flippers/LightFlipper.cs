using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteFlipper))]
public class LightFlipper : MonoBehaviour
{
    public SpriteFlipper spriteFlipper;

    public Light2D spotlightLeft;
    public Light2D spotlightRight;

    void Start()
    {
        spriteFlipper.Validated -= SpriteFlipper_Validated;
        spriteFlipper.Validated += SpriteFlipper_Validated;
    }

    private void OnValidate()
    {
        spriteFlipper.Validated -= SpriteFlipper_Validated;
        spriteFlipper.Validated += SpriteFlipper_Validated;
    }

    private void OnDestroy()
    {
        spriteFlipper.Validated -= SpriteFlipper_Validated;
    }

    private void SpriteFlipper_Validated()
    {
        if (spriteFlipper.isFlipped)
        {
            spotlightLeft.enabled = true;
            spotlightRight.enabled = false;
        }
        else
        {
            spotlightLeft.enabled = false;
            spotlightRight.enabled = true;
        }
    }
}
