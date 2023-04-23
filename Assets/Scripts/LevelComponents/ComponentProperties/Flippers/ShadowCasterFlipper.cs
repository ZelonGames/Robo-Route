using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteFlipper))]
public class ShadowCasterFlipper : MonoBehaviour
{
    public SpriteFlipper spriteFlipper;

    public ShadowCaster2D shadowCasterLeft;
    public ShadowCaster2D shadowCasterRight;

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
        return;
        for (int i = 0; i < shadowCasterLeft.shapePath.Length; i++)
        {
            shadowCasterLeft.shapePath[i].x *= -1;
        }
    }
}
