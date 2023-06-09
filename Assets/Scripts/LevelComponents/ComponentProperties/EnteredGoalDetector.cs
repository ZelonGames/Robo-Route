using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int requiredRobotsToSave;

    private readonly Dictionary<Collider2D, CollidingRobots> collidingRobots = new();
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateText();
    }

    private void OnValidate()
    {
        try
        {
            UpdateText();
        }
        catch { }
    }

    public void FixedUpdate()
    {
        foreach (var collidingRobot in collidingRobots.Values.ToList())
        {
            float currentDistance = Vector2.Distance(
                collidingRobot.collider2D.gameObject.transform.position,
                gameObject.transform.position);

            collidingRobot.spriteRenderer.color = new Color(
                collidingRobot.spriteRenderer.color.r,
                collidingRobot.spriteRenderer.color.g,
                collidingRobot.spriteRenderer.color.b,
                currentDistance / collidingRobot.startDistance);

            var robotBehaviour = collidingRobot.collider2D.gameObject.GetComponent<RobotBehaviour>();
            bool enteringCorrectSide = 
                spriteRenderer.flipX && robotBehaviour.Velocity.x > 0 ||
                !spriteRenderer.flipX && robotBehaviour.Velocity.x < 0;

            if (currentDistance <= 0.2f && enteringCorrectSide)
            {
                collidingRobots.Remove(collidingRobot.collider2D);
                Destroy(collidingRobot.collider2D.gameObject);
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
            collidingRobots.Add(collision, new CollidingRobots()
            {
                collider2D = collision,
                spriteRenderer = collision.GetComponent<SpriteRenderer>(),
                startDistance = Vector2.Distance(collision.gameObject.transform.position, gameObject.transform.position),
            });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Robot") && collidingRobots.ContainsKey(collision))
            collidingRobots.Remove(collision);
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
