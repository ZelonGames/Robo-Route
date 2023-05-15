using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMousePlacer : MonoBehaviour
{
    private ItemMover itemMover;

    private void Start()
    {
        itemMover = gameObject.GetComponent<ItemMover>();
        itemMover.canMove = true;
        itemMover.SetDragging(true);
        itemMover.UpdateMaterial();
        itemMover.FinishedMovingItem += ItemMover_FinishedMovingItem;
    }

    private void ItemMover_FinishedMovingItem()
    {
        if (!GameHelper.IsUsingMapEditor())
            gameObject.transform.SetParent(GameObject.Find("AddedObjects").transform);

        Destroy(this);
    }

    private void OnDestroy()
    {
        itemMover.FinishedMovingItem -= ItemMover_FinishedMovingItem;
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 snappedPosition = GridHelper.SnapToGrid(mousePosition);
        gameObject.transform.position = snappedPosition;
    }
}
