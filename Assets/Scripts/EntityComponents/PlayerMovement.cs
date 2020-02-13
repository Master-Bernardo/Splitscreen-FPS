using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EC_Movement, IPusheable<Vector3>
{
    #region Fields
    [Header("Player Movement")]
    public float targetMovementSpeed;
    public float maxAcceleration;
    [Tooltip("make desseleration slower than acceleration for better explosioneffects")]
    public float maxDecceleration;

    [Tooltip("after changing  maxRotationSpeed or angularDrag, we need to recallibrate PID")]
    public float rotationSpeed;

    public float playerAngularSpeed;

    bool jump = false;
    public float jumpForce;

    bool grounded = false; //do we touch the earth?
    public Transform rayCastStartPosition;
    public float groundedCheckRaycastDistance;
    public LayerMask groundedCheckLayermask;
    public float groundedCheckCapsuleHeight;
    public bool drawRaycastGizmo;

    [Header("Dash")]
    public bool useDashPoints;
    [Tooltip("like stamina - replenishes itself after time, only for player because of performance optimisation")]
    public float maxDashPoints;
    float currentDashPoints;
    public float dashPointReplenishmentSpeed;
    public DashPointsUI dashUI;

    #endregion

    public override void SetUpComponent(GameEntity entity)
    {
        currentDashPoints = maxDashPoints;

        angularSpeed = rotationSpeed;
        maxAcceleration *= Settings.Instance.forceMultiplier;
        maxDecceleration *= Settings.Instance.forceMultiplier;
        jumpForce *= Settings.Instance.forceMultiplier;
        angularSpeed = playerAngularSpeed;

        if(dashUI) dashUI.SetUp((int)maxDashPoints);
    }

    public override void UpdateComponent()
    {

    }

    #region Applying Movement/Force

    //gets called in fixed update, used by topdown controller
    public void UpdateMovement(Vector3 movementVector)
    {
        //custom gravity
        rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);

        #region apply physics-based movement

        bool playerWantsToMove = false;

        if (!grounded)
        {
            if (movementVector != new Vector3(0, 0, 0))
            {
                playerWantsToMove = true;
            }
        }
        else
        {
            //if we are being pushed or dashing, and there is no input from the player, we should not deccelerate
            if (movementVector != new Vector3(0, 0, 0) || movementVector == new Vector3(0, 0, 0) && (movementState == MovementState.Default))
            {
                playerWantsToMove = true;
            }
        }

        if (playerWantsToMove)
        {
            //movement
            Vector3 rbHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 targetVelocity = movementVector * targetMovementSpeed;

            Vector3 deltaV = targetVelocity - rbHorizontalVelocity;

            Vector3 accel = deltaV / Time.fixedDeltaTime;

            if ((rbHorizontalVelocity + accel.normalized).sqrMagnitude > rbHorizontalVelocity.sqrMagnitude)
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

        #endregion

        #region check if grounded

        Collider[] colliders = Physics.OverlapCapsule(rayCastStartPosition.position, rayCastStartPosition.position + new Vector3(0, groundedCheckCapsuleHeight, 0), groundedCheckRaycastDistance, groundedCheckLayermask);

        if (colliders.Length > 1)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        #endregion

        #region jump & dash
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
        dashUI.UpdateDashPoints((int)currentDashPoints);
        #endregion
    }


    //spine and body rotation script, only for player, the ai guys have other scripts, used in topdown mode
    public void SmoothRotateTo(Vector3 direction)
    {
        //float deltaTime = Time.time - lastRotationTime;

        if (useSpine)
        {
            Quaternion desiredSpineRotation = Quaternion.LookRotation(new Vector3(spine.transform.forward.x, direction.y, spine.transform.forward.z));
            spine.rotation = Quaternion.RotateTowards(spine.rotation, desiredSpineRotation, angularSpeed*3 * Time.fixedDeltaTime); //cause rotateTowards and Lerp use differend speed, we increase this speed by 4 - this 4 is only an approximation
        }

        direction.y = 0;
        Quaternion desiredLookRotation = Quaternion.LookRotation(direction);
        //because we want the same speed as the agent, which has its angular speed saved as degrees per second we use the rotaate towards function
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredLookRotation, angularSpeed * Time.fixedDeltaTime);
        //lastRotationTime = Time.time;
    }

    //used in fps mode
    public void InstantRotateTo(Vector3 direction)
    {
        float deltaTime = Time.time - lastRotationTime;

        if (useSpine)
        {
            spine.rotation = Quaternion.LookRotation(new Vector3(spine.transform.forward.x, direction.y, spine.transform.forward.z));
        }

        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
        lastRotationTime = Time.time;
    }

    public void Jump()
    {
        jump = true;
    }

    public override void Dash(Vector3 direction)
    {
        if (grounded)
        {
            if (currentDashPoints >= 1)
            {
                base.Dash(direction);
                currentDashPoints--;
            }
        } 
    }

    public override void Push(Vector3 force)
    {
        if (canBePushed)
        {
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

    #endregion

    #region Status Checks

    public override bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1;
    }

    public override Vector3 GetCurrentVelocity()
    {
        return rb.velocity;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (drawRaycastGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rayCastStartPosition.position, groundedCheckRaycastDistance);
            Gizmos.DrawWireSphere(rayCastStartPosition.position + new Vector3(0, groundedCheckCapsuleHeight, 0), groundedCheckRaycastDistance);

        }
    }

    #endregion
}
