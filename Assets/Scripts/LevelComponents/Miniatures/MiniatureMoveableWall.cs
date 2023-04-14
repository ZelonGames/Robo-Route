using UnityEngine;


public class MiniatureMoveableWall : LevelComponent
{
    public MiniatureMoveableWall(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, true)
    {
        LoadPrefab();
    }

    public MiniatureMoveableWall() : base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniatureMoveableWall");
    }
}