using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotDamager : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmount;
    public float damageInterval;

    public float damageRadius;


    public float damageTimer;

    public bool isActivelyDamaging;
    void Start()
    {

    }



    public void StartAnimation(){

        IEnumerator StartAnimation()
        {
            transform.DOScale(Vector3.one* damageRadius*ModifierManager.instance.TryGetModifierValue("dotArea"), 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.5f);
            isActivelyDamaging = true;
        }

        StartCoroutine(StartAnimation());
    }


    public void StopAnimation(){
        IEnumerator StopAnimation()
        {
            isActivelyDamaging = false;
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(StopAnimation());
    }

    // Update is called once per frame
    void Update()
    {

        if (isActivelyDamaging)
        {
            DamageOverTime();
        }

    }


    void DamageOverTime()
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
            transform.DORewind();
            transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    collider.GetComponent<Health>().TakeDamage(Mathf.RoundToInt(damageAmount* ModifierManager.instance.TryGetModifierValue("dotDamage")), transform.position, false);
                }
            }
            damageTimer = damageInterval * ModifierManager.instance.TryGetModifierValue("dotInterval");
        }
    }
}
