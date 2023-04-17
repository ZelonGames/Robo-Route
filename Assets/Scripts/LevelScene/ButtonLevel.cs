using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevel : MonoBehaviour
{
    public static event Action<ButtonLevel> MouseEnter;
    public static event Action<ButtonLevel> StoppedShaking;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private TextMeshPro text;
    [SerializeField] public List<ButtonLevel> unlockingLevels = new();
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Floater verticalFloater;
    [SerializeField] private Floater horizontalFloater;
    [SerializeField] private Shaker shaker;
    [SerializeField] private SpriteRenderer padLock;
    [SerializeField] private SpriteRenderer star;
    [SerializeField] private bool unlocked = false;
    [SerializeField] private bool completed = false;
    [SerializeField] private bool addLines = false;
    private SwipeMove cameraSwipeMove;

    private Color unlockedColor = Color.white;
    private Color lockedColor = Color.gray;
    private Color completedColor = Color.green;

    private byte colorChange = 100;
    private bool previousAddLines = false;
    private bool canClick = true;

    private void Start()
    {
        GameObject.Find("Main Camera").TryGetComponent(out cameraSwipeMove);
        if (cameraSwipeMove != null)
        {
            cameraSwipeMove.StartedMovingObject += CameraSwipeMove_StartedMovingObject;
            cameraSwipeMove.StoppedMovingObject += CameraSwipeMove_StoppedMovingObject;
        }

        text.text = gameObject.name;

        UpdateAppearance();

        foreach (var unlockingLevel in unlockingLevels)
        {
            if (unlockingLevel == null)
                continue;

            GameObject line = Instantiate(linePrefab);
            line.transform.SetParent(GameObject.Find("Lines").transform);
            var lineBetweenPoints = line.GetComponent<LineBetweenPoints>();
            lineBetweenPoints.fromObject = gameObject;
            lineBetweenPoints.toObject = unlockingLevel.gameObject;
        }

        shaker.StoppedShaking += Shaker_StoppedShaking;
    }

    private void Shaker_StoppedShaking()
    {
        StoppedShaking?.Invoke(this);
    }

    private void OnDestroy()
    {
        if (cameraSwipeMove != null)
        {
            cameraSwipeMove.StartedMovingObject -= CameraSwipeMove_StartedMovingObject;
            cameraSwipeMove.StoppedMovingObject -= CameraSwipeMove_StoppedMovingObject;
        }
    }

    private void OnValidate()
    {
        text.text = gameObject.name;
    }

    private void CameraSwipeMove_StoppedMovingObject()
    {
        canClick = true;
    }

    private void CameraSwipeMove_StartedMovingObject()
    {
        canClick = false;
    }


    private void OnMouseUp()
    {
        if (!unlocked || !canClick)
            return;

        SceneManager.LoadScene(gameObject.name);

        if (!File.Exists(LevelLoader.LastPlayedLevelFile))
            File.Create(LevelLoader.LastPlayedLevelFile).Dispose();
        using StreamWriter streamWriter = new(LevelLoader.LastPlayedLevelFile, false);
        streamWriter.WriteLine(name);
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        text.color = Color.white;

        shaker.enabled = true;
        verticalFloater.enabled = false;
        horizontalFloater.enabled = false;

        MouseEnter?.Invoke(this);
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
        text.color = Color.black;

        verticalFloater.enabled = true;
        horizontalFloater.enabled = true;
        shaker.enabled = false;
    }

    public void Complete()
    {
        completed = true;
        UpdateAppearance();
    }

    public void Unlock()
    {
        unlocked = true;
        UpdateAppearance();
    }

    public void UpdateAppearance()
    {
        if (completed)
        {
            padLock.enabled = false;
            star.enabled = true;
        }
        else
        {
            star.enabled = false;
            padLock.enabled = !unlocked;
        }
    }
}
