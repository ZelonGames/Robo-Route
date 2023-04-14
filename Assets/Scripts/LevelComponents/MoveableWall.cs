using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveableWall : LevelComponent
{
    public MoveableWall(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, true)
    {
    }

    public MoveableWall() : base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/MoveableWall");
    }
}