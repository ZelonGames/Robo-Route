using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    private ItemMover itemMover;
    void Start()
    {
        itemMover.FinishedMovingItem += ItemMover_FinishedMovingItem;
    }

    private void ItemMover_FinishedMovingItem()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
