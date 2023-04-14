using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObjectQueue : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> gameObjects = new Queue<GameObject>();

    private GameObject placingGameObject = null;

    void Start()
    {
        ItemMover.FinishedMovingAnyItem += CursorObjectQueue_FinishedMovingItem;
    }

    public void AddGameObject(GameObject gameObject)
    {
        gameObjects.Enqueue(gameObject);

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
        addedGameObject.AddComponent(typeof(ItemMousePlacer));
        placingGameObject = addedGameObject;
        placingGameObject.transform.SetParent(GameObject.Find("CursorObjectQueue").transform);
    }
}
