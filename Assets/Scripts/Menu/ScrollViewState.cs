using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action MouseEntered;
    public static event Action MouseExit;

    void Start()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEntered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit?.Invoke();
    }
}
