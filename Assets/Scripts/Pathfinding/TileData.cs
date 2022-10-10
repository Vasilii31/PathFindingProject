using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour 
{
    public int tileX;
    public int tileZ;

    public HexTileMapGenerator map;

    public bool isTaken;

    private void OnMouseUp()
    {
        Debug.Log("click");
        //map.GeneratePathTo(tileX, tileZ);
    }
}
