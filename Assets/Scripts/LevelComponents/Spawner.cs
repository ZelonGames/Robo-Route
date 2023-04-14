using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : LevelComponent
{
    [JsonIgnore] public float spawnTimeInSeconds = 1;
    [JsonIgnore] public int robotsToSpawn = 5;

    public Spawner() : base()
    {
    }

    public override void InstantiateWithArgs(Dictionary<string, object> args)
    {
        base.InstantiateWithArgs(args);

        spawnTimeInSeconds = Convert.ToSingle(GetArgument(nameof(spawnTimeInSeconds)));
        robotsToSpawn = Convert.ToInt32(GetArgument(nameof(robotsToSpawn)));
    }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Spawner");
    }
}