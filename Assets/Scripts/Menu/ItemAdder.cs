using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemAdder : MonoBehaviour
{
    public delegate void TryAddItemEventHandler(GameObject addedObject);
    public static event TryAddItemEventHandler TryAddItem;

    public delegate void AddedItemEventHandler(ComponentBehaviour componentBehaviour, GameObject addedGameObject);
    public static event AddedItemEventHandler AddedItem;

    private LevelController levelController;
    private ClickDetector clickDetector;

    private GameObject addedGameObjectsParent = null;

    void Start()
    {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        clickDetector = GetComponent<ClickDetector>();

        if (levelController != null)
            levelController.RequestedAddItem += LevelController_RequestedAddItem;
        if (GameHelper.IsUsingMapEditor())
            clickDetector.ItemClicked += ClickDetector_ItemClicked;
        else
        {
            GameController.gameController.StartedLevel += GameController_StartedLevel;
            GameController.gameController.StoppedLevel += GameController_StoppedLevel;
        }
    }

    private void GameController_StoppedLevel()
    {
        clickDetector.ItemClicked -= ClickDetector_ItemClicked;
    }

    private void GameController_StartedLevel()
    {
        clickDetector.ItemClicked += ClickDetector_ItemClicked;
    }

    private void OnDestroy()
    {
        clickDetector.ItemClicked -= ClickDetector_ItemClicked;
        if (levelController != null)
            levelController.RequestedAddItem -= LevelController_RequestedAddItem;
        if (!GameHelper.IsUsingMapEditor())
            GameController.gameController.StartedLevel -= GameController_StartedLevel;
    }

    private void LevelController_RequestedAddItem(GameObject parentObject, GameObject prefab)
    {
        if (gameObject != parentObject)
            return;

        // Spawn the prefab and make it follow the mouse cursor.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gridPosition = GridHelper.SnapToGrid(mousePosition);

        GameObject addedGameObject = Instantiate(prefab, gridPosition, Quaternion.identity);
        if (GameHelper.IsUsingMapEditor())
            addedGameObject.AddComponent<LevelComponentSettings>();
        else
        {
            if (GameHelper.IsUsingMapEditor())
                addedGameObject.transform.SetParent(GameObject.Find("GridWorld").transform);
            else
            {
                if (addedGameObjectsParent == null)
                    addedGameObjectsParent = GameObject.Find("AddedObjects");
                addedGameObject.transform.SetParent(addedGameObjectsParent.transform);
            }
        }

        addedGameObject.GetComponent<ItemMover>().SetDragging(true);
        if (GameHelper.IsUsingMapEditor())
            addedGameObject.GetComponent<ItemMover>().canMove = false;
        AddedItem?.Invoke(GetComponent<ComponentBehaviour>(), addedGameObject);
    }

    private void ClickDetector_ItemClicked(GameObject clickedGameObject)
    {
        if (gameObject != clickedGameObject)
            return;

        TryAddItem?.Invoke(clickedGameObject);
    }
}
