using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAFight : MonoBehaviour
{
    public BattleFieldManager bfManager;
    public BattleManager btManager;

    [Header("Character Stats")]
    public float damage = 5f;
    public float maxhealth = 100f;
    public float currentHealth = 100f;
    public float moveSpeed = 1.2f;
    public float attackRange = 1f;
    public float maxMana = 100f;
    public float currentMana = 0f;
    public float manaGainOnAttack = 20f;

    public Ability ultimate;

    //moving Behaviour variables
    public GameObject currentTarget;
    public float currentTargetDistance;

    public int tilePositionX;
    public int tilePositionZ;
    public HexTileMapGenerator htMap;
    public List<Node> currentPath = null;
    public List<Node> PathToTarget = null;
    public Node currentNode;

    //attack Behaviour variables
    private float attackTimer = 2f;
    public float attackSpeed = 2f;
    

    public bool isEnemy;
    Color color;
    public enum CombatState { MOVING, ATTACK, IDLE, DEATH }
    public CombatState state;

    void Start()
    {
        bfManager = GameObject.Find("BattleFieldManager").GetComponent<BattleFieldManager>();
        btManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        htMap = GameObject.Find("TileMap").GetComponent<HexTileMapGenerator>();
        state = CombatState.MOVING;
        currentNode = htMap.graph[tilePositionX, tilePositionZ];
        if (isEnemy)
        {
            color = Color.red;
        }
        else
        {
            color = Color.blue;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath != null)
        {

            int currNode = 0;
            while (currNode < currentPath.Count - 1)
            {
                Vector3 Start = htMap.TileCoordToWorldCoord(PathToTarget[currNode].x, PathToTarget[currNode].z) + new Vector3(0, 1f, 0);
                Vector3 End = htMap.TileCoordToWorldCoord(PathToTarget[currNode + 1].x, PathToTarget[currNode + 1].z) + new Vector3(0, 1f, 0);
                Debug.DrawLine(Start, End, color);
                currNode++;
            }

        }
        if (btManager.battleState == BattleManager.BattleState.BATTLE)
        {


            switch (state)
            {
                case CombatState.ATTACK:
                    {
                        if (attackTimer >= attackSpeed)
                        {
                            Debug.Log(gameObject.name + "attacks " + currentTarget.name + " for " + damage + "damage !");
                            currentTarget.GetComponent<IAFight>().TakeDamage(damage);
                            Debug.Log(currentTarget.name + " has " + currentHealth + " hp left !");
                            attackTimer = 0;
                           
                            ManaUpDown(manaGainOnAttack);
                            Debug.Log(gameObject.name + " has " + currentMana +  "Mana ");
                            if(currentMana >= maxMana)
                            {
                                if(ultimate != null)
                                {
                                    ultimate.Initialize(this.gameObject);
                                    ultimate.TriggerAbility();
                                    currentMana = 0;
                                    
                                }
                                Debug.Log("Ultimate should have been launched");
                            }
                        }
                        attackTimer += Time.deltaTime;
                        break;
                    }
                case CombatState.IDLE:
                    break;
                case CombatState.MOVING:
                    {
                        // tentative d'optimisation, a revoir
                        //if (FindClosestTarget() != currentTarget || currentTarget.GetComponent<IAFight>().currentNode != currentPath[currentPath.Count] || currentTarget == null)
                        //{
                        Debug.Log(gameObject.name + " " + currentTargetDistance + " attack range = " + attackRange);

                        currentTarget = FindClosestTarget();
                        if (currentPath == null)
                        {
                            GeneratePathToTarget();
                        }
                        if (currentTargetDistance <= attackRange)
                        {
                            state = CombatState.ATTACK;
                            break;
                        }
                        else
                        {
                            Move();
                        }
                        //}

                        //MoveToTarget();
                        break;
                    }
                case CombatState.DEATH:
                    {
                        Die();
                        break;
                    }
            }
        }
    }

    private void ManaUpDown(float manaAdd)
    {
        currentMana += manaAdd;
    }

    private void Die()
    {
        
        if(isEnemy)
        {
            //give xp value to all champions
            
        }
        else
        {
            //add to graveyard and hand to potential resurrect, to be determined

        }
        Destroy(gameObject);
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    private void Move()
    {

        if (PathToTarget == null)
            return;
        Vector3 movePoint = htMap.TileCoordToWorldCoord(PathToTarget[0].x, PathToTarget[0].z) + new Vector3(0, transform.position.y, 0);
        transform.position += (movePoint - transform.position).normalized * Time.deltaTime * moveSpeed;
        //Debug.Log("Move but no recalculating");

        //Si on change de Node, On recalcule le chemin vers la cible
        if (Mathf.CeilToInt(transform.position.x) == Mathf.CeilToInt(movePoint.x) && Mathf.CeilToInt(transform.position.z) == Mathf.CeilToInt(movePoint.z))
        {
            //Debug.Log(gameObject.name + "TILE CHANGE");
            //Debug.Log("recalculating");

            PathToTarget.RemoveAt(0);
            tilePositionX = PathToTarget[0].x;
            tilePositionZ = PathToTarget[0].z;
            //Debug.Log("test tiles" + tilePositionX + " + " + tilePositionZ);
            //Debug.Log("test target tiles" + currentTarget.GetComponent<IAFight>().tilePositionX + " + " + currentTarget.GetComponent<IAFight>().tilePositionZ);
            //Debug.Log(gameObject.name + "current Node : " + currentNode.x + " , " + currentNode.z + " And Path to target first Node : " + PathToTarget[0].x + " , " + PathToTarget[0].z);
            currentNode = htMap.graph[tilePositionX, tilePositionZ];

            //Debug.Log("test apres changement de Node" + gameObject.name + " NEW current Node : " + currentNode.x + " , " + currentNode.z);

            Recalculate();

        }
    }

    private void Recalculate()
    {
        currentTarget = FindClosestTarget();
        GeneratePathToTarget();
        Debug.Log("Calculated Path to " + currentTarget + " Located at " + currentTarget.GetComponent<IAFight>().currentNode.x + " , " + currentTarget.GetComponent<IAFight>().currentNode.z + "with tilex = " + currentTarget.GetComponent<IAFight>().tilePositionX + " and tilez = " + currentTarget.GetComponent<IAFight>().tilePositionZ);
    }

    public void MoveToNextTile()
    {
        if (currentPath == null)
            return;
        //Remove the old current/first node of the path
        currentPath.RemoveAt(0);

        //Now grab the new first node and move us to that position

        transform.position = htMap.TileCoordToWorldCoord(currentPath[0].x, currentPath[0].z) + new Vector3(0, transform.position.y, 0);
        tilePositionX = currentPath[0].x;
        tilePositionZ = currentPath[0].z;
        if (currentPath.Count == 1)
        {
            // We only have one tile left in the path, and that tile MUST be our ultimate destination, so let's clear our pathfinding info
            currentPath = null;
        }
    }

    /*private void MoveToTarget()
    {
        transform.LookAt(target.transform);
        if (currentTargetDistance > attackRange)
        {
            //transform.position += (target.transform.position - transform.position).normalized * Time.deltaTime * moveSpeed;
            GetComponent<NavMeshAgent>().destination = target.transform.position;
        }
        else
        {
            GetComponent<NavMeshAgent>().isStopped = true;

            state = CombatState.ATTACK;
        }
        
    }*/

    public GameObject FindClosestTarget()
    {
        GameObject[] potentialTargets;
        GameObject temptarget = null;

        if (isEnemy)
        {
            potentialTargets = GameObject.FindGameObjectsWithTag("Champion");
        }
        else
        {
            potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");
        }
        if (potentialTargets.Length == 0)
        {
            return null;
        }
        float distanceCalculated;
        if (currentTarget == null)
        {
            currentTargetDistance = Mathf.Infinity;
        }
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            distanceCalculated = CalculateDistance(potentialTargets[i]);
            if (distanceCalculated < currentTargetDistance)
            {
                temptarget = potentialTargets[i];
                currentTargetDistance = distanceCalculated;
            }
        }
        return temptarget;
        /*if(isEnemy)
        {
            foreach (GameObject champion in bfManager.championsOnBf)
            {
                distanceCalculated = CalculateDistance(champion);
                if (distanceCalculated < currentTargetDistance)
                {
                    temptarget = champion;
                    currentTargetDistance = distanceCalculated;
                }
            }
        }
        else
        {
            foreach (GameObject enemy in bfManager.enemiesOnBf)
            {
                distanceCalculated = CalculateDistance(enemy);
                if (distanceCalculated < currentTargetDistance)
                {
                    temptarget = enemy;
                    currentTargetDistance = distanceCalculated;
                }
            }
        }*/

    }

    private float CalculateDistance(GameObject e)
    {
        return (transform.position - e.transform.position).magnitude;
    }

    public Node ReturnTargetCurrentNode()
    {
        Node node;
        node = htMap.graph[tilePositionX, tilePositionZ];
        return node;
    }
    private void GeneratePathToTarget()
    {

        currentPath = null;


        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        //set up the Q : the list of node we haven't checked yet
        List<Node> unvisited = new List<Node>();

        //Node source = graph[(int)unit.GetComponent<IAFight>().tilePositionX, (int)unit.GetComponent<IAFight>().tilePositionZ];
        //Node source = htMap.graph[currentNode.x, currentNode.z];
        Node source = currentNode;
        Debug.Log("VERIF current - source ? " + gameObject.name + currentNode.x + currentNode.z + " s " + source.x + source.z);
        //Node source = graph[0, 0];
        //Node target = htMap.graph[currentTarget.GetComponent<IAFight>().currentNode.x, currentTarget.GetComponent<IAFight>().currentNode.z];
        Debug.Log("verif currenttarget - target " + gameObject.name + currentTarget.name + currentTarget.GetComponent<IAFight>().currentNode.x + currentTarget.GetComponent<IAFight>().currentNode.z);
        Node target = currentTarget.GetComponent<IAFight>().currentNode;
        //Node target = htMap.graph[currentTarget.GetComponent<IAFight>().tilePositionX, currentTarget.GetComponent<IAFight>().tilePositionZ];

        dist[source] = 0;
        prev[source] = null;


        //Initialize everything to have INFINITY distance, since we don't know any better right now
        foreach (Node v in htMap.graph)
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
            return;//switch target ?
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

        PathToTarget = currentPath;
    }
    public bool UnitCanEnterTile(int x, int z)
    {
        if (htMap.graph[x, z].isTaken)
        {
            return false;
        }
        return true;
    }

}
