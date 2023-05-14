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

    public float launchSpeed = 2f;
    public float spawnTimeInSeconds = 1;
    public int robotsToSpawn = 1;
    public bool isLocked = false;

    private LevelController levelController;
    private GameObject robotsGameObject;

    private readonly List<Robot> spawnedRobots = new List<Robot>();
    private bool startedTimer = false;


    private void Start()
    {
        robotsGameObject = GameObject.Find("Robots");
        var levelControllerGameObject = GameObject.Find("LevelController");
        if (levelControllerGameObject != null)
        {
            levelController = levelControllerGameObject.GetComponent<LevelController>();
            levelController.FailedLevel += LevelController_FailedLevel;
        }

        if (testMode)
            StartSpawning();

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
        UpdateText();
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
