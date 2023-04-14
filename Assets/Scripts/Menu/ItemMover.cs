using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMover : MonoBehaviour
{
    public delegate void MovedItemEventHandler(GameObject movedGameObject);
    public event MovedItemEventHandler FinishedMovingItem;
    public static event MovedItemEventHandler FinishedMovingAnyItem;
    public event MovedItemEventHandler StartedMovingItem;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider2D;
    [SerializeField] private bool isDragging = false;

    public Material initialMaterial;
    public Material canMoveMaterial;
    public bool canMove = false;

    private GameObject gridWorld;
    private GameObject movedObjects;
    private LevelComponentSettings levelComponentSettings;
    private Vector3 initialMousePos;
    private Vector3 initialObjectPos;

    private static bool isDraggingAnyObject = false;

    public Vector2 SpawnPosition { get; private set; }
    public bool IsDragging { get; private set; }


    private void Start()
    {
        SpawnPosition = gameObject.transform.position;

        if (GameController.gameController != null)
            GameController.gameController.StartedLevel += GameController_StartedGame;

        levelComponentSettings = gameObject.GetComponent<LevelComponentSettings>();
        if (levelComponentSettings != null)
        {
            if (!GameHelper.IsUsingMapEditor())
                levelComponentSettings.UpdateSetting(nameof(canMove), true);

            if (levelComponentSettings.settings.ContainsKey(nameof(canMove)))
                canMove = (bool)levelComponentSettings.settings[nameof(canMove)];
            OnValidate();
        }

        if (!GameHelper.IsUsingMapEditor())
        {
            movedObjects = GameObject.Find("MovedObjects");
            gridWorld = GameObject.Find("GridWorld");
        }
    }

    public void UpdateMaterial()
    {
        if (canMoveMaterial != null)
            spriteRenderer.material = canMove ? canMoveMaterial : initialMaterial;
    }

    private void OnValidate()
    {
        if (levelComponentSettings != null)
            levelComponentSettings.UpdateSetting(nameof(canMove), canMove);

        UpdateMaterial();
    }

    private void OnDestroy()
    {
        if (GameController.gameController != null)
            GameController.gameController.StartedLevel -= GameController_StartedGame;
    }

    private void GameController_StartedGame()
    {
        IsDragging = isDraggingAnyObject = false;
    }

    private void OnMouseDown()
    {
        if (!GameHelper.IsUsingMapEditor() && !GameHelper.IsTesting() && !GameController.hasStartedGame)
            return;
        if ((canMove || (GameHelper.IsUsingMapEditor() && !GameHelper.IsTesting())) && !isDraggingAnyObject && !IsDragging)
        {
            IsDragging = isDraggingAnyObject = true;
            initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            initialObjectPos = gameObject.transform.position;

            StartedMovingItem?.Invoke(gameObject);
        }
    }


    private void Update()
    {
        isDragging = IsDragging;
        if (IsDragging)
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                0.5f);

            if (canMoveMaterial != null)
            {
                spriteRenderer.material.SetColor("_SolidOutline", new Color(
                    canMoveMaterial.color.r,
                    canMoveMaterial.color.g,
                    canMoveMaterial.color.b,
                    0.5f));
            }

            Vector2 offset = initialObjectPos - initialMousePos;

            // Snap the spawned object to the nearest grid cell and add the offset back in.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 snappedPosition = GridHelper.SnapToGrid(mousePosition + offset);
            gameObject.transform.position = snappedPosition;
        }

        if (Input.GetMouseButtonUp(0) && IsDragging)
        {
            // Stop dragging the spawned object.
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                1);
            if (canMoveMaterial != null)
            {
                spriteRenderer.material.SetColor("_SolidOutline", new Color(
                    canMoveMaterial.color.r,
                    canMoveMaterial.color.g,
                    canMoveMaterial.color.b,
                    1));
            }

            IsDragging = isDraggingAnyObject = false;

            if (movedObjects != null && gameObject.transform.parent == gridWorld.transform)
                gameObject.transform.SetParent(movedObjects.transform);

            FinishedMovingItem?.Invoke(gameObject);
            FinishedMovingAnyItem?.Invoke(gameObject);
        }
    }

    public void SetDragging(bool isDragging)
    {
        IsDragging = isDraggingAnyObject = isDragging;

        if (IsDragging)
            StartedMovingItem?.Invoke(gameObject);
    }
}
