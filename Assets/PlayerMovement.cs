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
    public float maxRotationSpeed;

    //public Vector3 currentLookVector;

    //Vector3 movementVector;

    bool jump;
    public float jumpForce;

    public float gravityMultiplier;

    [Header("PID Controller")]
    //for PID Controller
    public float pGain = 1f;
    public float iGain = 1f;
    public float dGain = 0.4f;
    float lastPError = 0;

    /* https://robotics.stackexchange.com/questions/167/what-are-good-strategies-for-tuning-pid-loops
    * 
    * 1. Set all gains to zero.
      2. Increase the P gain until the response to a disturbance is steady oscillation.
      3. Increase the D gain until the the oscillations go away (i.e. it's critically damped).
      4. Repeat steps 2 and 3 until increasing the D gain does not stop the oscillations.
      5. Set P and D to the last stable values.
      6.Increase the I gain until it brings you to the setpoint with the number of oscillations desired (normally zero but a quicker response can be had if you don't mind a couple oscillations of overshoot)
    */




   public override void SetUpComponent(GameEntity entity)
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void UpdateComponent()
    {
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

        //rotation
        //Quaternion desiredRotation = Quaternion.LookRotation(currentLookVector);

        float deltaTime = Time.deltaTime;
        //PID Code
        float pError = Vector3.SignedAngle(transform.forward, currentLookVector, transform.up);
        float iError = pError * deltaTime;
        float dError = (pError - lastPError) / deltaTime;
        lastPError = pError;

        float torque = (pGain * pError + iGain * iError + dGain * dError);// * rotationAcceleration;
        //we do nod necessary ned to multiply with rotationAcceleration - but it would be nice if this also makes a difference

        //cklamp - set a max rotation velocity
        if (torque > maxRotationSpeed) torque = maxRotationSpeed;
        else if (torque < -maxRotationSpeed) torque = -maxRotationSpeed;

        rb.AddTorque(transform.up * torque);




        if (jump)
        {
            Debug.Log("Jump");
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }
    }


    public void Jump()
    {
        jump = true;
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
