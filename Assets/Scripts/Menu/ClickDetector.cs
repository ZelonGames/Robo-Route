using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour, IPointerDownHandler
{
    public delegate void ItemClickHandler(GameObject clickedGameObject);
    public event ItemClickHandler ItemClicked;
    
    void Start()
    {
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            ItemClicked?.Invoke(gameObject);
    }
}
