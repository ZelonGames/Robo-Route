using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWorldSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shakingButtonSound;
    [SerializeField] private AudioClip stoppedShakingButtonSound;

    private void Start()
    {
        ButtonLevel.MouseEnter += PlayShakingSound;
        ButtonLevel.StoppedShaking += PlayStoppedShakingSound;
    }


    private void OnDestroy()
    {
        ButtonLevel.MouseEnter -= PlayShakingSound;
        ButtonLevel.StoppedShaking -= PlayStoppedShakingSound;
    }

    private void PlayStoppedShakingSound(ButtonLevel obj)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = stoppedShakingButtonSound;
        audioSource.Play();
    }

    private void PlayShakingSound(ButtonLevel obj)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = shakingButtonSound;
        audioSource.Play();
    }
}
