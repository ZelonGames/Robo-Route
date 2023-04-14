using UnityEngine;


public class MiniatureCannon : LevelComponent
{
    public MiniatureCannon(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
    }

    public MiniatureCannon() : base() { }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Miniatures/MiniatureCannon");
    }
}