using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource rain;
    [SerializeField] private AudioSource wind;
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip robotLanding;
    [SerializeField] private AudioClip enteringPortal;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip clickLowSound;

    private bool shouldPlaySounds = true;
    private float lastPlayTime = 0;

    private void Start()
    {
        FallingCollision.RobotLanded += PlayRobotLandingSound;
        EnteredGoalDetector.GoalEntered += PlayEnteringPortal;
        ItemMiniature.CollectedItem += PlayCollectedItem;
        ItemMover.ChangedPosition += PlayMovedItem;
        ItemMover.FinishedMovingAnyItem += PlayClickLowSound;
        ItemMover.StartedMovingAnyItem += PlayClickSound;
        SceneFader.Fading += SceneFader_Fading;
        BouncingPlatformBehaviour.LaunchedRobot += OnPlayLaunchSound;
    }

    private void OnDestroy()
    {
        if (audioSource != null)
            audioSource.Stop();

        FallingCollision.RobotLanded -= PlayRobotLandingSound;
        EnteredGoalDetector.GoalEntered -= PlayEnteringPortal;
        ItemMiniature.CollectedItem -= PlayCollectedItem;
        ItemMover.ChangedPosition -= PlayMovedItem;
        ItemMover.FinishedMovingAnyItem -= PlayClickLowSound;
        ItemMover.StartedMovingAnyItem -= PlayClickSound;
        SceneFader.Fading -= SceneFader_Fading;
        BouncingPlatformBehaviour.LaunchedRobot -= OnPlayLaunchSound;
    }

    private void OnPlayLaunchSound()
    {
        PlaySound(launchSound);
    }

    private void SceneFader_Fading(float volume)
    {
        shouldPlaySounds = false;
        rain.volume = volume * 0.4f;
        wind.volume = volume * 0.4f;
        if (volume >= 1)
            shouldPlaySounds = true;
    }

    private void PlayClickLowSound(GameObject movedGameObject)
    {
        PlaySound(clickLowSound, 0.5f);
    }

    private void PlayClickSound(GameObject movedGameObject)
    {
        PlaySound(clickSound);
    }

    private void PlayMovedItem()
    {
        if (Time.time - lastPlayTime > 0.05f)
        {
            PlaySound(clickSound, 0.3f);
            lastPlayTime = Time.time;
        }
    }

    private void PlayCollectedItem()
    {
        PlaySound(clickSound);
    }

    private void PlaySound(AudioClip audioClip, float volume = 1f)
    {
        if (shouldPlaySounds)
            audioSource.PlayOneShot(audioClip, volume);
    }

    public void PlayRobotLandingSound()
    {
        PlaySound(robotLanding);
    }

    public void PlayEnteringPortal(int savedRobots, int requiredRobotsToSave)
    {
        PlaySound(enteringPortal);
    }
}
