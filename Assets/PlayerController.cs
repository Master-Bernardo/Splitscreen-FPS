using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Camera cam;
    public int layerMaskForMove;
    public Rigidbody rb;
    public float movementSpeed;
    public float rotationSpeed;

    public Vector3 currentLookVector;

    Vector3 movementVector;

    bool jump;
    public float jumpForce;

    void Start()
    {
        currentLookVector = transform.forward;
    }


    
    void Update()
    {
        //old click to move
        /*if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1 << layerMaskForMove;

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                agent.SetDestination(hit.point);
               // Debug.Log("move to : " + hit.point);
            }

        }*/

        //get movement

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 camRight = (cam.transform.right).normalized;
        Vector3 horV = camRight * hor;
        Vector3 verV = new Vector3(-camRight.z, 0f, camRight.x) * ver;
        movementVector = horV + verV;

        //agent.SetDestination(transform.position + movementVector.normalized*0.5f);

        //get rotation
        if (Input.GetMouseButton(0))
        {
            Debug.Log(Input.mousePosition);
            Vector2 direction = new Vector3(Screen.width / 2, Screen.height / 2, 0f) - Input.mousePosition ;
            currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y+180, 0) * new Vector3(direction.x, 0f, direction.y);

            //transform.forward = currentLookVector;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movementVector * movementSpeed * Time.deltaTime);
        Quaternion desiredRotation = Quaternion.LookRotation(currentLookVector);
        Quaternion rotationThisRound = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        rb.MoveRotation(rotationThisRound);

        if (jump)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
