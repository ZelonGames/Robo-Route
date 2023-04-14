using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    [SerializeField] private float scrollSpeed = 1f;

    private float minY = 0f;
    private float maxY = 10f;

    private bool canScroll = true;

    private void Start()
    {


        ScrollViewState.MouseEntered += ScrollViewState_MouseEntered;
        ScrollViewState.MouseExit += ScrollViewState_MouseExit;
    }

    private void OnDestroy()
    {
        ScrollViewState.MouseEntered -= ScrollViewState_MouseEntered;
        ScrollViewState.MouseExit -= ScrollViewState_MouseExit;
    }

    private void ScrollViewState_MouseExit()
    {
        canScroll = true;
    }

    private void ScrollViewState_MouseEntered()
    {
        canScroll = false;
    }

    private void Update()
    {
        if (!canScroll)
            return;

        if (levelController != null && levelController.currentLevel != null)
        {
            minY = levelController.currentLevel.lowestPosition.Value;
            maxY = levelController.currentLevel.highestPosition.Value;
        }

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        float newY = transform.position.y + scrollAmount * scrollSpeed;

        if (levelController != null)
            newY = Mathf.Clamp(newY, minY, maxY);
        newY = GridHelper.SnapToGrid(new Vector2(0, newY)).y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
