using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string aName = "New Ability";
    public int aId;
    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}
