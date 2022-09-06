using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = (Vector3)((Vector2)(cam.ScreenToWorldPoint(Input.mousePosition)));
    }
}
