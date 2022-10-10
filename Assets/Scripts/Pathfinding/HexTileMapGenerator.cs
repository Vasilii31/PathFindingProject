using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HexTileMapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;


    //public GameObject unit;

    //List<Node> currentPath = null;

    //TileType[,] tiles;

    int mapWidth = 10;
    int mapHeight = 10;

    float tileXoffset = 0.5f;
    float tileZoffset = 0.85f;

     public Node[,] graph;

    void Start()
    {
        //CreateHexTileMap();
        // setup the unit's variable
        //unit.GetComponent<IAFight>().tilePositionX = (int)unit.transform.position.x;
        //unit.GetComponent<IAFight>().tilePositionZ = (int)unit.transform.position.z;

        GeneratePahthfindingGraph();
        Debug.Log("generation du graph");
        Debug.Log(graph[0, 0]);

    }
    public Vector3 TileCoordToWorldCoord(float x, float z)
    {
        if(z % 2 == 0)
        {
            return new Vector3(x, 0, z * tileZoffset);
        }
        else
        {
            return new Vector3(x + tileXoffset, 0, z * tileZoffset);
        }
        
        
    }
    void GeneratePahthfindingGraph()
    {
        //Initialize the array
        graph = new Node[mapWidth, mapHeight];

        //initialize a node for each spot in the array
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                graph[x, z] = new Node();
                graph[x, z].x = x;
                graph[x, z].z = z;
                graph[x, z].isTaken = false;
                Debug.Log(graph[x, z].isTaken);
            }
        }

        //float CostToEnterTile(int x, int y)
        //{
        //    TileType tt = ti
        //}

        //now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                // 4 way connection version
                //if (x > 0)
                //    graph[x, z].neighbours.Add(graph[x - 1, z]);
                //if (x < mapWidth - 1)
                //    graph[x, z].neighbours.Add(graph[x + 1, z]);
                //if (z > 0)
                //    graph[x, z].neighbours.Add(graph[x, z - 1]);
                //if (z < mapHeight - 1)
                //    graph[x, z].neighbours.Add(graph[x, z + 1]);

                //8 way connection version
                //Left
                //if (x > 0)
                //{
                //    graph[x, z].neighbours.Add(graph[x - 1, z]);
                //    if (z > 0)
                //        graph[x, z].neighbours.Add(graph[x - 1, z - 1]);
                //    if (z < mapHeight - 1)
                //        graph[x, z].neighbours.Add(graph[x - 1, z + 1]);
                //}
                ////right    
                //if (x < mapWidth - 1)
                //{
                //    graph[x, z].neighbours.Add(graph[x + 1, z]);
                //    if (z > 0)
                //        graph[x, z].neighbours.Add(graph[x + 1, z - 1]);
                //    if (z < mapHeight - 1)
                //        graph[x, z].neighbours.Add(graph[x + 1, z + 1]);
                //}
                ////straight up and down    
                //if (z > 0)
                //    graph[x, z].neighbours.Add(graph[x, z - 1]);
                //if (z < mapHeight - 1)
                //    graph[x, z].neighbours.Add(graph[x, z + 1]);

                // 6 way hexes FINAL VERSION
                if (x > 0) // a droite
                    graph[x, z].neighbours.Add(graph[x - 1, z]);
                if (x < mapWidth - 1)
                    graph[x, z].neighbours.Add(graph[x + 1, z]);
                if (z % 2 == 0) // lignes paires
                {
                    if (z > 0) // tant qu'on est pas à la premiere ligne donc verif pour cases de dessous
                    {
                        //dans tout les cas on rajoute la case en bas a droite
                        graph[x, z].neighbours.Add(graph[x, z - 1]);
                        if (x > 0) //mais si on est pas a la case 0 on peut rajouter la case en bas a gauche
                            graph[x, z].neighbours.Add(graph[x - 1, z - 1]);
                    }
                    if (z < mapHeight - 1) // tant qu'on est pas a la derniere ligne donc verif pour les cases de dessus
                    {
                        graph[x, z].neighbours.Add(graph[x, z + 1]);//dans tout les cas on ajoute la case en haut a droite
                        if (x > 0)
                            graph[x, z].neighbours.Add(graph[x - 1, z + 1]);
                    }
                }
                else//lignes impaires
                {
                    if (z > 0) // tant qu'on est pas à la premiere ligne donc verif pour cases de dessous
                    {
                        //dans tout les cas on rajoute la case en bas a gauche
                        graph[x, z].neighbours.Add(graph[x, z - 1]);
                        if (x < mapWidth - 1) //mais si on est pas a la derniere case on peut rajouter la case en bas a droite
                            graph[x, z].neighbours.Add(graph[x + 1, z - 1]);
                    }
                    if (z < mapHeight - 1) // tant qu'on est pas a la derniere ligne donc verif pour les cases de dessus
                    {
                        graph[x, z].neighbours.Add(graph[x, z + 1]);//dans tout les cas on ajoute la case en haut a gauche
                        if (x < mapWidth - 1)
                            graph[x, z].neighbours.Add(graph[x + 1, z + 1]);
                    }
                }
            }
        }
    }
    void CreateHexTileMap()
    {
        for (int x = 0; x <= mapWidth; x++)
        {
            for (int z = 0; z <= mapHeight; z++)
            {
                GameObject tempGO = Instantiate(tilePrefab);
                if (z % 2 == 0)
                {
                    tempGO.transform.position = new Vector3(x * tileXoffset, 0, z * tileZoffset);
                }
                else
                {
                    tempGO.transform.position = new Vector3(x * tileXoffset + tileXoffset / 2, 0, z * tileZoffset);
                }
                SetTileInfo(tempGO, x, z);
            }
        }
    }

    void SetTileInfo(GameObject GO, int x, int z)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + ", " + z.ToString();
        TileData td = GO.GetComponent<TileData>();
        td.map = this;
        td.tileX = x;
        td.tileZ = z;
        td.isTaken = false;
        //tiles[x, z] = td;
    }

    public bool UnitCanEnterTile(int x, int z)
    {
        if (graph[x, z].isTaken)
        {
            return false;
        }
        return true;
    }
    /*public List<Node> GeneratePathTo(int x, int z)
    {
        unit.GetComponent<IAFight>().currentPath = null;


        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //set up the Q : the list of node we haven't checked yet
        List<Node> unvisited = new List<Node>();

        //Node source = graph[(int)unit.GetComponent<IAFight>().tilePositionX, (int)unit.GetComponent<IAFight>().tilePositionZ];
        //Node source = 
        //Node source = graph[0, 0];
        Node target = graph[x, z];

        dist[source] = 0;
        prev[source] = null;

        //Initialize everything to have INFINITY distance, since we don't know any better right now
        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }
        while (unvisited.Count > 0)
        {
            //u is going to be the unvisited node with the smallest distance
            Node u = null;
            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    //U est ajouté uniquement si c'est une case libre
                    if (!possibleU.isTaken)
                    {
                        u = possibleU;
                    }

                }
            }

            if (u == target)
                break;

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + u.DistanceTo(v);
                // if we had different types of tiles
                //float alt = dist[u] + u.DistanceTo(v) + CostToEnterTile(v.x, v.z); 
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        // if we get here, either we found the shortest route to our target or there is no route at All to our target.
        if (prev[target] == null)
        {
            //no route to our target !
            return null;//switch target ?
        }

        currentPath = new List<Node>();
        Node curr = target;

        //step through the prev chain and add it to our path
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        // right now currentPath describe a route from our target to our source
        // So we need to invert it;
        currentPath.Reverse();
        return currentPath;
        //unit.GetComponent<IAFight>().currentPath = currentPath;
    }*/

}
