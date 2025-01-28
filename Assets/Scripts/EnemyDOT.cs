using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDOT : MonoBehaviour
{

    [SerializeField]
    Transform background;


    public float damageInterval = 1f;

    public float damageRadius = 1f;


    public float damageTimer = 0f;


    public bool isActivelyDamaging = false;


    public float lifetime;







    void Start()
    {

        StartAnimation();
        StartCoroutine(Death());

    }



    IEnumerator Death()
    {
        yield return new WaitForSeconds(lifetime);
        DieOff();
    }
    void Update()
    {
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0 && isActivelyDamaging)
        {

            //do overlapping circle to see if player 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, LayerMask.GetMask("Player"));
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Health>() != null)
                {
                    collider.GetComponent<Health>().TakeDamage(1, transform.position, false);
                }
            }
            damageTimer = damageInterval;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    void StartAnimation()
    {

        IEnumerator _()
        {
            var particleSystem = GetComponent<ParticleSystem>().emission;
            background.localScale = Vector3.zero;

            background.DOScale(Vector3.one * 2, 03f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(.3f);
            particleSystem.enabled = true;
            isActivelyDamaging = true;
        }


        StartCoroutine(_());
    }



    void DieOff()
    {

        IEnumerator _()
        {
            var particleSystem = GetComponent<ParticleSystem>().emission;
            particleSystem.enabled = false;
            background.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(.3f);
            Destroy(gameObject);
        }

        StartCoroutine(_());
    }

}

