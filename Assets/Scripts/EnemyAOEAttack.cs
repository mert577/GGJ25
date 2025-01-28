using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAOEAttack : MonoBehaviour
{
    public float projectileTime;

    public GameObject projectilePrefab;


    public float minAttackInterval;
    public float maxAttackInterval;


    public float cooldownTimer = 0;


    public float projectileLifetime;


    public GameObject projectile;
    public GameObject projectileGraphics;

    public float projectileJumpHeight;

    public GameObject dotAOE;

    public EnemytRangeMovement enemytRangeMovement;


    public GameObject currentProjectile;
    // Start is called before the first frame update
    void Start()
    {

        enemytRangeMovement = GetComponent<EnemytRangeMovement>();   
        cooldownTimer = Random.Range(minAttackInterval, maxAttackInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
        cooldownTimer -= Time.deltaTime;

        if(cooldownTimer <= 0 && enemytRangeMovement.readyToAttack)
        {
            Attack();
            cooldownTimer = Random.Range(minAttackInterval, maxAttackInterval);
        }
    }


    void Attack()
    {
        IEnumerator ThrowProjectileAsMortar()
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            currentProjectile = projectile;
            
            //get player velocity
            Vector2 playerVelocity = GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity;

            //get player position
            Vector2 playerPosition = GameObject.Find("Player").transform.position;


            //target player position
            Vector2 targetPosition = playerPosition + playerVelocity;
            

            projectile.transform.DOMove(targetPosition, projectileTime);
            projectile.transform.GetChild(0).DOLocalMoveY(projectileJumpHeight, projectileTime / 2).SetEase(Ease.OutCirc).OnComplete(() => {
                projectile.transform.GetChild(0).DOLocalMoveY(0, projectileTime / 2).SetEase(Ease.InCirc);
            });
            

            yield return new WaitForSeconds(projectileTime);

            //spawn dot aoe
            GameObject dot = Instantiate(dotAOE, targetPosition, Quaternion.identity);


             Destroy(projectile);   

        }

        StartCoroutine(ThrowProjectileAsMortar());
    }



    private void OnDestroy() {
        if(currentProjectile != null)
        {
            Destroy(currentProjectile);
        }
    }
}
