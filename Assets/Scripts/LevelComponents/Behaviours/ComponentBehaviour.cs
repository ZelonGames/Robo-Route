using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBehaviour : MonoBehaviour
{
    public LevelComponent levelComponent;

    void Start()
    {
        Type componentType = Type.GetType(gameObject.name);
        levelComponent = (LevelComponent)Activator.CreateInstance(componentType);
    }
}
