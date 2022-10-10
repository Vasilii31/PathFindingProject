using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    bool isHighlighted= false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(!isHighlighted)
        {
            GetComponent<Outline>().OutlineWidth = 10f;
            isHighlighted = true;
        }
    }

    private void OnMouseExit()
    {
        if(isHighlighted)
        {
            GetComponent<Outline>().OutlineWidth = 0f;
            isHighlighted = false;
        }
    }
}
