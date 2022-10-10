using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public BattleFieldManager bfManager;
    public GameObject target;
    public float currentTargetDistance;
    public float moveSpeed = 1.2f;
    private float attackRange = 1f;
    private enum CombatState { MOVING, ATTACK, IDLE }
    private CombatState state;

    private HexTileMapGenerator htMap;

    private List<Node> currentPath;

    public BattleManager btManager;
    void Start()
    {
        bfManager = GameObject.Find("BattleFieldManager").GetComponent<BattleFieldManager>();
        btManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        htMap = GameObject.Find("TileMap").GetComponent<HexTileMapGenerator>();
        state = CombatState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath != null)
        {
            int currNode = 0;
            while (currNode < currentPath.Count - 1)
            {
                Vector3 Start = htMap.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].z) + new Vector3(0, 1f, 0);
                Vector3 End = htMap.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].z) + new Vector3(0, 1f, 0);
                Debug.DrawLine(Start, End, Color.blue);
                currNode++;
            }

        }
        if (btManager.battleState == BattleManager.BattleState.BATTLE)
        {
            state = CombatState.MOVING;
        }
        switch (state)
        {
            case CombatState.ATTACK:
                Debug.Log("Attacking" + target.name);
                break;
            case CombatState.IDLE:
                break;
            case CombatState.MOVING:
                FindClosestTarget();
                MoveToTarget();
                break;
        }
    }

    private void MoveToTarget()
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

    }

    public void FindClosestTarget()
    {
        Debug.Log(currentTargetDistance);
        float distanceCalculated;
        if (target == null)
        {
            target = bfManager.championsOnBf[0];
            currentTargetDistance = CalculateDistance(target);
        }
        foreach (GameObject enemy in bfManager.championsOnBf)
        {
            distanceCalculated = CalculateDistance(enemy);
            if (distanceCalculated < currentTargetDistance)
            {
                target = enemy;
                currentTargetDistance = distanceCalculated;
            }
        }
        Debug.Log(target.name);
    }

    private float CalculateDistance(GameObject e)
    {
        return (transform.position - e.transform.position).magnitude;
    }
}
