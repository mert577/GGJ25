using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyPulseFollower : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform target;

    public float tweenTime = 1f;
    public float tweenAmount = 1f;

    public Rigidbody2D rb;
    void Start()
    {
        
        target = GameObject.Find("Player").transform;
        StartCoroutine(MoveLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator MoveLoop()
    {
       
       if(target != null)
        {
            Vector2 targetPos = target.position;
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

            //rotate direction a random amount
            float randomAngle = Random.Range(-45, 45);
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;


            Vector2 newPos = (Vector2)transform.position + direction * tweenAmount;
            rb.DOMove(newPos, tweenTime).SetEase(Ease.InBack);
            yield return new WaitForSeconds(tweenTime);
            StartCoroutine(MoveLoop());
        }
    }
}
