using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CursorObjectQueue : MonoBehaviour
{
    [SerializeField] private SpriteRenderer uiSpriteRenderer;

    private new Camera camera;
    private Queue<GameObject> itemIcons = new();
    private Queue<GameObject> gameObjects = new();
    private Queue<ItemMover> itemMovers = new();

    private GameObject placingGameObject = null;
    private Vector2 position = Vector2.zero;

    private float totalWidth;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        uiSpriteRenderer.enabled = false;
        ItemMover.FinishedMovingAnyItem += CursorObjectQueue_FinishedMovingItem;
    }

    private void OnDestroy()
    {
        ItemMover.FinishedMovingAnyItem -= CursorObjectQueue_FinishedMovingItem;
    }

    public void Update()
    {
        var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        position = mousePos;
        position.y = mousePos.y + 0.5f;
        //position.x = mousePos.x + totalWidth;
        transform.position = position;


        float totalItemWidth = 0;
        for (int i = 0; i < itemIcons.Count; i++)
        {
            float itemWidth = itemIcons.Skip(i).First().transform.GetComponent<SpriteRenderer>().bounds.size.x;
            totalItemWidth += itemWidth;
            float leftEdge = uiSpriteRenderer.gameObject.transform.position.x - uiSpriteRenderer.gameObject.transform.localScale.x * 0.5f + itemWidth * 0.5f;

            var spriteRenderer = itemIcons.Skip(i).First().GetComponent<SpriteRenderer>();
            Bounds spriteBounds = spriteRenderer.sprite.bounds;
            Transform spriteTransform = spriteRenderer.gameObject.transform;
            Vector2 spriteCenter = spriteBounds.center;
            float adjustedPivotY = spriteCenter.y * spriteTransform.localScale.y;

            itemIcons.Skip(i).First().transform.position = 
                new Vector2(leftEdge + totalItemWidth - itemWidth, transform.position.y - adjustedPivotY);
        }
    }

    public void Reset()
    {
        placingGameObject = null;
        uiSpriteRenderer.enabled = false;

        foreach (var itemIcon in itemIcons)
            Destroy(itemIcon);

        gameObjects.Clear();
        itemMovers.Clear();
        itemIcons.Clear();
    }

    public void AddGameObjectToQueue(GameObject largeObjectPrefab, ItemMover itemMover, bool instantiate = true)
    {
        gameObjects.Enqueue(largeObjectPrefab);
        itemMovers.Enqueue(itemMover);

        if (placingGameObject != null)
        {
            AddItemIcon(itemMover.gameObject);
            UpdateUIWidth();

            uiSpriteRenderer.enabled = true;
        }

        if (placingGameObject == null)
            MoveNextGameObject(instantiate);
    }

    private void CursorObjectQueue_FinishedMovingItem(GameObject movedGameObject)
    {
        if (movedGameObject != placingGameObject)
            return;

        if (gameObjects.Count > 0)
            MoveNextGameObject();
        else
            placingGameObject = null;

        if (itemIcons.Count > 0)
        {
            Destroy(itemIcons.Dequeue());
            UpdateUIWidth();
        }

        if (gameObjects.Count == 0)
            uiSpriteRenderer.enabled = false;
    }

    private void MoveNextGameObject(bool instantiate = true)
    {
        GameObject addedGameObject = instantiate ? Instantiate(gameObjects.Dequeue()) : gameObjects.Dequeue();
        var itemMover = itemMovers.Dequeue();
        var addedGameObjectItemMover = addedGameObject.GetComponent<ItemMover>();

        addedGameObjectItemMover.usingLimitedMoves = itemMover.usingLimitedMoves;
        addedGameObjectItemMover.allowedMovesCount = addedGameObjectItemMover.initialAllowedMovesCount = itemMover.allowedMovesCount;

        addedGameObject.AddComponent(typeof(ItemMousePlacer));
        placingGameObject = addedGameObject;
        placingGameObject.transform.SetParent(transform);
    }

    private void AddItemIcon(GameObject collectedObject)
    {
        var item = new GameObject();
        item.name = "queue-" + collectedObject.name;
        item.transform.position = position;
        var itemSprite = item.AddComponent<SpriteRenderer>();
        itemSprite.sortingLayerName = "Canvas";
        itemSprite.sortingOrder = 5;
        var itemMoverSpriteRenderer = collectedObject.GetComponent<SpriteRenderer>();
        itemSprite.sprite = itemMoverSpriteRenderer.sprite;
        itemSprite.flipX = itemMoverSpriteRenderer.flipX;
        item.transform.localScale = itemMoverSpriteRenderer.transform.localScale;
        itemIcons.Enqueue(item);
    }

    private void UpdateUIWidth()
    {
        if (itemIcons.Count == 0)
            return;

        totalWidth = itemIcons.Sum(x => x.GetComponent<SpriteRenderer>().bounds.size.x);
        Vector2 uiLocalScale = uiSpriteRenderer.gameObject.transform.localScale;
        uiLocalScale.x = totalWidth;
        uiLocalScale.y = itemIcons.Max(x => x.GetComponent<SpriteRenderer>().bounds.size.y);
        uiSpriteRenderer.gameObject.transform.localScale = uiLocalScale;
    }
}
