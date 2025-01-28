using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string upgradeDescription;
    public int upgradeCost;


    public Sprite upgradeSprite;


    public virtual void ApplyUpgrade()
    {
        Debug.Log("Applying upgrade: " + upgradeName);
    }
}
