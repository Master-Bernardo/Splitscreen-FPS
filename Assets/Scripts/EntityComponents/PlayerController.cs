using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

    public Vector3 currentLookVector;

     Vector3 movementVector;

    public PlayerMovement playerMovement;


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

        //Vector2 direction = cam.WorldToScreenPoint(transform.position) - Input.mousePosition;
        Vector2 direction = new Vector3(Screen.width / 2, Screen.height / 2, 0f) - Input.mousePosition;
        //Vector2 direction = new Vector3(0.5f,0.5f,0) - Input.mousePosition;
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
