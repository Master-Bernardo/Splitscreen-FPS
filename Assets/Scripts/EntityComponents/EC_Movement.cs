using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Rigidbody))]
public class EC_Movement : EntityComponent, IPusheable<Vector3>
{  
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
    Transform targetToLookAt;
    bool lookAt = false;
    float lastRotationTime; // if we rotate only once every x frames, we need to calculate our own deltaTIme

    //TODO Add dashing 
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
    float pushTreshold;
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
    //bool dashBrake = false; //are we giving force in the opposite direction?

    [Header("Debug")]
    [SerializeField]
    bool showGizmo;

    public override void SetUpComponent(GameEntity entity)
    {
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
        //almost the same speed as original navmeshAgent
        angularSpeed = agent.angularSpeed;
        //optimisation
        nextMovementUpdateTime = Time.time + Random.Range(0, movementUpdateIntervall);

        pushTreshold *= pushTreshold;
    }

    //update is only for looks- the rotation is important for logic but it can be a bit jaggy if far away or not on screen - lod this script, only call it every x seconds
    public override void UpdateComponent()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
        }

        if (lookAt)
       {
            if (Time.time > nextMovementUpdateTime)
            {
                nextMovementUpdateTime = Time.time + movementUpdateIntervall;
                if (targetToLookAt != null)
                {
                    RotateTo(targetToLookAt.position - transform.position);
                }
            }
       }

        //Debug.Log(gameObject.name + " rb speed: " + new Vector3(rb.velocity.x, 0 ,rb.velocity.z).magnitude);


    }

    public override void FixedUpdateComponent()
    {

        if (movementState == MovementState.BeingPushed)
        {

            rb.AddForce(-transform.up * (Physics.gravity.magnitude * Settings.Instance.gravityMultiplier), ForceMode.Acceleration);

            // Debug.Log("current veloity: " + rb.velocity.sqrMagnitude);
            //Debug.Log("pushTreshold: " + pushTreshold);
            float velocityThisTime = rb.velocity.sqrMagnitude;

            //because of the start of the push the velocity can also be like 0, so we only check against the treshold, when the velocity is getting smaller
            if (velocityThisTime < velocityLastTime)
            {
                if (velocityThisTime < pushTreshold)
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
                    if (rb.velocity.sqrMagnitude < pushTreshold)
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

    public virtual void Push(Vector3 force)
    {
        if (canBePushed)
        {
            //Debug.Log("destination " + agent.destination);
            if (agent.destination != null)
            {
                movementOrderIssuedWhileBeingPushed = true;
                targetMovePositionNotYetOrdered = agent.destination;
            }

            //isBeingPushed = true;
            movementState = MovementState.BeingPushed;
            Vector3 agentVelocity = agent.velocity;
            agent.enabled = false;
            rb.isKinematic = false;
            rb.velocity = agentVelocity;
            // StopLookAt();

            rb.AddForce(force, ForceMode.Impulse);
            velocityLastTime = 0;

            
        }
    }

    //works similar to push
    public virtual void Dash(Vector3 direction)
    {
        //Push(direction * dashForce);
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
        direction.y = 0;
        Quaternion desiredLookRotation = Quaternion.LookRotation(direction);
        //because we want the same speed as the agent, which has its angular speed saved as degrees per second we use the rotaate towards function
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredLookRotation, angularSpeed * deltaTime );
        lastRotationTime = Time.time;
    }

    public void SmoothRotateTo(Vector3 direction)
    {
        float deltaTime = Time.time - lastRotationTime;
        direction.y = 0;
        Quaternion desiredLookRotation = Quaternion.LookRotation(direction);
        //because we want the same speed as the agent, which has its angular speed saved as degrees per second we use the rotaate towards function
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredLookRotation, angularSpeed * deltaTime);
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


    //this method tells the agent to look at a specific target while moving
    public void LookAt(Transform targetToLookAt)
    {
        this.targetToLookAt = targetToLookAt;
        agent.updateRotation = false;
        lastRotationTime = Time.time;
        lookAt = true;
    }

    public void StopLookAt()
    {
        agent.updateRotation = true;
        lookAt = false;
    }

    public virtual bool IsMoving()
    {
            return agent.velocity.magnitude > agent.speed / 2;
    }

    public void Stop()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();

        }
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
}

