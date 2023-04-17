using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using System;

public class SwipeMove : MonoBehaviour
{
    public event Action StartedMovingObject;
    public event Action StoppedMovingObject;

    [SerializeField] private float swipeThreshold = 1f;

    private Vector3 lastMousePosition;

    private bool shouldInvokeStartedMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shouldInvokeStartedMoving = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            if (delta.magnitude > swipeThreshold)
            {
                if (shouldInvokeStartedMoving)
                {
                    shouldInvokeStartedMoving = false;
                    StartedMovingObject?.Invoke();
                }

                // Calculate the distance to move based on the delta position of the mouse
                float distanceToMove = delta.magnitude / Screen.dpi;

                // Convert the delta vector to world space and move the camera
                Vector3 direction = -delta.normalized;
                gameObject.transform.Translate(direction * distanceToMove, Space.World);
            }
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StoppedMovingObject?.Invoke();
        }
    }
}
