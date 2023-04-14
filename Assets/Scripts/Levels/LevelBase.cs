using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class LevelBase
{
    public List<LevelComponent> startingLevelComponents = new List<LevelComponent>();
    [JsonIgnore]
    public List<LevelComponent> miniatures = new List<LevelComponent>();

    public string levelName;
    public int levelNumber;

    public float? highestPosition = null;
    public float? lowestPosition = null;

    public LevelBase()
    {
    }

    public void Update()
    {
        foreach (var levelComponent in startingLevelComponents)
        {
            if (levelComponent.spawnedGameObject != null)
            {
                switch (levelComponent.typeName)
                {
                    case nameof(Spawner):
                        (levelComponent as Spawner).spawnTimeInSeconds = levelComponent.spawnedGameObject.GetComponent<RobotSpawner>().spawnTimeInSeconds;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
