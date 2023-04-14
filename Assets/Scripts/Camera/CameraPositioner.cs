using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public Transform target;
    public float yOffset = 0;

    void Start()
    {
        return;
        target = GameObject.FindGameObjectsWithTag("Spawner").FirstOrDefault().transform;

        // Get the top edge of the target gameobject in world space
        float targetY = target.position.y + target.GetComponent<SpriteRenderer>().bounds.extents.y;

        // Get the height of the camera's view frustum
        float camHeight = Camera.main.orthographicSize;

        // Calculate the position of the camera so its top edge is slightly above the target gameobject's top edge
        float y = targetY - camHeight + yOffset;
        y = GridHelper.SnapToGrid(new Vector2(0, y)).y;

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
