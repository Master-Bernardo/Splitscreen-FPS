using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EC_Movement : EntityComponent, IPusheable<Vector3>
{
    #region Fields
    [HideInInspector]
    NavMeshAgent agent;

    //for rotation independent of navmeshAgent;
    protected float angularSpeed;

    //for optimisation we can call the updater only every x frames
    float nextMovementUpdateTime;
    [Header("PerformanceOptimisation")]
    [SerializeField]
    float movementUpdateIntervall = 1 / 6;

    //our agent can either rotate to the direction he is facing or have a target to which he is alwys rotated to - if lookAt is true
    bool lookAt = false;
    protected float lastRotationTime; // if we rotate only once every x frames, we need to calculate our own deltaTIme

    protected enum MovementState
    {
        Default,
        BeingPushed,
        Dashing
    }
    protected MovementState movementState;

    [Header("References")]
    //pushing with rb
    [SerializeField]
    protected Rigidbody rb;
    //public float gravityMultiplier;

    [Header("PushPhysics")]
    public bool canBePushed;
    //bool isBeingPushed = false;
    [Tooltip("under which velocity is the pushed agent not considered pushed anymore")]
    [SerializeField]
    float pushEndTreshold;
    [Tooltip("a force must be larger than this force to initiate a push")]
    [SerializeField]
    float pushBeginnTreshold;
    float velocityLastTime;
    bool movementOrderIssuedWhileBeingPushed = false;
    Vector3 targetMovePositionNotYetOrdered;

    [Header("Dash")]
    //dashing with rb
    public float dashForce;
    public float dashTime;//how long does the dash force gets applied?


    float nextDashEndTime;
    Vector3 dashDirection;
    public float dashMultiplier; // when an agents gets his rb activated and dashes, he does not have the same gravity forces applied to it as the player so we modify his dash push force value

    [Header("Debug")]
    [SerializeField]
    bool showGizmo;

    [Header("SpineRotation")]
    [Tooltip("only used by humanoids")]
    public bool useSpine;
    public Transform spine;

    #endregion

    public override void SetUpComponent(GameEntity entity)
    {
        agent = GetComponent<NavMeshAgent>();
        //almost the same speed as original navmeshAgent
        angularSpeed = agent.angularSpeed;
        //optimisation
        nextMovementUpdateTime = Time.time + Random.Range(0, movementUpdateIntervall);

        pushEndTreshold *= pushEndTreshold; //because we cheking against squared values for optimisation
        pushBeginnTreshold *= pushBeginnTreshold;
    }

    //update is only for looks- the rotation is important for logic but it can be a bit jaggy if far away or not on screen - lod this script, only call it every x seconds
    public override void UpdateComponent()
    {
        // Debug.Log("mov update");
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            if(agent!=null)MoveTo(transform.position + new Vector3(0, 0, 15));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Push(new Vector3(1000f, 0f, 0));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Push(new Vector3(0f, 3000f, 0));

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Dash(Vector3.right);
        }*/

        if (!lookAt)
        {
            if (useSpine)
            {
                //reset the spine to normal position if look at is diabled
                if (spine.localRotation.eulerAngles != Vector3.zero)
                {
                    float deltaTime = Time.time - lastRotationTime;

                    Quaternion desiredSpineRotation = Quaternion.Euler(0, 0, 0);

                    spine.localRotation = Quaternion.RotateTowards(spine.localRotation, desiredSpineRotation, angularSpeed * deltaTime);

                    lastRotationTime = Time.time;
                }

            }
        }
    }

    public override void FixedUpdateComponent()
    {
        if (movementState == MovementState.BeingPushed)
        {

            rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);

            float velocityThisTime = rb.velocity.sqrMagnitude;

            //because of the start of the push the velocity can also be like 0, so we only check against the treshold, when the velocity is getting smaller
            if (velocityThisTime < velocityLastTime)
            {
                if (velocityThisTime < pushEndTreshold)
                {
                    //the unit can only move back to normal movement - if it is grounded -> raycast against navmesh
                    
                    //check if gorunded
                    RaycastHit hit;
                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(transform.position + transform.up*0.1f, -Vector3.up, out hit, 0.2f))
                    {
                        movementState = MovementState.Default;
                        if (agent != null)
                        {
                            agent.enabled = true;
                            rb.isKinematic = true;
                        }

                        if (movementOrderIssuedWhileBeingPushed)
                        {
                            MoveTo(targetMovePositionNotYetOrdered);
                        }
                    }               
                }
            }


            velocityLastTime = rb.velocity.sqrMagnitude;
        }
        else if (movementState == MovementState.Dashing)
        {
           rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);


            if (Time.time > nextDashEndTime)
            {
                //dashBrake = true;
                if (agent != null)
                {
                    if (rb.velocity.sqrMagnitude < pushEndTreshold)
                    {
                        movementState = MovementState.Default;

                        agent.enabled = true;
                        rb.isKinematic = true;

                        if (movementOrderIssuedWhileBeingPushed)
                        {
                            MoveTo(targetMovePositionNotYetOrdered);
                        }
                    }
                }
                else
                {
                    movementState = MovementState.Default;
                }

            }
            else
            {
                //Debug.Log(gameObject.name + " force added: " + dashDirection * dashForce * Settings.Instance.forceMultiplier);
                if (agent != null)
                {
                    rb.AddForce(dashDirection * dashForce * Settings.Instance.forceMultiplier * dashMultiplier, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(dashDirection * dashForce * Settings.Instance.forceMultiplier, ForceMode.Acceleration);
                }
            }

        }
    }

    #region Movement Orders

    public virtual void Push(Vector3 force)
    {
        if (canBePushed)
        {
            //Debug.Log("push force is: " + force.sqrMagnitude);
            //Debug.Log("must be higher than: " + pushBeginnTreshold);
            if (force.sqrMagnitude > pushBeginnTreshold)
            {
                if (agent.destination != null)
                {
                    movementOrderIssuedWhileBeingPushed = true;
                    targetMovePositionNotYetOrdered = agent.destination;
                }

                movementState = MovementState.BeingPushed;
                Vector3 agentVelocity = agent.velocity;
                agent.enabled = false;
                rb.isKinematic = false;
                rb.velocity = agentVelocity;

                rb.AddForce(force, ForceMode.Impulse);
                velocityLastTime = 0;
            }
        }
    }

    //works similar to push
    public virtual void Dash(Vector3 direction)
    {
        //if a units wants to use dash points, do it in the behaviour
        if (agent != null)
        {
            if (agent.destination != null)
            {
                movementOrderIssuedWhileBeingPushed = true;
                targetMovePositionNotYetOrdered = agent.destination;
            }

            Vector3 agentVelocity = agent.velocity;
            agent.enabled = false;
            rb.isKinematic = false;
            rb.velocity = agentVelocity;
        }
      
        dashDirection = direction;
        nextDashEndTime = Time.time + dashTime;
        movementState = MovementState.Dashing;
    }

    //sets the agent to rotate 
    public void RotateTo(Vector3 direction)
    {
        float deltaTime = Time.time - lastRotationTime;

        if (useSpine)
        {
            //only rotate on local x direction
            //first delete side movements- only leave y and z
            Vector3 directionForSpine = transform.InverseTransformDirection(direction);

            Quaternion desiredSpineRotation = Quaternion.LookRotation(directionForSpine);
            desiredSpineRotation = Quaternion.Euler(desiredSpineRotation.eulerAngles.x, 0, 0);
            spine.localRotation = Quaternion.RotateTowards(spine.localRotation, desiredSpineRotation, angularSpeed * deltaTime);
        }


        direction.y = 0;
        Quaternion desiredLookRotation = Quaternion.LookRotation(direction);
        //because we want the same speed as the agent, which has its angular speed saved as degrees per second we use the rotaate towards function
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredLookRotation, angularSpeed * deltaTime );
        lastRotationTime = Time.time;
    }

    //for now simple moveTo without surface ship or flying
    public void MoveTo(Vector3 destination)
    {
        if (movementState == MovementState.Default)
        {
            agent.SetDestination(destination);
        }
        else
        {
            movementOrderIssuedWhileBeingPushed = true;
            targetMovePositionNotYetOrdered = destination;
        }
    }

    public void Stop()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();

        }
    }

    #endregion

    #region LookAt

    //this enables the use of the method lookAT
    public void ActivateLookAt()
    {
        lookAt = true;
        lastRotationTime = Time.time;
        agent.updateRotation = false;
    }

    //looks into the desired direction, needs to be called every frame
    public void LookAt(Vector3 direction)
    {
        RotateTo(direction);
    }

    public void StopLookAt()
    {
        agent.updateRotation = true;
        lookAt = false;
    }


    #endregion

    #region Status Checks
    public virtual bool IsMoving()
    {
         return agent.velocity.magnitude > agent.speed / 2;
    }

    public float GetCurrentVelocityMagnitude()
    {
        return agent.velocity.magnitude;
    }

    public virtual Vector3 GetCurrentVelocity()
    {
        return agent.velocity;
    }

    public float GetMaxSpeed()
    {
        return agent.speed;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
     {
         if (showGizmo && agent!=null)
         {
             if (agent.destination != null)
             {
                 Gizmos.color = Color.green;
                 Gizmos.DrawCube(agent.destination, new Vector3(0.2f, 2, 0.2f));

             }
         }
     }

    #endregion
}

