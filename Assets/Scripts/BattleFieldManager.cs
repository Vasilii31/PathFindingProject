using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleFieldManager : MonoBehaviour
{
    public List<GameObject> championsOnBf = new List<GameObject>();
    public List<GameObject> enemiesOnBf = new List<GameObject>();
    public List<GameObject> placementPoints = new List<GameObject>();

    

    public BattleManager btManager;
    private void Start()
    {
        btManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();

    }

    
}
