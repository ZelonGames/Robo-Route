using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables compontents while ItemMover is dragging something
/// </summary>
public class ComponentToggler : MonoBehaviour
{
    public List<Behaviour> monoBehaviours;
    public ItemMover itemMover;

    void Start()
    {
        if (itemMover != null)
        {
            itemMover.StartedMovingItem += ItemMover_StartedMovingItem;
            itemMover.FinishedMovingItem += ItemMover_MovedItem;
        }
    }

    private void OnDestroy()
    {
        if (itemMover != null)
        {
            itemMover.StartedMovingItem -= ItemMover_StartedMovingItem;
            itemMover.FinishedMovingItem -= ItemMover_MovedItem;
        }
    }

    private void ItemMover_MovedItem(GameObject movedGameObject)
    {
        EnableMonoBehaviours();
    }

    private void ItemMover_StartedMovingItem(GameObject movedGameObject)
    {
        DisableMonoBehaviours();
    }

    private void EnableMonoBehaviours()
    {
        monoBehaviours.ForEach(x => x.enabled = true);
    }

    private void DisableMonoBehaviours()
    {
        monoBehaviours.ForEach(x => x.enabled = false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
