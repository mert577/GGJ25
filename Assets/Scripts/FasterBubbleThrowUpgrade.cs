using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterBubbleThrowUpgrade : Upgrade
{
    public string modifierKey;
    public float modifierValue;



    public override void ApplyUpgrade()
    {
        Debug.Log("Applying upgrade: " + upgradeName);
        
        ModifierManager.instance.UpgradeModifierLevel(modifierKey);


    }
}

