using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LevelComponent
{
    public string typeName;
    public Dictionary<string, object> args = new Dictionary<string, object>();

    [JsonIgnore] public GameObject spawnedGameObject;
    [JsonIgnore] public CustomPosition startingPosition;
    [JsonIgnore] public bool isFlipped = false;
    [JsonIgnore] public bool canMove = false;
    [JsonIgnore] public bool usingLimitedMoves = false;
    [JsonIgnore] public int allowedMovesCount = 0;

    [JsonIgnore] public GameObject Prefab { get; protected set; }

    public LevelComponent(CustomPosition startingPosition, bool isFlipped, bool canMove)
    {
        this.startingPosition = startingPosition;
        this.isFlipped = isFlipped;
        this.canMove = canMove;

        LoadPrefab();
    }

    public LevelComponent()
    {
        LoadPrefab();
    }

    public object GetArgument(string name) => args[name];

    public virtual void InstantiateWithArgs(Dictionary<string, object> args)
    {
        this.args = args;
        startingPosition = JsonConvert.DeserializeObject<CustomPosition>(GetArgument(nameof(startingPosition)).ToString());
        if (args.ContainsKey(nameof(isFlipped)))
            isFlipped = (bool)GetArgument(nameof(isFlipped));
        if (args.ContainsKey(nameof(canMove)))
            canMove = (bool)GetArgument(nameof(canMove));
        if (args.ContainsKey(nameof(allowedMovesCount)))
            allowedMovesCount = Convert.ToInt32(args[nameof(allowedMovesCount)]);
        if (args.ContainsKey(nameof(usingLimitedMoves)))
            usingLimitedMoves = (bool)args[nameof(usingLimitedMoves)];
    }

    public virtual void LoadPrefab()
    { }

}
