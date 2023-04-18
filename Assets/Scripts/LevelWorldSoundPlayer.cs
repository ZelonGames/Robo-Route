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

    private void PlaySound(AudioClip audioClip)
    {

        audioSource.PlayOneShot(audioClip);
    }

    private void PlayStoppedShakingSound(ButtonLevel obj)
    {
        PlaySound(stoppedShakingButtonSound);
    }

    private void PlayShakingSound(ButtonLevel obj)
    {
        PlaySound(shakingButtonSound);
    }
}
