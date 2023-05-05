using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GridHelper
    {
        public static Vector2 SnapToGrid(Vector2 position)
        {
            float gridWidth = 0.23f;// EditorSnapSettings.scale;
            float gridHeight = 0.23f;// EditorSnapSettings.scale;

            float snappedX = Mathf.Round(position.x / gridWidth) * gridWidth;
            float snappedY = Mathf.Round(position.y / gridHeight) * gridHeight;

            return new Vector2(snappedX, snappedY);
        }
    }
}
