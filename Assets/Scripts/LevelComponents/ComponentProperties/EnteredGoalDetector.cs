using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnteredGoalDetector : MonoBehaviour
{
    public delegate void GoalEnteredEventHandler(int savedRobots, int requiredRobotsToSave);
    public static event GoalEnteredEventHandler GoalEntered;

    public delegate void ReachedRequirementEventHandler(EnteredGoalDetector enteredGoalDetector);
    public static event ReachedRequirementEventHandler ReachedRequirement;

    public LevelComponentSettings levelComponentSettings;

    [SerializeField] private TextMeshPro textMeshPro;

    private int savedRobots;
    private bool hasInvokedReachedRequirement = false;
    public int requiredRobotsToSave;

    void Start()
    {
        levelComponentSettings = gameObject.GetComponent<LevelComponentSettings>();
        UpdateText();

        if (levelComponentSettings != null)
        {
            if (levelComponentSettings.settings.ContainsKey(nameof(requiredRobotsToSave)))
                requiredRobotsToSave = Convert.ToInt32(levelComponentSettings.settings[nameof(requiredRobotsToSave)]);

            OnValidate();
        }
    }

    private void OnValidate()
    {
        try
        {
            UpdateText();

            if (levelComponentSettings != null)
                levelComponentSettings.UpdateSetting(nameof(requiredRobotsToSave), requiredRobotsToSave);
        }
        catch { }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Robot")
        {
            Destroy(collision.gameObject);
            savedRobots++;
            UpdateText();
            GoalEntered?.Invoke(savedRobots, requiredRobotsToSave);
            if (!hasInvokedReachedRequirement && savedRobots >= requiredRobotsToSave)
            {
                ReachedRequirement?.Invoke(this);
                hasInvokedReachedRequirement = true;
            }
        }
    }

    public void ResetStats()
    {
        savedRobots = 0;
        hasInvokedReachedRequirement = false;
        UpdateText();
    }

    private void UpdateText()
    {
        textMeshPro.text = savedRobots + " / " + requiredRobotsToSave;
    }
}
