using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private Squash squash;

    private void Start()
    {
        squash = GetComponent<Squash>();

        if (!GameHelper.IsUsingMapEditor())
        {
            Destroy(GetComponent<LevelComponentRemover>());
            Destroy(GetComponent<ItemMover>());
        }

        GetComponent<EnteredGoalDetector>().GoalEnteredSelf += OnPlaySquash;
    }

    private void OnPlaySquash()
    {
        squash.Play();
    }
}
