using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public int upgradeCost;


    public virtual void ApplyUpgrade()
    {
        Debug.Log("Applying upgrade: " + upgradeName);
    }
}
