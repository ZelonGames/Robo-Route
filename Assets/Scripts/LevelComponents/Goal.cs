using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Goal : LevelComponent
{
    [JsonIgnore] public int requiredRobotsToSave = 5;

    public Goal() : base()
    {
    }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Goal");
        if (isFlipped)
        {
            var spriteRenderer = Prefab.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = true;
        }
    }

    public override void InstantiateWithArgs(Dictionary<string, object> args)
    {
        base.InstantiateWithArgs(args);

        requiredRobotsToSave = Convert.ToInt32(GetArgument(nameof(requiredRobotsToSave)));
    }
}