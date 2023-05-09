using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHider : MonoBehaviour
{
    void Awake()
    {
#if DEBUG
        Cursor.visible = true;
#else
        Cursor.visible = false;
#endif

    }
}
