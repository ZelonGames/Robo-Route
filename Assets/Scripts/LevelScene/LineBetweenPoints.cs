using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LineBetweenPoints : MonoBehaviour
{
    public GameObject fromObject;
    public GameObject toObject;

    public void UpdateLine()
    {
        if (fromObject == null || toObject == null)
        {
            gameObject.transform.localScale = new Vector3(0, 0, 1);
            return;
        }

        float distance = Vector2.Distance(fromObject.transform.localPosition, toObject.transform.localPosition);
        gameObject.transform.localScale = new Vector3(0.5f, distance, 1);

        Vector2 direction = toObject.transform.localPosition - fromObject.transform.localPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.transform.localPosition = (fromObject.transform.localPosition + toObject.transform.localPosition) / 2f;
    }

    private void Update()
    {
        UpdateLine();
    }

    public void OnValidate()
    {
        UpdateLine();
    }
}

