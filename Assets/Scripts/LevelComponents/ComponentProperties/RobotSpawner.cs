using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[SerializeField]
public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public float launchSpeed = 2f;
    public float spawnTimeInSeconds = 1;
    public int robotsToSpawn = 1;

    private LevelController levelController;
    private GameObject robotsGameObject;
    private readonly List<Robot> spawnedRobots = new List<Robot>();

    private float firstStopTime;
    private float? lastStopTime = null;
    private float timePassed = 0;
    private bool startedTimer = false;

    private bool IsStopped => lastStopTime.HasValue;

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
        firstStopTime = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
        StartCoroutine(CountTimer());
    }

    public void StopSpawning()
    {
        lastStopTime = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
        timePassed += (lastStopTime.Value - firstStopTime) / 1000f;

        StopAllCoroutines();
        enabled = false;
    }

    private IEnumerator CountTimer()
    {
        while (!IsStopped)
        {
            yield return new WaitForSeconds(spawnTimeInSeconds);

            if (spawnedRobots.Count < robotsToSpawn)
                SpawnRobot();
        }

        if (IsStopped)
        {
            yield return new WaitForSeconds(spawnTimeInSeconds - timePassed);

            if (spawnedRobots.Count < robotsToSpawn)
            {
                SpawnRobot();
                lastStopTime = null;
                StartCoroutine(CountTimer());
            }
        }
    }

    private void SpawnRobot()
    {
        var robot = new Robot();
        robot.spawnedGameObject = Instantiate(robot.Prefab);
        robot.spawnedGameObject.transform.position = gameObject.transform.position;

        float velocity = spriteRenderer.flipX ? -launchSpeed : launchSpeed;
        robot.spawnedGameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0);
        robot.spawnedGameObject.transform.SetParent(GameObject.Find("Robots").transform);
        spawnedRobots.Add(robot);
        UpdateText();
        firstStopTime = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
        timePassed = 0;
    }

    private void UpdateText()
    {
        int spawnedCount = spawnedRobots == null ? 0 : spawnedRobots.Count;
        textMeshPro.text = spawnedCount + " / " + robotsToSpawn;
    }
}
