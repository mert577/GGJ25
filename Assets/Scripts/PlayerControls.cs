
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


    // Start is called before the first frame update


    void SpawnParticle(GameObject particlePrefab, Vector3 position)
    {
        GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
        Destroy(particle, 1);
    }

    void Start()
    {

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
            float jumpTime = Vector3.Distance(transform.position, bubble.transform.position) / jumpSpeed;
            jumpTime = Mathf.Clamp(jumpTime, minJumpTime, maxJumpTime);
            isJumping = true;
            rb.DOMove(bubble.transform.position, jumpTime);
            playerCoreSprite.transform.DOLocalMoveY(jumpHeight, jumpTime).SetEase(jumpCurve);
            yield return new WaitForSeconds(jumpTime);
            isJumping = false;
            StopCoroutine(bubbleCoroutine);
            ResetBubble();
            impulseSource.GenerateImpulseWithVelocity(Random.insideUnitCircle.normalized * screenShakeForce);
            SpawnParticle(stompParticlePrefab, transform.position);

        }

        StartCoroutine(JumpToBubbleCoroutine());

    }


    void ThrowBubble()
    {

        //set bubble parent to null so it doesn't follow the player
        //add force to the bubble in the direction of the mouse

        //after a few seconds lerp the bubble back to the player
        IEnumerator ThrowBubbleCoroutine()
        {
            bubble.transform.parent = null;
            isBubbled = false;
            Rigidbody2D bubbleRb = bubble.GetComponent<Rigidbody2D>();
            bubbleRb.velocity = rb.velocity;
            bubbleRb.isKinematic = false;
            bubbleRb.AddForce(directionToMouse * bubbleThrowForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(bubbleWaitTime);


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

        bubbleCoroutine = StartCoroutine(ThrowBubbleCoroutine());

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
