using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemMiniature : MonoBehaviour
{
    [SerializeField] private ItemMover itemMover;
    [SerializeField] private GameObject largeVersion;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer.sprite = largeVersion.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Robot"))
            return;

        GameObject.Find("CursorObjectQueue").GetComponent<CursorObjectQueue>().AddGameObjectToQueue(largeVersion, itemMover);
        Destroy(gameObject);
    }
}
