using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(ItemMover))]
public class KeyBehaviour : MonoBehaviour
{
    public static event Action<Unlocker> UsedAnyKey;
    [SerializeField] private ItemMover itemMover;
    private BoxCollider2D boxCollider2D;
    private Dictionary<BoxCollider2D, Unlocker> collisions = new();

    private bool shouldDestroy = false;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
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
            UsedAnyKey?.Invoke(GetClosestCollisionUnlocker());
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        var unlockers = FindObjectsByType<Unlocker>(FindObjectsSortMode.None).Where(x => x.enabled);
        foreach (var unlocker in unlockers)
        {
            var unlockerCollider = unlocker.GetComponent<BoxCollider2D>();
            if (boxCollider2D.IsTouching(unlockerCollider))
            {
                if (!collisions.ContainsKey(unlockerCollider))
                    collisions.Add(unlockerCollider, unlocker);

                shouldDestroy = true;
            }
            else
            {
                if (collisions.ContainsKey(unlockerCollider))
                    collisions.Remove(unlockerCollider);

                shouldDestroy = false;
            }
        }
    }

    private Unlocker GetClosestCollisionUnlocker()
    {
        BoxCollider2D closestCollision = collisions.Keys.FirstOrDefault();
        Unlocker closestUnlocker = null;

        foreach (var collision in collisions)
        {
            if (closestCollision.Equals(collision))
                continue;

            if (Vector2.Distance(boxCollider2D.bounds.center, closestCollision.bounds.center) >
                Vector2.Distance(collision.Key.bounds.center, closestCollision.bounds.center))
            {
                closestCollision = collision.Key;
                closestUnlocker = collision.Value;
            }
        }

        return closestUnlocker;
    }
}
