using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public TileType[] tileTypes;

    int[,] tiles;

    int mapSizeX = 10;
    int mapSizeZ = 10;

    private void Start()
    {
        GenerateMapData();
        GenerateMapVisuals();
    }

    private void GenerateMapData()
    {
        tiles = new int[mapSizeX, mapSizeZ];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int z = 0; z < mapSizeZ; z++)
            {
                tiles[x, z] = 0;
            }
        }
    }

    private void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeZ; y++)
            {
                Instantiate(tileTypes[0].tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
}
