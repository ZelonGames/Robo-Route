using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObjectQueue : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> gameObjects = new Queue<GameObject>();
    [SerializeField] private Queue<ItemMover> itemMovers = new Queue<ItemMover>();

    private GameObject placingGameObject = null;

    void Start()
    {
        ItemMover.FinishedMovingAnyItem += CursorObjectQueue_FinishedMovingItem;
    }

    public void AddGameObjectToQueue(GameObject largeObjectPrefab, ItemMover itemMover)
    {
        gameObjects.Enqueue(largeObjectPrefab);
        itemMovers.Enqueue(itemMover);

        if (placingGameObject == null)
            MoveNextGameObject();
    }

    private void CursorObjectQueue_FinishedMovingItem(GameObject movedGameObject)
    {
        if (movedGameObject != placingGameObject)
            return;

        if (gameObjects.Count > 0)
            MoveNextGameObject();
        else
            placingGameObject = null;
    }

    private void MoveNextGameObject()
    {
        GameObject addedGameObject = Instantiate(gameObjects.Dequeue());
        var itemMover = itemMovers.Dequeue();
        var addedGameObjectItemMover = addedGameObject.GetComponent<ItemMover>();

        addedGameObjectItemMover.usingLimitedMoves = itemMover.usingLimitedMoves;
        addedGameObjectItemMover.allowedMovesCount = addedGameObjectItemMover.initialAllowedMovesCount = itemMover.allowedMovesCount + 1;
        
        addedGameObject.AddComponent(typeof(ItemMousePlacer));
        placingGameObject = addedGameObject;
        placingGameObject.transform.SetParent(GameObject.Find("CursorObjectQueue").transform);
    }
}
