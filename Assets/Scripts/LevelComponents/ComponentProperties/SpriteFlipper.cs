using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private LevelComponentSettings levelComponentSettings;

    public bool isFlipped = false;

    void Start()
    {
        levelComponentSettings = gameObject.GetComponent<LevelComponentSettings>();
        spriteRenderer.flipX = isFlipped;

        if (levelComponentSettings != null)
        {
            if (levelComponentSettings.settings.ContainsKey(nameof(isFlipped)))
                isFlipped = (bool)levelComponentSettings.settings[nameof(isFlipped)];
            OnValidate();
        }
    }

    private void OnValidate()
    {
        spriteRenderer.flipX = isFlipped;

        if (levelComponentSettings != null)
            levelComponentSettings.UpdateSetting(nameof(isFlipped), isFlipped);
    }
}
