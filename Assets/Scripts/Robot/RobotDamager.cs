using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDamager : MonoBehaviour
{
    public delegate void DestroyedRobotEventHandler(GameObject gameObject);
    public static event DestroyedRobotEventHandler DestroyedRobot;

    private LevelController levelController;

    [SerializeField] private Rigidbody2D rigidbody2D;
    public float maxVelocity = 9;
    public float hp = 5;
    public float damage = 5;

    private ItemMover itemMover;
    private bool killed = false;

    public void Start()
    {
        if (GameObject.Find("LevelController") != null)
            levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        itemMover = gameObject.GetComponent<ItemMover>();
    }

    public void FixedUpdate()
    {
        if (levelController == null)
            return;

        //if (transform.position.y < levelController.currentLevel.lowestPosition - 5)
          //  hp = 0;

        if (hp <= 0 && !killed)
        {
            killed = true;
            try
            {
                DestroyedRobot?.Invoke(gameObject);
            }
            catch { }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<NoDamage>(out var noDamage))
            return;

        if ((itemMover != null && !itemMover.IsDragging || itemMover == null) && Mathf.Abs(rigidbody2D.velocity.y) > maxVelocity)
            hp -= damage;
    }
}
