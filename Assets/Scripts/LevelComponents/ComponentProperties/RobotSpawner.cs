using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[SerializeField]
public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private SpriteFlipper spriteFlipper;

    public float launchSpeed = 2f;
    public float spawnTimeInSeconds = 1;
    public int robotsToSpawn = 1;

    private LevelController levelController;
    private GameObject robotsGameObject;
    private readonly List<Robot> spawnedRobots = new List<Robot>();

    private float lastSpawnedTime;
    private float? stopTime = null;
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
        if (!enabled || robotsToSpawn == 0)
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

    public void ContinueSpawning()
    {
        StartCoroutine(CountTimer());
    }

    public void StopSpawning()
    {
        stopTime = Time.fixedTime;
        enabled = false;
    }

    private IEnumerator CountTimer()
    {
        while (true)
        {
            if (stopTime.HasValue)
            {
                yield return new WaitForSeconds(spawnTimeInSeconds - (stopTime.Value - lastSpawnedTime));
                stopTime = null;
            }
            else
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
        lastSpawnedTime = Time.fixedTime;
    }

    private void UpdateText()
    {
        int spawnedCount = spawnedRobots == null ? 0 : spawnedRobots.Count;
        textMeshPro.text = spawnedCount + " / " + robotsToSpawn;
    }
}
