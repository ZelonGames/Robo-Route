using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public delegate void RequestAddItemHandler(GameObject parentObject, GameObject prefab);
    public event RequestAddItemHandler RequestedAddItem;
    public event Action FinishedGeneratingGameObjects;

    public delegate void LevelEventHandler(LevelBase currentLevel);
    public event LevelEventHandler FinishedLevel;
    public event LevelEventHandler FailedLevel;

    [SerializeField] private GameObject cursorObjectQueue;
    [SerializeField] private GameObject robots;
    [SerializeField] private GameObject addedObjects;
    [SerializeField] private GameObject movedObjects;
    [SerializeField] private GameObject backupGridWorld;
    [SerializeField] private GameObject gridWorld;

    public int goalsToReach = 0;
    public int completedGoals = 0;

    private bool isGameReset = true;

    void Start()
    {
        completedGoals = 0;

        ItemAdder.TryAddItem += ItemAdder_TryAddItem;

        EnteredGoalDetector.GoalEntered += EnteredGoalDetector_GoalEntered;
        EnteredGoalDetector.ReachedRequirement += EnteredGoalDetector_ReachedRequirement;
        RobotDamager.DestroyedRobot += RobotDamager_DestroyedRobot;
        if (GameController.gameController != null)
            GameController.gameController.StartedLevel += GameController_StartedLevel;

        foreach (Transform child in gridWorld.transform)
        {
            // Get a reference to the prefab that was used to create the original GameObject
            // Instantiate a new copy of the prefab
            GameObject levelComponent = Instantiate(child.gameObject);

            // Set the position, rotation, and other properties of the new GameObject to match the original GameObject
            levelComponent.transform.SetParent(backupGridWorld.transform);
            levelComponent.transform.position = child.gameObject.transform.position;
            levelComponent.transform.rotation = child.gameObject.transform.rotation;
            levelComponent.transform.localScale = child.gameObject.transform.localScale;
            levelComponent.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        ItemAdder.TryAddItem -= ItemAdder_TryAddItem;
        EnteredGoalDetector.GoalEntered -= EnteredGoalDetector_GoalEntered;

        RobotDamager.DestroyedRobot -= RobotDamager_DestroyedRobot;
        if (GameController.gameController != null)
            GameController.gameController.StartedLevel -= GameController_StartedLevel;
    }

    private void GameController_StartedLevel()
    {
        RestartLevel();
        FinishedGeneratingGameObjects?.Invoke();
    }

    private void RobotDamager_DestroyedRobot(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private void EnteredGoalDetector_ReachedRequirement(EnteredGoalDetector enteredGoalDetector)
    {
        completedGoals++;
        if (completedGoals >= goalsToReach)
        {
            completedGoals = 0;
            FinishedLevel?.Invoke(null);
        }
    }

    private void EnteredGoalDetector_GoalEntered(int savedRobots, int requiredRobotsToSave)
    {
    }

    public void RestartLevel()
    {
        completedGoals = 0;
        foreach (var enteredGoalDetector in FindObjectsOfType<EnteredGoalDetector>())
            enteredGoalDetector.ResetStats();

        foreach (Transform child in gridWorld.transform)
            Destroy(child.gameObject);

        foreach (Transform child in cursorObjectQueue.transform)
        {
            if (child.gameObject.name != "UI")
                Destroy(child.gameObject);
        }

        foreach (Transform child in robots.transform)
            Destroy(child.gameObject);

        foreach (Transform child in addedObjects.transform)
            Destroy(child.gameObject);

        foreach (Transform child in movedObjects.transform)
            Destroy(child.gameObject);

        foreach (Transform child in backupGridWorld.transform)
        {
            GameObject levelComponent = Instantiate(child.gameObject);
            levelComponent.transform.SetParent(gridWorld.transform);
            levelComponent.SetActive(true);
        }

        FindObjectOfType<TimeStopper>().ResetStates();
        FindObjectOfType<CursorObjectQueue>().Reset();

        isGameReset = true;

        StartLevel();
    }

    public void StartLevel()
    {
        foreach (RobotSpawner robotSpawner in FindObjectsOfType<RobotSpawner>())
            robotSpawner.StartSpawning();

        isGameReset = false;
    }

    private void ItemAdder_TryAddItem(GameObject addedObject)
    {
        bool shouldRequestAddItem = false;

        if (GameHelper.IsUsingMapEditor())
            shouldRequestAddItem = true;

        if (shouldRequestAddItem)
        {
            var prefab = addedObject.GetComponent<ComponentBehaviour>().levelComponent.Prefab;
            RequestedAddItem?.Invoke(addedObject, prefab);
        }
    }
}
