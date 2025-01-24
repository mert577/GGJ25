using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{


    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Vector2 movementInput;

    [SerializeField]
    float maxSpeed ,accelerationForce;


    [SerializeField]
    Vector2  directionToMouse;

    [SerializeField]
    GameObject bubble;

    [SerializeField]
    float bubbleThrowForce, bubbleWaitTime, bubbleReturnTime;
  
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }

        if(Input.GetMouseButtonDown(0))
        {
            ThrowBubble();
        }
        
        CalculateDirectionToMouse();
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
    }


    void ThrowBubble(){

        //set bubble parent to null so it doesn't follow the player
        //add force to the bubble in the direction of the mouse

        //after a few seconds lerp the bubble back to the player
        IEnumerator ThrowBubbleCoroutine()
        {
            bubble.transform.parent = null;
            bubble.GetComponent<Rigidbody2D>().AddForce(directionToMouse * 10, ForceMode2D.Impulse);
            yield return new WaitForSeconds(bubbleWaitTime);


            //lerp the bubble back to the player
            float timeElapsed = 0;
            Vector3 initialPosition = bubble.transform.position;
            while (timeElapsed < bubbleReturnTime)
            {
                bubble.transform.position = Vector3.Lerp(initialPosition, transform.position, timeElapsed / bubbleReturnTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            


            bubble.transform.parent = transform;
        }

        StartCoroutine(ThrowBubbleCoroutine());

    }


    void FixedUpdate()
    {
      Movement();
    }
}
