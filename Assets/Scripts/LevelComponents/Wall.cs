using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Wall : LevelComponent
{
    public Wall(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
    }

    public Wall() : base()
    {
    }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Wall");
    }
}