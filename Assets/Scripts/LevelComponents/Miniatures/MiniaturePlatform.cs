using UnityEngine;


public class MiniaturePlatform : LevelComponent
{
    public MiniaturePlatform(CustomPosition startingPosition, bool isFlipped, bool canMove) : 
        base(startingPosition, isFlipped, canMove)
    {
    }

    public MiniaturePlatform() : base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniaturePlatform");
    }
}