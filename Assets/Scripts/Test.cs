using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 worldPosition;
    RaycastHit hit;
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        DrawTheRay();

    }

    private void DrawTheRay()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }
        Debug.DrawRay(Camera.main.transform.position, worldPosition - Camera.main.transform.position, Color.red);
    }
}
