using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public delegate void RequestAddItemHandler(GameObject parentObject, GameObject prefab);
    public event RequestAddItemHandler RequestedAddItem;

    public delegate void LevelEventHandler(LevelBase currentLevel);
    public event LevelEventHandler FinishedLevel;
    public event LevelEventHandler FailedLevel;

    public LevelBase currentLevel;
    public int goalsToReach = 0;
    public int completedGoals = 0;

    void Start()
    {
        completedGoals = 0;
        if (!GameHelper.IsUsingMapEditor())
        {
            currentLevel = LevelEditor.Load("level" + ButtonLevel.selectedLevelNumber);

            if (currentLevel != null)
            {
                goalsToReach = currentLevel.startingLevelComponents.Where(x => x.GetType() == typeof(Goal)).Count();

                for (int i = 0; i < currentLevel.startingLevelComponents.Count; i++)
                {
                    var levelComponent = currentLevel.startingLevelComponents[i];
                    var itemMover = currentLevel.startingLevelComponents[i].spawnedGameObject.GetComponent<ItemMover>();
                    GameController.hasStartedGame = false;

                    Destroy(currentLevel.startingLevelComponents[i].spawnedGameObject.GetComponent<LevelComponentRemover>());
                }
            }
        }
        else
            currentLevel = new LevelBase();

        ItemAdder.TryAddItem += ItemAdder_TryAddItem;

        EnteredGoalDetector.GoalEntered += EnteredGoalDetector_GoalEntered;
        EnteredGoalDetector.ReachedRequirement += EnteredGoalDetector_ReachedRequirement;
        RobotDamager.DestroyedRobot += RobotDamager_DestroyedRobot;
        if (GameController.gameController != null)
            GameController.gameController.StartedLevel += GameController_StartedLevel;
    }

    private void OnDestroy()
    {
        currentLevel = null;
        ItemAdder.TryAddItem -= ItemAdder_TryAddItem;
        EnteredGoalDetector.GoalEntered -= EnteredGoalDetector_GoalEntered;

        RobotDamager.DestroyedRobot -= RobotDamager_DestroyedRobot;
        if (GameController.gameController != null)
            GameController.gameController.StartedLevel -= GameController_StartedLevel;
    }

    private void GameController_StartedLevel()
    {
        RestartLevel();
    }

    private void RobotDamager_DestroyedRobot(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    private void EnteredGoalDetector_ReachedRequirement(EnteredGoalDetector enteredGoalDetector)
    {
        completedGoals++;
        if (currentLevel != null &&
            completedGoals >= goalsToReach)
        {
            completedGoals = 0;
            FinishedLevel?.Invoke(currentLevel);
        }
    }

    private void EnteredGoalDetector_GoalEntered(int savedRobots, int requiredRobotsToSave)
    {
    }

    public void RestartLevel()
    {
        foreach (var miniature in currentLevel.miniatures)
        {
            if (miniature.spawnedGameObject == null)
            {
                miniature.spawnedGameObject = Instantiate(miniature.Prefab);
                miniature.spawnedGameObject.transform.position =
                    new Vector2(miniature.startingPosition.x, miniature.startingPosition.y);
            }
        }

        completedGoals = 0;
        foreach (var enteredGoalDetector in FindObjectsOfType<EnteredGoalDetector>())
            enteredGoalDetector.ResetStats();

        foreach (Transform child in GameObject.Find("CursorObjectQueue").transform)
            Destroy(child.gameObject);

        foreach (Transform child in GameObject.Find("Robots").transform)
            Destroy(child.gameObject);

        foreach (Transform child in GameObject.Find("AddedObjects").transform)
            Destroy(child.gameObject);

        foreach (Transform child in GameObject.Find("MovedObjects").transform)
        {
            var itemMover = child.gameObject.GetComponent<ItemMover>();
            child.gameObject.transform.position = itemMover.SpawnPosition;
        }
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
