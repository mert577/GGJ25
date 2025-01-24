using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{



    public LayerMask damageableLayer;

    public int damageAmount;


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
        
    }
}
