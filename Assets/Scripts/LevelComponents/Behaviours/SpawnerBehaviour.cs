using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    private void Awake()
    {
        if (!GameHelper.IsUsingMapEditor())
        {
            Destroy(GetComponent<LevelComponentRemover>());
            Destroy(GetComponent<ItemMover>());
        }
    }
}
