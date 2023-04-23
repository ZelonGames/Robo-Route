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

    [SerializeField] private TextMeshPro textMeshPro;

    public LevelComponentSettings levelComponentSettings;

    public int requiredRobotsToSave;

    private Queue<Collider2D> collidingRobots = new();
    private Collider2D currentCollidingRobot = null;

    private SpriteRenderer currentCollidingRobotSpriteRenderer = null;
    private float collidingRobotStartDistance;
    private int savedRobots;
    private bool hasInvokedReachedRequirement = false;

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

    public void FixedUpdate()
    {
        if (currentCollidingRobot != null)
        {
            float currentDistance = Vector2.Distance(
                currentCollidingRobot.gameObject.transform.position,
                gameObject.transform.position);

            currentCollidingRobotSpriteRenderer.color = new Color(
                currentCollidingRobotSpriteRenderer.color.r,
                currentCollidingRobotSpriteRenderer.color.g,
                currentCollidingRobotSpriteRenderer.color.b,
                 currentDistance / collidingRobotStartDistance);

            if (currentDistance <= 0.2f)
            {
                Destroy(currentCollidingRobot.gameObject);
                currentCollidingRobot = null;
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
        else
            collidingRobots.TryDequeue(out currentCollidingRobot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Robot"))
        {
            collidingRobots.Enqueue(collision);
            if (currentCollidingRobot == null)
            {
                currentCollidingRobot = collidingRobots.Dequeue();
                currentCollidingRobotSpriteRenderer = currentCollidingRobot.GetComponent<SpriteRenderer>();
                collidingRobotStartDistance = Vector2.Distance(
                    currentCollidingRobot.gameObject.transform.position,
                    gameObject.transform.position);
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
