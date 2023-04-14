using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : LevelRobot
{
    public Robot() : base()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Robot");
    }
}
