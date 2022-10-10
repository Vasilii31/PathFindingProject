using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSetup : MonoBehaviour
{
    BattleManager btManager;
    BattleFieldManager bfManager;

    public GameObject CountDownUI;
    public Text CountDownText;

    float countDown = 3;
    bool cdActive = false;

    void Start()
    {
        btManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        bfManager = GameObject.Find("BattleFieldManager").GetComponent<BattleFieldManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cdActive)
        {

            countDown -= Time.deltaTime;
            CountDownText.text = Mathf.CeilToInt(countDown).ToString();
            if (countDown <= 0)
            {
                cdActive = false;
                CountDownUI.SetActive(false);
                countDown = 3;
                btManager.battleState = BattleManager.BattleState.BATTLE;
            }
        }
    }

    public void OnEndSetupClick()
    {
        if (btManager.battleState == BattleManager.BattleState.SETUP)
        {
            /*foreach (GameObject p in bfManager.placementPoints)
            {
                p.SetActive(false);
            }*/
            CountDownUI.SetActive(true);
            cdActive = true;
        }
    }
}
