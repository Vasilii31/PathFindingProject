using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour
{
    bool isDragging = false;
    [SerializeField] float offsetDrag = 1.2f;
    Vector3 worldPosition;
    Vector3 originalPos;

    public int tileX;
    public int tileZ;

    
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Drag();
        }
    }

    private void Drag()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance))
        {
            Vector3 newPos = new Vector3(ray.GetPoint(distance).x, ray.GetPoint(distance).y + offsetDrag, ray.GetPoint(distance).z);
            transform.position = newPos;// ray.GetPoint(distance);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("bravo");
    }

    private void OnMouseDrag()
    {
        if(!isDragging)
        {
            originalPos = transform.position;
            isDragging = true;
        }        
    }


    //get the world position of the mouse
    private Vector3 GetMousePositionInWorld()
    {
        Vector3 result = Vector3.zero;
        Plane plane = new Plane(Vector3.up, 0);
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out distance))
        {
            result = ray.GetPoint(distance);
        }
        return result;
    }

    private void OnMouseUp()
    {
        if(isDragging)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(cam.transform.position, GetMousePositionInWorld() - cam.transform.position, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "PlacementCube")
                {
                    transform.position = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y + 0.5f, hit.collider.transform.position.z);
                    Debug.Log("relaché sur " + hit.collider.name);
                }
                else
                {
                    transform.position = originalPos;
                    Debug.Log("relaché sur rien");
                }
                isDragging = false;
            }

        }
    }

}
