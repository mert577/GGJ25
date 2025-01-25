using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterBubbleThrowUpgrade : Upgrade
{
    public float bubbleSpeedMultiplier = 1.5f;
    public float bubbleSizeMultiplier = 1.5f;
    public float bubbleDamageMultiplier = 1.5f;

    public override void ApplyUpgrade()
    {
        Debug.Log("Applying upgrade: " + upgradeName);
    }
}

