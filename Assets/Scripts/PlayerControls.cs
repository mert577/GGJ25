
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Events;

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
    public GameObject playerCoreSprite;

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


    [SerializeField]
    AnimationCurve scaleDuringJump;
    // Start is called before the first frame update
    [SerializeField]
    GameObject hurtParticlePrefab;

    [SerializeField]

    float recallForce;


    float jumpCooldown = 1f;
    float jumpCooldownTimer = 0f;


    public UnityEvent onBubbleThrown = new UnityEvent();
    public UnityEvent onBubbleReached = new UnityEvent();

    public bool isRecallingBubble = false;


    public bool canHaveBubble = true;

    public float lastVelocityMagnitude;


    public LineRenderer lineRenderer;



    IEnumerator GoIntoCanHaveBubbleCooldown()
    {
        canHaveBubble = false;
        yield return new WaitForSeconds(0.5f);
        canHaveBubble = true;
    }


    IEnumerator LerpObjectToPosition(GameObject objectToMove, float time, System.Action callback = null)
    {
        float timeElapsed = 0;
        Vector3 initialPosition = objectToMove.transform.position;
        while (timeElapsed < time)
        {
            objectToMove.transform.position = Vector3.Lerp(initialPosition, transform.position, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (callback != null)
        {
            callback();
        }
    }


    public void SpawnHurtParticle(int health)
    {
        SpawnParticle(hurtParticlePrefab, transform.position);
    }
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

        







        jumpCooldownTimer -= Time.deltaTime;
        if (jumpCooldownTimer < 0)
        {
            jumpCooldownTimer = 0;
        }


        UIManager.instance.UpdateJumpCooldown(jumpCooldownTimer / (jumpCooldown * ModifierManager.instance.TryGetModifierValue("jumpCooldown")));

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
                StartCoroutine(GoIntoCanHaveBubbleCooldown());

            }
        }

        if (Input.GetMouseButton(1))
        {
            if (!isBubbled && !isJumping)
            {
                RecallBubbleWithForce();
            }
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isBubbled && !isJumping && jumpCooldownTimer <= 0)
            {
                JumpToBubble();
            }
        }


        if (!isBubbled && canHaveBubble && bubble.transform.parent == null)
        {

            if (Vector2.Distance(transform.position, bubble.transform.position) < 1.2f)
            {

                ResetBubble();
            }

        }


    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            //as callback add coin to player and destroy coin
            StartCoroutine(LerpObjectToPosition(other.gameObject, 0.5f, () =>
            {
                Destroy(other.gameObject);
                UpgradeManager.instance.AddCoins(1);
            }));


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

            jumpCooldownTimer = jumpCooldown * ModifierManager.instance.TryGetModifierValue("jumpCooldown");


            playerCollider.enabled = false;
            bubble.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            float jumpTime = Vector3.Distance(transform.position, bubble.transform.position) / jumpSpeed;
            jumpTime = Mathf.Clamp(jumpTime, minJumpTime, maxJumpTime);
            isJumping = true;
            rb.DOMove(bubble.transform.position, jumpTime);

            playerCoreSprite.transform.DOLocalMoveY(jumpHeight, jumpTime).SetEase(jumpCurve);
            playerCoreSprite.transform.DORotate(new Vector3(0, 0, 360), jumpTime, RotateMode.FastBeyond360);
            playerCoreSprite.transform.DOScale(Vector3.one * 1.5f, jumpTime).SetEase(scaleDuringJump);
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


            playerCoreSprite.transform.localScale = Vector3.one;
            playerCoreSprite.transform.localPosition = Vector3.zero;

        }

        StartCoroutine(JumpToBubbleCoroutine());

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stompRadius);
    }


    void RecallBubbleWithForce()
    {
        Vector2 force = (transform.position - bubble.transform.position);
        force = Vector2.ClampMagnitude(force, 1f);



        force *= recallForce;
        bubble.GetComponent<Rigidbody2D>().AddForce(force);
    }

    void RecallBubble()
    {
        IEnumerator RecallBubbleCoroutine()
        {
            isRecallingBubble = true;

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
        bubbleRb.AddForce(directionToMouse * bubbleThrowForce * ModifierManager.instance.TryGetModifierValue("throwForce"), ForceMode2D.Impulse);
        onBubbleThrown.Invoke();

    }

    void ResetBubble()
    {
        if (isRecallingBubble)
        {
            return;
        }
        StartCoroutine(_());
        IEnumerator _()
        {

            Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
            bubbleRb.isKinematic = true;
            bubbleRb.velocity = Vector2.zero;
            bubble.transform.parent = transform;

            float lerpTime = .12f;
            float timeElapsed = 0;

            while (timeElapsed < lerpTime)
            {
                bubble.transform.localPosition = Vector3.Lerp(bubble.transform.localPosition, Vector3.zero, timeElapsed / lerpTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            isRecallingBubble = false;
            isBubbled = true;
            onBubbleReached.Invoke();
        }

    }
    void FixedUpdate()
    {
        Movement();




    }
}
