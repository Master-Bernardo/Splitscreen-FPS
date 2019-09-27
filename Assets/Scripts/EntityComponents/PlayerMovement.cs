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

    public float gravityMultiplier;

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
    }

    

    public void UpdateMovement(Vector3 currentLookVector, Vector3 movementVector)
    {
        rb.AddForce(-transform.up * (Physics.gravity.magnitude * gravityMultiplier));

        rb.AddForce(movementVector * moveAcceleration * Time.deltaTime, ForceMode.Impulse);


        Vector3 velocityToCheck = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (velocityToCheck.magnitude > maxMovementSpeed)
        {
            velocityToCheck = velocityToCheck.normalized * maxMovementSpeed;
            //Debug.Log("too fast");
            rb.velocity = new Vector3(velocityToCheck.x, rb.velocity.y, velocityToCheck.z);
        }

        transform.forward = Vector3.Lerp(transform.forward, currentLookVector, rotationSpeed * Time.deltaTime);

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
            rb.AddForce(force);
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
