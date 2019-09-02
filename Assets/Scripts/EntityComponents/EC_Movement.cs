using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EC_Movement : EntityComponent, IPusheable<Vector3>
{  
    [HideInInspector]
    NavMeshAgent agent;

    //for rotation independent of navmeshAgent;
    float angularSpeed;

    public bool showGizmo;


    //for optimisation we can call the updater only every x frames
    float nextMovementUpdateTime;
    [SerializeField]
    float movementUpdateIntervall = 1 / 6;

    //our agent can either rotate to the direction he is facing or have a target to which he is alwys rotated to - if lookAt is true
    Transform targetToLookAt;
    bool lookAt = false;
    float lastRotationTime; // if we rotate only once every x frames, we need to calculate our own deltaTIme

    //pushing with rb
    protected Rigidbody rb;
    public bool canBePushed;
    bool isBeingPushed = false;
    [Tooltip("under which velocity is the pushed agent not considered pushed anymore")]
    [SerializeField]
    float pushTreshold;
    float velocityLastTime;
    bool movementOrderIssuedWhileBeingPushed = false;
    Vector3 targetMovePositionNotYetOrdered;

    public override void SetUpComponent(GameEntity entity)
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //almost the same speed as original navmeshAgent
        angularSpeed = agent.angularSpeed;
        //optimisation
        nextMovementUpdateTime = Time.time + Random.Range(0, movementUpdateIntervall);

        pushTreshold *= pushTreshold;
    }

    //update is only for looks- the rotation is important for logic but it can be a bit jaggy if far away or not on screen - lod this script, only call it every x seconds
    public override void UpdateComponent()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("move");
            MoveTo(transform.position + new Vector3(-5, 0 - 5));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Push(new Vector3(50000f, 5000f, 0));
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

        if (isBeingPushed)
        {
           // Debug.Log("current veloity: " + rb.velocity.sqrMagnitude);
            //Debug.Log("pushTreshold: " + pushTreshold);
            float velocityThisTime = rb.velocity.sqrMagnitude;

            //because of the start of the push the velocity can also be like 0, so we only check against the treshold, when the velocity is getting smaller
            if (velocityThisTime < velocityLastTime)
            {
                if (velocityThisTime < pushTreshold)
                {
                    isBeingPushed = false;
                    agent.enabled = true;
                    rb.isKinematic = true;

                    if (movementOrderIssuedWhileBeingPushed)
                    {
                        MoveTo(targetMovePositionNotYetOrdered);
                    }
                }
            }
           

            velocityLastTime = rb.velocity.sqrMagnitude;
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

            isBeingPushed = true;
            agent.enabled = false;
            rb.isKinematic = false;
           // StopLookAt();

            rb.AddForce(force);
            velocityLastTime = 0;

            
        }
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

    //for now simple moveTo without surface ship or flying
    public void MoveTo(Vector3 destination)
    {
        if (!isBeingPushed)
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

