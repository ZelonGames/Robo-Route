using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpiderwebBehaviour : MonoBehaviour
{
    private void Start()
    {
        if (GameHelper.IsUsingMapEditor())
        {
            Destroy(gameObject.GetComponent<SidewaysMover>());
        }
    }
}




