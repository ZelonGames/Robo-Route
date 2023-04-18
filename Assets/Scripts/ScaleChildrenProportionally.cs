using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChildrenProportionally : MonoBehaviour
{
    // This is the initial size of the parent object, used to calculate the scale factor.
    [SerializeField] private Transform parentTransform;
    private Vector3 initialSize;

    private void Start()
    {
        initialSize = parentTransform.localScale;
    }

    private void Update()
    {
        var scaleFactor = new Vector3(parentTransform.localScale.x / initialSize.x,
                                           parentTransform.localScale.y / initialSize.y,
                                           parentTransform.localScale.z / initialSize.z);

        foreach (Transform child in parentTransform)
        {
            var initialChildScale = new Vector3(child.localScale.x / initialSize.x,
                                                    child.localScale.y / initialSize.y,
                                                    child.localScale.z / initialSize.z);

            child.localScale = new Vector3(initialChildScale.x * scaleFactor.x,
                                            initialChildScale.y * scaleFactor.y,
                                            initialChildScale.z * scaleFactor.z);
        }
    }
}

