using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMovement : MonoBehaviour
{


    public Transform playerTransform;

    public Rigidbody2D rb;

    public float maxSpeed, accelerationForce;

    public bool stunned = false;

    public Health health;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }
    void Start()
    {

        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {

        if (!health.isStunned)
        {

            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.AddForce(direction * accelerationForce);
            //Print velocity magnitude and max speed

            if (rb.velocity.magnitude > maxSpeed)
            {

                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            }

          
        }
    }
}
