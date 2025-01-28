using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemytRangeMovement : MonoBehaviour
{
    // Start is called before the first frame update


    public float speed = 5f;

    public float closeDistance = 4f;

    public float farDistance = 5f;

    public Transform playerTransform;

    public bool readyToAttack = false;

    [SerializeField]
    Rigidbody2D rb;
    void Start()
    {

        playerTransform = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction;
        bool shouldMoveAwayFromPlayer = Vector2.Distance(transform.position, playerTransform.position) < closeDistance;
        bool shouldMoveTowardsPlayer = Vector2.Distance(transform.position, playerTransform.position) > farDistance;


        readyToAttack = shouldMoveAwayFromPlayer;

        if (shouldMoveAwayFromPlayer)
        {
            direction = (transform.position - playerTransform.position).normalized;

        }
        else if (shouldMoveTowardsPlayer)
        {
            direction = (playerTransform.position - transform.position).normalized;

        }
        else
        {
            direction = Vector2.zero;
        }

        rb.velocity = direction * speed;
    }
}
