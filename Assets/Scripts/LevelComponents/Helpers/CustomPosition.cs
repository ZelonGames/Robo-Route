using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CustomPosition
{
    public float x;
    public float y;

    public CustomPosition()
    {

    }

    public CustomPosition(Vector2 position)
    {
        x = position.x;
        y = position.y;
    }

    public CustomPosition(Vector3 position)
    {
        x = position.x;
        y = position.y;
    }
}
