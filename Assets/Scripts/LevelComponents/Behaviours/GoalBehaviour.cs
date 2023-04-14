using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    private void Start()
    {
        if (!GameHelper.IsUsingMapEditor())
        {
            Destroy(GetComponent<LevelComponentRemover>());
            Destroy(GetComponent<ItemMover>());
        }
    }
}
