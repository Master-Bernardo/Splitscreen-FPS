using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EC_Movement, IPusheable<Vector3>
{
    [Header("Player Movement")]
    public float targetMovementSpeed;
    public float maxAcceleration;
    [Tooltip("make desseleration slower than acceleration for better explosioneffects")]
    public float maxDecceleration;

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
        maxAcceleration *= Settings.Instance.forceMultiplier;
        maxDecceleration *= Settings.Instance.forceMultiplier;
        jumpForce *= Settings.Instance.forceMultiplier;

    }


    //gets called in fixed update
    public void UpdateMovement(Vector3 currentLookVector, Vector3 movementVector)
    {
        //Debug.Log("actualVelcoity This frame: " + new Vector3(rb.velocity.x, 0, rb.velocity.z));
        //gravity
        rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);

        if(movementVector!=new Vector3(0, 0, 0))
        {
            //movement
            Vector3 rbHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 targetVelocity = movementVector * targetMovementSpeed;

            Vector3 deltaV = targetVelocity - rbHorizontalVelocity;

            Vector3 accel = deltaV / Time.deltaTime;

            if((rbHorizontalVelocity + accel.normalized).sqrMagnitude > rbHorizontalVelocity.sqrMagnitude)
            {
                //Debug.Log("accelerate");
                if (accel.sqrMagnitude > maxAcceleration * maxAcceleration)
                    accel = accel.normalized * maxAcceleration;
            }
            else
            {
                //Debug.Log("deccelerate");
                if (accel.sqrMagnitude > maxDecceleration * maxDecceleration)
                    accel = accel.normalized * maxDecceleration;
            }


           

            rb.AddForce(accel, ForceMode.Acceleration);
        }


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
