using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    //public Rigidbody rb;
    //public float maxMovementSpeed;
    //public float moveAcceleration;

    //[Tooltip("after changing rotationAcceleration or maxRotationSpeed or angularDrag, we need to recallibrate PID")]
    //public float rotationAcceleration; //- is this needed?
    //public float maxRotationSpeed;
    public Vector3 currentLookVector;

     Vector3 movementVector;

    public PlayerMovement playerMovement;

    /*
     bool jump;
     public float jumpForce;

     public float gravityMultiplier;

     [Header("PID Controller")]
     //for PID Controller
     public float pGain = 1f;
     public float iGain = 1f;
     public float dGain = 0.4f;
     float lastPError = 0; */

    /* https://robotics.stackexchange.com/questions/167/what-are-good-strategies-for-tuning-pid-loops
     * 
     * 1. Set all gains to zero.
       2. Increase the P gain until the response to a disturbance is steady oscillation.
       3. Increase the D gain until the the oscillations go away (i.e. it's critically damped).
       4. Repeat steps 2 and 3 until increasing the D gain does not stop the oscillations.
       5. Set P and D to the last stable values.
       6.Increase the I gain until it brings you to the setpoint with the number of oscillations desired (normally zero but a quicker response can be had if you don't mind a couple oscillations of overshoot)
     */

    void Start()
    {
        currentLookVector = transform.forward;
    }

    void Update()
    {

        //get movement

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 camRight = (cam.transform.right).normalized;
        Vector3 horV = camRight * hor;
        Vector3 verV = new Vector3(-camRight.z, 0f, camRight.x) * ver;
        movementVector = horV + verV;

        //agent.SetDestination(transform.position + movementVector.normalized*0.5f);

        //get rotation

        Vector2 direction = new Vector3(Screen.width / 2, Screen.height / 2, 0f) - Input.mousePosition;
        currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y + 180, 0) * new Vector3(direction.x, 0f, direction.y);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
        }

    }

    private void FixedUpdate()
    {
        playerMovement.UpdateMovement(currentLookVector, movementVector);
    }
}
