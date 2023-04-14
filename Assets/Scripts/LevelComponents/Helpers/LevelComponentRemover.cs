using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelComponentRemover : MonoBehaviour
{
    public delegate void DestroyedObjectHandler(GameObject destroyedGameObject);
    public static event DestroyedObjectHandler DestroyedObject;

    private void Start()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
            DestroyedObject?.Invoke(gameObject);
        }
    }
}
