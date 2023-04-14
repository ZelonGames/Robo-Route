using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMousePlacer : MonoBehaviour
{
    private void Start()
    {
        var itemMover = gameObject.GetComponent<ItemMover>();
        itemMover.canMove = true;
        itemMover.SetDragging(true);
        itemMover.UpdateMaterial();
        itemMover.FinishedMovingItem += ItemMover_FinishedMovingItem;
    }

    private void ItemMover_FinishedMovingItem(GameObject movedGameObject)
    {
        Destroy(this);
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = GridHelper.SnapToGrid(mousePosition);
        gameObject.transform.position = snappedPosition;
    }
}
