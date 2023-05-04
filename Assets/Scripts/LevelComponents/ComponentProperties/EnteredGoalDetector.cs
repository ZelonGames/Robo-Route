using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnteredGoalDetector : MonoBehaviour
{
    public delegate void GoalEnteredEventHandler(int savedRobots, int requiredRobotsToSave);
    public static event GoalEnteredEventHandler GoalEntered;
    public event Action GoalEnteredSelf;

    public delegate void ReachedRequirementEventHandler(EnteredGoalDetector enteredGoalDetector);
    public static event ReachedRequirementEventHandler ReachedRequirement;

    [SerializeField] private TextMeshPro textMeshPro;

    public LevelComponentSettings levelComponentSettings;

    public int requiredRobotsToSave;

    private readonly List<CollidingRobots> collidingRobots = new();

    private int savedRobots;
    private bool hasInvokedReachedRequirement = false;

    private class CollidingRobots
    {
        public Collider2D collider2D = null;
        public SpriteRenderer spriteRenderer = null;
        public float startDistance;
    }

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
        for (int i = 0; i < collidingRobots.Count; i++)
        {
            var currentCollidingRobot = collidingRobots[i];

            float currentDistance = Vector2.Distance(
                currentCollidingRobot.collider2D.gameObject.transform.position,
                gameObject.transform.position);

            currentCollidingRobot.spriteRenderer.color = new Color(
                currentCollidingRobot.spriteRenderer.color.r,
                currentCollidingRobot.spriteRenderer.color.g,
                currentCollidingRobot.spriteRenderer.color.b,
                 currentDistance / currentCollidingRobot.startDistance);

            if (currentDistance <= 0.2f)
            {
                collidingRobots.Remove(currentCollidingRobot);
                Destroy(currentCollidingRobot.collider2D.gameObject);
                currentCollidingRobot = null;
                savedRobots++;
                UpdateText();
                GoalEntered?.Invoke(savedRobots, requiredRobotsToSave);
                GoalEnteredSelf?.Invoke();
                if (!hasInvokedReachedRequirement && savedRobots >= requiredRobotsToSave)
                {
                    ReachedRequirement?.Invoke(this);
                    hasInvokedReachedRequirement = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Robot"))
        {
            collidingRobots.Add(new CollidingRobots()
            {
                collider2D = collision,
                spriteRenderer = collision.GetComponent<SpriteRenderer>(),
                startDistance = Vector2.Distance(collision.gameObject.transform.position, gameObject.transform.position),
            });
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
