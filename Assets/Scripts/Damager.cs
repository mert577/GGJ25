using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{



    public LayerMask damageableLayer;

    [SerializeField]
    int baseDamage;

    public int realDamage;

    public bool isPlayer;



    public enum DamagerType{
        EnterOnly,
        DamagesWhileAlive
    }

    public DamagerType damagerType;

    public bool isKnockback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer){
            realDamage = Mathf.RoundToInt(baseDamage * ModifierManager.instance.TryGetModifierValue("bubbleDamage"));
        }
        else{
            realDamage = baseDamage;
        }
    }


    
}
