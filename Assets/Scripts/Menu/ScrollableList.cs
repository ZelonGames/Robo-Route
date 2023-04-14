using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollableList : MonoBehaviour, IDragHandler, IPointerExitHandler, IPointerEnterHandler
{
    public float scrollSpeed = 1f;
    private Vector3 startPosition;

    private bool canMove = false;

    private void Start()
    {
        
        startPosition = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canMove) return;

        float deltaY = eventData.delta.y;
        Vector3 newPosition = transform.localPosition + new Vector3(0f, deltaY * scrollSpeed, 0f);

        foreach (Transform child in transform)
        {
            Vector3 childPosition = child.localPosition;
            
            childPosition.y -= newPosition.y - startPosition.y;
            child.localPosition = childPosition;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        canMove = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canMove = false;
    }
}


