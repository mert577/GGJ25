using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{

    public float projectileSpeed;

    public GameObject projectilePrefab;


    public float minAttackInterval;
    public float maxAttackInterval;


    public float cooldownTimer = 0;


    public float projectileLifetime;

    public EnemytRangeMovement enemytRangeMovement;
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (GameObject.Find("Player").transform.position - transform.position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

        Destroy(projectile, projectileLifetime);   
    }
}
