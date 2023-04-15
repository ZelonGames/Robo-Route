using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private SpriteFlipper spriteFlipper;
    [SerializeField] private bool testMode = false;

    [HideInInspector]
    public LevelComponentSettings levelComponentSettings;

    public Spawner spawnerRelation;

    public float launchSpeed = 2f;
    public float spawnTimeInSeconds = 1;
    public int robotsToSpawn = 1;

    private LevelControllerNew levelController;
    private GameObject robotsGameObject;

    private readonly List<Robot> spawnedRobots = new List<Robot>();
    private bool startedTimer = false;


    private void Start()
    {
        robotsGameObject = GameObject.Find("Robots");
        var levelControllerGameObject = GameObject.Find("LevelController");
        if (levelControllerGameObject != null)
        {
            levelController = levelControllerGameObject.GetComponent<LevelControllerNew>();
            levelController.FailedLevel += LevelController_FailedLevel;
        }

        if (testMode)
            StartSpawning();

        levelComponentSettings = gameObject.GetComponent<LevelComponentSettings>();

        if (levelComponentSettings != null)
        {
            if (levelComponentSettings.settings.ContainsKey(nameof(robotsToSpawn)))
                robotsToSpawn = Convert.ToInt32(levelComponentSettings.settings[nameof(robotsToSpawn)]);
            if (levelComponentSettings.settings.ContainsKey(nameof(spawnTimeInSeconds)))
                spawnTimeInSeconds = Convert.ToSingle(levelComponentSettings.settings[nameof(spawnTimeInSeconds)]);

            OnValidate();
        }

        UpdateText();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnValidate()
    {
        try
        {
            UpdateText();

            if (spawnerRelation != null)
            {
                spawnerRelation.spawnTimeInSeconds = spawnTimeInSeconds;
                spawnerRelation.robotsToSpawn = robotsToSpawn;
            }

            if (levelComponentSettings != null)
            {
                levelComponentSettings.UpdateSetting(nameof(spawnTimeInSeconds), spawnTimeInSeconds);
                levelComponentSettings.UpdateSetting(nameof(robotsToSpawn), robotsToSpawn);
            }
        }
        catch { }
    }

    private void LevelController_FailedLevel(LevelBase currentLevel)
    {
        StopAllCoroutines();
    }

    public void StartSpawning()
    {
        if (robotsToSpawn == 0)
            return;

        foreach (var spawnedRobot in spawnedRobots)
        {
            if (spawnedRobot.spawnedGameObject != null)
                Destroy(spawnedRobot.spawnedGameObject);
        }
        spawnedRobots.Clear();

        SpawnRobot();
        StopAllCoroutines();
        StartCoroutine(CountTimer());
        startedTimer = true;
    }

    private IEnumerator CountTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimeInSeconds);
            if (spawnedRobots.Count < robotsToSpawn)
                SpawnRobot();
        }
    }

    private void SpawnRobot()
    {
        var robot = new Robot();
        robot.spawnedGameObject = Instantiate(robot.Prefab);
        robot.spawnedGameObject.transform.position = gameObject.transform.position;

        robot.spawnedGameObject.GetComponent<Rigidbody2D>().velocity =
            new Vector2(spriteFlipper.isFlipped ? -launchSpeed : launchSpeed, 0);
        robot.spawnedGameObject.transform.SetParent(GameObject.Find("Robots").transform);
        spawnedRobots.Add(robot);
        UpdateText();
    }

    private void UpdateText()
    {
        int spawnedCount = spawnedRobots == null ? 0 : spawnedRobots.Count;
        textMeshPro.text = spawnedCount + " / " + robotsToSpawn;
    }
}
