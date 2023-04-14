using UnityEngine;

public class Platform : LevelComponent
{
    public Platform(CustomPosition startingPosition, bool isFlipped, bool canMove) :
        base(startingPosition, isFlipped, canMove)
    {
    }

    public Platform() : base() { 
    }

    public override void LoadPrefab()
    {
        Prefab = Resources.Load<GameObject>("Prefabs/Level Components/Platform");
    }
}
