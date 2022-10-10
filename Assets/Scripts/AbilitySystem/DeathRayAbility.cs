using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/DeathRayAbility")]
public class DeathRayAbility : Ability
{
    public GameObject aTarget;

    public override void Initialize(GameObject obj)
    {
        aTarget = obj.GetComponent<IAFight>().currentTarget;
    }

    public override void TriggerAbility()
    {
        Debug.Log("DEATH RAY LAUNCHED ON " + aTarget.name);
    }
}
