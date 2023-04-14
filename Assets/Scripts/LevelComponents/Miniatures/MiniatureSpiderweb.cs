using UnityEngine;


public class MiniatureSpiderweb : LevelComponent
{
    public MiniatureSpiderweb(CustomPosition startingPosition, bool isFlipped, bool canMove) : 
        base(startingPosition, isFlipped, canMove)
    {
    }

    public MiniatureSpiderweb() : base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniatureSpiderweb");
    }
}