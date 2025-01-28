using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HealUpgradde", menuName = "HealUpgradde")]
public class HealUpgradde : Upgrade
{
    public int healAmount;
    public override void ApplyUpgrade()
    {

        //find the player
        //get the player health
        //add the heal amount to the player health

        Debug.Log("Applying upgrade: " + upgradeName);
        GameObject player = GameObject.Find("Player");
        Health playerHealth = player.GetComponent<Health>();
        playerHealth.Heal(healAmount);

    }
}
