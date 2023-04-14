using UnityEngine;


public class MiniatureBouncingPlatform : LevelComponent
{
    public MiniatureBouncingPlatform(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
        LoadPrefab();
    }

    public MiniatureBouncingPlatform() : base()
    {
    }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniatureBouncingPlatform");
    }
}