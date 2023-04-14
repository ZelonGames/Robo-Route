using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spiderweb : LevelComponent
{
    public Spiderweb(CustomPosition startingPosition, bool isFlipped, bool canMove) : 
        base(startingPosition, isFlipped, canMove)
    {
    }


    public Spiderweb():base()
    {

    }


    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Spiderweb");
    }
}