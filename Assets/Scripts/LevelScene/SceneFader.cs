using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static event Action<float> Fading;

    public string sceneName = "LevelWorld";
    [SerializeField] private CanvasGroup fadeBackground;
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float waitTime = 0.3f;
    [SerializeField] private bool fadeIn = true;

    public void Start()
    {
        if (!fadeIn)
        {
            fadeBackground.gameObject.SetActive(true);
            StartCoroutine(FadeOut(fadeTime));
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Play()
    {
        if (fadeIn)
            StartCoroutine(FadeIn(fadeTime));
    }

    private IEnumerator FadeOut(float duration)
    {
        yield return new WaitForSeconds(waitTime);

        fadeBackground.gameObject.SetActive(true);
        fadeBackground.alpha = 1;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / duration);
            fadeBackground.alpha = alpha;
            Fading?.Invoke(1 - alpha);
            yield return null;
        }

        fadeBackground.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(float duration)
    {
        fadeBackground.gameObject.SetActive(true);
        fadeBackground.alpha = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            fadeBackground.alpha = alpha;
            Fading?.Invoke(1 - alpha);
            yield return null;
        }

        if (sceneName != "")
        {
            yield return new WaitForSeconds(waitTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
