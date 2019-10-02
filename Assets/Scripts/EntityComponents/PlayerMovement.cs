using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EC_Movement, IPusheable<Vector3>
{
    [Header("Player Movement")]
    public float maxMovementSpeed;
    public float moveAcceleration;

    [Tooltip("after changing  maxRotationSpeed or angularDrag, we need to recallibrate PID")]
    public float rotationSpeed;

    //public Vector3 currentLookVector;

    //Vector3 movementVector;

    bool jump = false;
    public float jumpForce;

    //public float gravityMultiplier;

    bool grounded = false; //do we touch the earth?
    public Transform rayCastStartPosition;
    public float groundedCheckRaycastDistance;
    public LayerMask groundedCheckLayermask;

    [Tooltip("like stamina - replenishes itself after time, only for player because of performance optimisation")]
    public float maxDashPoints;
    float currentDashPoints;
    public float dashPointReplenishmentSpeed;




    public override void SetUpComponent(GameEntity entity)
    {
        rb = GetComponent<Rigidbody>();
        currentDashPoints = maxDashPoints;

        angularSpeed = rotationSpeed;
    }

    
    //gets called in fixed update
    public void UpdateMovement(Vector3 currentLookVector, Vector3 movementVector)
    {
        Debug.Log("actualVelcoity This frame: " + new Vector3(rb.velocity.x, 0, rb.velocity.z));
        //gravity
        rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);

        //movement

        //how to cap the force correctly? - do not cap the velocity, instead cap the velocity we add
        Vector3 forceToAddThisFrame = movementVector * moveAcceleration * Time.deltaTime;
        Vector3 cappedForce = forceToAddThisFrame;

        Vector3 rbHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        Vector3 resultingVelocity = rbHorizontalVelocity + forceToAddThisFrame;
        Debug.Log("predicted velocity next frame: " + resultingVelocity);
        Debug.Log("resulting magnitude: " + resultingVelocity.magnitude);
        if (resultingVelocity.magnitude > maxMovementSpeed)
        {
            //check if our current moveemtn would raise or lower magnitude, if it lowers it- tan we allow it
            if (resultingVelocity.magnitude > rbHorizontalVelocity.magnitude)
            {
                Debug.Log("cap");
                //cappedForce = new Vector3(0, 0, 0);
            }
        }
        Debug.Log("curr force: "+ cappedForce);
        rb.AddForce(cappedForce, ForceMode.Acceleration);

       // rb.MovePosition(transform.position + movementVector * moveAcceleration * Time.deltaTime);

        //rb.AddForce(movementVector * moveAcceleration * Time.deltaTime, ForceMode.Impulse);

        //Vector3 velocityToCheck = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        /*
        if (velocityToCheck.magnitude > maxMovementSpeed)
        {
            velocityToCheck = velocityToCheck.normalized * maxMovementSpeed;
            //Debug.Log("too fast");
            rb.velocity = new Vector3(velocityToCheck.x, rb.velocity.y, velocityToCheck.z);
        }*/

        SmoothRotateTo(currentLookVector);
        //transform.forward = Vector3.Lerp(transform.forward, currentLookVector.normalized, rotationSpeed * Time.deltaTime);

        //check if gorunded
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(rayCastStartPosition.position, -Vector3.up, out hit, groundedCheckRaycastDistance))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (jump)
        {
            if (grounded)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);       
            }

            jump = false;
        }

       
        currentDashPoints += dashPointReplenishmentSpeed * Time.deltaTime;
        if (currentDashPoints > maxDashPoints) currentDashPoints = maxDashPoints;
        
    }


    public void Jump()
    {
        jump = true;
    }

    public override void Dash(Vector3 direction)
    {
        if (currentDashPoints >= 1)
        {
            base.Dash(direction);
            currentDashPoints--;
        }
    }

    public override void Push(Vector3 force)
    {
        if (canBePushed)
        {
            Debug.Log("player push; " + force.magnitude);
            Debug.Log("force: " + force);

            rb.AddForce(force, ForceMode.Impulse);
            //rb.AddForce(force);
        }
    }

    public override bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1;
    }

    public override Vector3 GetCurrentVelocity()
    {
        return rb.velocity;
    }
}
