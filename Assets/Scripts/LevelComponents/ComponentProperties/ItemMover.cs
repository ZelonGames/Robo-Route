using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.GraphicsBuffer;

public class ItemMover : MonoBehaviour
{
    public delegate void MovedItemEventHandler(GameObject movedGameObject);
    public static event MovedItemEventHandler StartedMovingAnyItem;
    public static event MovedItemEventHandler FinishedMovingAnyItem;
    public static event Action ChangedPosition;
    public event Action FinishedMovingItem;
    public event Action StartedMovingItem;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider2D;
    [SerializeField] private TextMeshPro textAllowedMovesCount;
    [SerializeField] private Light2D highlight;

    public Material initialMaterial;
    public Material canMoveMaterial;

    public bool usingLimitedMoves = false;
    public int allowedMovesCount = 0;
    [HideInInspector] public int initialAllowedMovesCount = 0;

    public bool canMove = false;
    public bool initialCanMove = false;

    private CursorObjectQueue cursorObjectQueue;
    private GameObject gridWorld;
    private GameObject movedObjects;
    private LevelComponentSettings levelComponentSettings;
    private Vector2? previousPosition = null;
    private Vector2 initialMousePos;
    private Vector2 initialObjectPos;

    private static bool isDraggingAnyObject = false;

    public Vector2 SpawnPosition { get; private set; }
    public bool IsDragging { get; private set; }


    private void Start()
    {
        initialAllowedMovesCount = allowedMovesCount;
        SpawnPosition = gameObject.transform.position;
        cursorObjectQueue = FindObjectOfType<CursorObjectQueue>();

        if (highlight != null && canMove)
            highlight.enabled = true;

        if (GameController.gameController != null)
            GameController.gameController.StartedLevel += GameController_StartedGame;
    }

    public void UpdateMaterial()
    {
        if (canMoveMaterial != null)
            spriteRenderer.material = canMove ? canMoveMaterial : initialMaterial;
    }

    public void UpdateAllowedMovesCountText()
    {
        if (textAllowedMovesCount != null)
        {
            textAllowedMovesCount.text = usingLimitedMoves ? allowedMovesCount.ToString() : "";
            if (allowedMovesCount <= 0)
            {
                textAllowedMovesCount.text = "";
                if (highlight != null)
                    highlight.enabled = false;
            }
        }
    }

    private void OnValidate()
    {
        if (levelComponentSettings != null)
        {
            levelComponentSettings.UpdateSetting(nameof(canMove), canMove);
            levelComponentSettings.UpdateSetting(nameof(usingLimitedMoves), usingLimitedMoves);
            levelComponentSettings.UpdateSetting(nameof(allowedMovesCount), allowedMovesCount);
        }

        UpdateAllowedMovesCountText();

        initialAllowedMovesCount = allowedMovesCount;

        if (usingLimitedMoves && allowedMovesCount <= 0)
            spriteRenderer.material = initialMaterial;

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
        if (!GameController.hasStartedGame)
            return;

        if (canMove)
        {
            if (!isDraggingAnyObject && !IsDragging)
            {
                if (!usingLimitedMoves || usingLimitedMoves && allowedMovesCount > 0)
                {
                    if (highlight != null)
                        highlight.intensity *= 0.5f;

                    IsDragging = isDraggingAnyObject = true;
                    initialMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    initialObjectPos = gameObject.transform.position;
                    StartedMovingAnyItem?.Invoke(gameObject);

                    if (cursorObjectQueue != null)
                        cursorObjectQueue.AddGameObjectToQueue(gameObject, this, false);
                    else
                    {
                        StartedMovingItem?.Invoke();
                    }
                }
            }
        }
    }

    public void ResetAllowedMovesCount()
    {
        if (!usingLimitedMoves)
            return;

        allowedMovesCount = initialAllowedMovesCount;
    }

    private void DecreaseAllowedMovesCount()
    {
        if (!canMove || !usingLimitedMoves)
            return;

        if (allowedMovesCount > 0)
        {
            allowedMovesCount--;
            textAllowedMovesCount.text = allowedMovesCount.ToString();
        }

        if (allowedMovesCount <= 0)
        {
            canMove = false;
            spriteRenderer.material = initialMaterial;
            textAllowedMovesCount.text = "";
        }
    }

    private void Update()
    {
        if (IsDragging)
        {
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                0.5f);

            if (canMoveMaterial != null)
            {/*
                spriteRenderer.material.SetColor("_SolidOutline", new Color(
                    canMoveMaterial.color.r,
                    canMoveMaterial.color.g,
                    canMoveMaterial.color.b,
                    0.5f));*/
            }

            Vector2 offset = initialObjectPos - initialMousePos;

            // Snap the spawned object to the nearest grid cell and add the offset back in.
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 snappedPosition = GridHelper.SnapToGrid(mousePosition + offset);
            gameObject.transform.position = snappedPosition;

            if (previousPosition.HasValue && previousPosition != gameObject.transform.position)
                ChangedPosition?.Invoke();

            previousPosition = gameObject.transform.position;
        }

        if (Input.GetMouseButtonUp(0) && IsDragging)
        {
            // Stop dragging the spawned object.
            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                1);

            if (highlight != null)
                highlight.intensity /= 0.5f;

            if (canMoveMaterial != null)
            {/*
                spriteRenderer.material.SetColor("_SolidOutline", new Color(
                    canMoveMaterial.color.r,
                    canMoveMaterial.color.g,
                    canMoveMaterial.color.b,
                    1));*/
            }

            IsDragging = isDraggingAnyObject = false;

            if (movedObjects != null && gameObject.transform.parent == gridWorld.transform)
                gameObject.transform.SetParent(movedObjects.transform);

            DecreaseAllowedMovesCount();
            FinishedMovingItem?.Invoke();
            FinishedMovingAnyItem?.Invoke(gameObject);
        }
    }

    public void SetDragging(bool isDragging)
    {
        IsDragging = isDraggingAnyObject = isDragging;

        if (IsDragging)
            StartedMovingItem?.Invoke();
    }
}