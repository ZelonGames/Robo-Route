using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BouncingPlatform : LevelComponent
{
    public BouncingPlatform(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
    }

    public BouncingPlatform():base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/BouncingPlatform");
    }
}