using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip robotLanding;
    [SerializeField] private AudioClip enteringPortal;

    private void Start()
    {
        FallingCollision.RobotLanded += PlayRobotLandingSound;
        EnteredGoalDetector.GoalEntered += PlayEnteringPortal;
    }

    private void OnDestroy()
    {
        if (audioSource != null)
            audioSource.Stop();
        
        FallingCollision.RobotLanded -= PlayRobotLandingSound;
        EnteredGoalDetector.GoalEntered -= PlayEnteringPortal;
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
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
