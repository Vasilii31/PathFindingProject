using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState { SETUP, SPAWN, BATTLE, VICTORY, LOSS}
    public BattleState battleState;

    public float timeBetweenWaves;
    public float battleTimer = 0;

    private GameManager gm;
    private BattleFieldManager bfm;

    public Transform firstTileForChampion;
    public Transform firstTileForEnemy;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        bfm = GameObject.Find("BattleFieldManager").GetComponent<BattleFieldManager>();
        battleState = BattleState.SPAWN;
    }

    void Update()
    {
        switch(battleState)
        {
            case BattleState.SPAWN:
                //Spawn a wave
                SpawnEntities();
                battleState = BattleState.SETUP;
                break;
            case BattleState.SETUP:
                break;
            case BattleState.BATTLE:
                battleTimer += Time.deltaTime;
                if(battleTimer >= timeBetweenWaves)
                {
                    //Spawn a new Wave
                }
                break;
            case BattleState.VICTORY:
                break;
            case BattleState.LOSS:
                break;

        }
    }

    private void SpawnEntities()
    {
        foreach(GameObject champion in gm.champions)
        {
            Instantiate(champion, firstTileForChampion.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
            champion.GetComponent<IAFight>().currentNode = GameObject.Find("TileMap").GetComponent<HexTileMapGenerator>().graph[0, 0];
            champion.GetComponent<IAFight>().tilePositionZ = 0;
            champion.GetComponent<IAFight>().tilePositionX = 0;
            champion.GetComponent<IAFight>().isEnemy = false;
            bfm.championsOnBf.Add(champion);
        }
        foreach(GameObject enemy in gm.tempEnemies)
        {
            Instantiate(enemy, firstTileForEnemy.position + new Vector3(0,0.7f,0) , Quaternion.identity);
            enemy.GetComponent<IAFight>().currentNode = GameObject.Find("TileMap").GetComponent<HexTileMapGenerator>().graph[9, 9];
            enemy.GetComponent<IAFight>().tilePositionX = 9;
            enemy.GetComponent<IAFight>().tilePositionZ = 9;
            enemy.GetComponent<IAFight>().isEnemy = true;
            bfm.enemiesOnBf.Add(enemy);
        }
        battleState = BattleState.SETUP;
    }
}
