using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private new Camera camera;
    private Vector3 position;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        position = camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        gameObject.transform.position = position;
    }
}
