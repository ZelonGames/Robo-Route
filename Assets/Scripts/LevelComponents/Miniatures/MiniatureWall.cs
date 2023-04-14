using UnityEngine;


public class MiniatureWall : LevelComponent
{
    public MiniatureWall(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
    }

    public MiniatureWall():base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniatureWall");
    }
}