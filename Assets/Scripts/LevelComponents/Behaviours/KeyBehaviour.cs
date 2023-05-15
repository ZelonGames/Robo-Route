using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemMover))]
public class KeyBehaviour : MonoBehaviour
{
    public static event Action<Unlocker> UsedAnyKey;
    [SerializeField] private ItemMover itemMover;

    private Unlocker unlocker;
    private bool shouldDestroy = false;

    void Start()
    {
        itemMover.FinishedMovingItem += ItemMover_FinishedMovingItem;
    }

    private void OnDestroy()
    {
        itemMover.FinishedMovingItem -= ItemMover_FinishedMovingItem;
    }

    private void ItemMover_FinishedMovingItem()
    {
        if (shouldDestroy)
        {
            UsedAnyKey?.Invoke(unlocker);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unlocker>(out var unlocker))
        {
            shouldDestroy = true;
            this.unlocker = unlocker;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Unlocker>(out var unlocker))
            shouldDestroy = false;
    }
}
