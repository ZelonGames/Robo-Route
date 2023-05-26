using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    [SerializeField] private GameObject robots;
    private readonly Dictionary<GameObject, (RigidbodyConstraints2D constraints, Vector2 velocity)> robotsState = new();
    private readonly Dictionary<RobotSpawner, bool> spawnersState = new();

    private bool isTimeStopped = false;

    void Start()
    {
        GameController.gameController.StoppedLevel += ResetStates;
        GameController.gameController.StartedLevel += ResetStates;
    }

    private void OnDestroy()
    {
        GameController.gameController.StoppedLevel -= ResetStates;
        GameController.gameController.StartedLevel -= ResetStates;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.hasStartedGame || !Input.GetMouseButtonUp(1))
            return;

        if (!isTimeStopped)
        {
            var spawners = FindObjectsByType<RobotSpawner>(FindObjectsSortMode.None);
            foreach (var spawner in spawners)
            {
                spawnersState.Add(spawner, spawner.enabled);
                spawner.StopSpawning();
            }

            foreach (Transform robot in robots.transform)
            {
                var rigidBody = robot.GetComponent<Rigidbody2D>();
                robotsState.Add(robot.gameObject, new(rigidBody.constraints, rigidBody.velocity));
                rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            isTimeStopped = true;
        }
        else
            ResetStates();
    }

    public void ResetStates()
    {
        foreach (var state in spawnersState)
        {
            if (state.Value)
            {
                state.Key.enabled = true;
                state.Key.ContinueSpawning();
            }
        }

        foreach (var state in robotsState)
        {
            var rigidBody = state.Key.GetComponent<Rigidbody2D>();
            rigidBody.velocity = state.Value.velocity;
            rigidBody.constraints = state.Value.constraints;
        }

        robotsState.Clear();
        spawnersState.Clear();
        isTimeStopped = false;
    }
}
