using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Cannon : LevelComponent
{
    public Cannon(CustomPosition startingPosition, bool isFlipped, bool canMove) : 
        base(startingPosition, isFlipped, canMove)
    {
    }

    public Cannon():base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Cannon");
        if (isFlipped)
        {
            var spriteRenderer = Prefab.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }
    }
}