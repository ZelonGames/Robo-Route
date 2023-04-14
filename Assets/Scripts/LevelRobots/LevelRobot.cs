using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class LevelRobot
{
    [JsonProperty("prefab")]
    public GameObject Prefab { get; protected set; }

    [JsonIgnore]
    public GameObject spawnedGameObject;
}
