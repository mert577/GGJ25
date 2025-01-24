
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PlayerControls : MonoBehaviour
{


    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Vector2 movementInput;

    [SerializeField]
    float maxSpeed, accelerationForce;


    [SerializeField]
    Vector2 directionToMouse;

    [SerializeField]
    GameObject bubble;

    [SerializeField]
    float bubbleThrowForce, bubbleWaitTime, bubbleReturnTime, screenShakeForce;

    [SerializeField]
    GameObject playerCoreSprite;

    [SerializeField]
    bool isBubbled = true;

    [SerializeField]
    bool isJumping = false;
    [SerializeField]
    float jumpHeight, jumpSpeed, minJumpTime, maxJumpTime;
    [SerializeField]
    AnimationCurve jumpCurve;

    Coroutine bubbleCoroutine;

    [SerializeField]
    CinemachineImpulseSource impulseSource;
    [SerializeField]
    GameObject stompParticlePrefab;

    [SerializeField]
    float stompRadius;
    [SerializeField]
    int stompDamage;

    [SerializeField]
    Health health;


    [SerializeField]
    Collider2D playerCollider;
    // Start is called before the first frame update


    void SpawnParticle(GameObject particlePrefab, Vector3 position)
    {
        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
        Destroy(particle, 1);
    }

    void Start()
    {
        bubble.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {

        CalculateDirectionToMouse();
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isBubbled)
            {
                ThrowBubble();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!isBubbled)
            {
                RecallBubble();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isBubbled)
            {
                JumpToBubble();
            }
        }


    }



    void Movement()
    {
        //add force until it saturates at max speed
        if (movementInput.magnitude > 0)
        {
            rb.AddForce(movementInput * accelerationForce);
        }


        //clamp the velocity to max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }



    void CalculateDirectionToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        directionToMouse = (mousePosition - transform.position).normalized;
        directionToMouse.Normalize();
    }



    void JumpToBubble()
    {


        //tween position to bubble position
        //also dotweenlooaljumpy the player core sprite

        IEnumerator JumpToBubbleCoroutine()
        {
            playerCollider.enabled = false;
            float jumpTime = Vector3.Distance(transform.position, bubble.transform.position) / jumpSpeed;
            jumpTime = Mathf.Clamp(jumpTime, minJumpTime, maxJumpTime);
            isJumping = true;
            rb.DOMove(bubble.transform.position, jumpTime);
            
            playerCoreSprite.transform.DOLocalMoveY(jumpHeight, jumpTime).SetEase(jumpCurve);
            playerCoreSprite.transform.DORotate(new Vector3(0, 0, 360), jumpTime, RotateMode.FastBeyond360);
            yield return new WaitForSeconds(jumpTime);
            health.TurnInvincibleForTime(0.33f);
            isJumping = false;
            playerCollider.enabled = true;
            if (bubbleCoroutine != null)
            {
                StopCoroutine(bubbleCoroutine);
            }
            ResetBubble();
            impulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized * screenShakeForce);
            SpawnParticle(stompParticlePrefab, transform.position);

            //get all colliders in a radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stompRadius, layerMask: LayerMask.GetMask("Enemy"));
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<Health>(out Health enemyHealth))
                {
                    enemyHealth.TakeDamage(stompDamage, transform.position, true);
                }
            }

    


        }

        StartCoroutine(JumpToBubbleCoroutine());

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stompRadius);
    }

    void RecallBubble()
    {
        IEnumerator RecallBubbleCoroutine()
        {

            //lerp the bubble back to the player
            float timeElapsed = 0;
            Vector3 initialPosition = bubble.transform.position;
            bool bubbleIsReallyClose = false;

            bubbleIsReallyClose = Vector3.Distance(bubble.transform.position, transform.position) < 0.25f;
            while (timeElapsed < bubbleReturnTime)
            {
                if (bubbleIsReallyClose || isJumping)
                {
                    break;
                }
                bubble.transform.position = Vector3.Lerp(initialPosition, transform.position, timeElapsed / bubbleReturnTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            if (!isJumping)
            {

                ResetBubble();
            }

        }

        bubbleCoroutine = StartCoroutine(RecallBubbleCoroutine());
    }
    void ThrowBubble()
    {


        bubble.transform.parent = null;
        isBubbled = false;
        Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
        bubbleRb.velocity = rb.velocity;
        bubbleRb.isKinematic = false;
        bubbleRb.AddForce(directionToMouse * bubbleThrowForce, ForceMode2D.Impulse);


    }

    void ResetBubble()
    {
        Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
        bubbleRb.isKinematic = true;
        bubbleRb.velocity = Vector2.zero;
        bubble.transform.parent = transform;
        bubble.transform.localPosition = Vector3.zero;
        isBubbled = true;
    }
    void FixedUpdate()
    {
        Movement();
    }
}
