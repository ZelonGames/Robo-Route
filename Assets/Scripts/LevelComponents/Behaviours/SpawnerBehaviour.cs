using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private RobotSpawner robotSpawner;

    private void Awake()
    {
        if (!GameHelper.IsUsingMapEditor())
        {
            Destroy(GetComponent<LevelComponentRemover>());
            Destroy(GetComponent<ItemMover>());
        }
    }

    private void Start()
    {
        robotSpawner = GetComponent<RobotSpawner>();

        if (TryGetComponent<Unlocker>(out var unlocker))
            unlocker.Unlock += OnUnlock;
    }

    private void OnUnlock()
    {
        robotSpawner.enabled = true;
        robotSpawner.StartSpawning();
    }
}
