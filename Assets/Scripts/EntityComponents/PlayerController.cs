using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera cam;

   

    public PlayerMovement playerMovement;
    public PlayerInput playerInput;
    public WeaponSystem weaponSystem;
    public InteractableShower interactableShower;

    public Vector3 currentLookVector;
    Vector3 movementVector;

    Vector3 movementInputVector;
    Vector2 lookInputVector; // for controller
    Vector2 lookInputVectorLastFrame; //for better controls
    Vector2 currentMousePosition; //for keyboard & mouse

    bool weaponPressed = false;
    int pressedWeaponID;

    bool interacting = false;

    #region controls

    /* private void Awake()
     {
         controls.Player.Shoot.performed += ctx => ShootTest(3);
     }

     private void OnEnable()
     {
         controls.Enable();
     }

     private void OnDisable()
     {
         controls.Disable();
     }*/

    public void OnMovement(InputValue value)
    {
        movementInputVector = value.Get<Vector2>();
    }

    public void OnW1Press()
    {
        Debug.Log("shoot");
        weaponSystem.UseWeaponStart(0);
        pressedWeaponID = 0;
        weaponPressed = true;
    }

    public void OnW1Release()
    {
        Debug.Log("shootRelease");
        weaponPressed = false;
    }

    public void OnReload()
    {
        weaponSystem.ReloadWeapon();
    }

    public void OnInteractStart()
    {
        interacting = true;
        interactableShower.StartInteract();
    }

    public void OnInteractStop()
    {
        interacting = false;
        interactableShower.StopInteract();
    }

    void OnJump()
    {
        playerMovement.Jump();
    }

    void OnRotateTowards(InputValue value)
    {
        //Debug.Log("control sheme: " + playerInput.controlScheme);
        if(playerInput.controlScheme == "Gamepad")
        {
            Debug.Log("rotate towards");
            lookInputVector = value.Get<Vector2>();
            //Debug.Log("gamepad");
            if(lookInputVector == new Vector2(0,0))
            {
                Debug.Log("is zero");
                lookInputVector = lookInputVectorLastFrame;
            }
        }
        else
        {
            currentMousePosition = value.Get<Vector2>();
            //Debug.Log("keyboard");
        }
    }

    #endregion
    void Start()
    {
        currentLookVector = transform.forward;
    }



    void Update()
    {

        //get movement
        float hor = movementInputVector.x;
        float ver = movementInputVector.y;

        Vector3 camRight = (cam.transform.right).normalized;
        Vector3 horV = camRight * hor;
        Vector3 verV = new Vector3(-camRight.z, 0f, camRight.x) * ver;
        movementVector = horV + verV;

        //agent.SetDestination(transform.position + movementVector.normalized*0.5f);

        //get rotation

        //Vector2 direction = cam.WorldToScreenPoint(transform.position) - Input.mousePosition;

        //Vector2 direction = new Vector3(0.5f,0.5f,0) - Input.mousePosition;

        //Vector2 direction = new Vector3(Screen.width / 2, Screen.height / 2, 0f) - Input.mousePosition;
        //currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y + 180, 0) * new Vector3(direction.x, 0f, direction.y);

        //rotate towards

        if (playerInput.controlScheme == "Gamepad")
        {
            //Debug.Log("look InputVector: " + lookInputVector);
            currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y, 0) * new Vector3(lookInputVector.x, 0f, lookInputVector.y);

            lookInputVectorLastFrame = lookInputVector;
        }
        else
        {
            // Vector2 direction = new Vector3(Screen.width / 2, Screen.height / 2, 0f) - new Vector3(currentMousePosition.x,0, currentMousePosition.y);
            Vector2 playerPos = new Vector3(cam.WorldToScreenPoint(transform.position).x, cam.WorldToScreenPoint(transform.position).y);
            
           Vector2 direction = playerPos -  currentMousePosition;
          // Debug.Log("player pos: " + playerPos);
           //Debug.Log("mouse pox: " + new Vector3(currentMousePosition.x, 0, currentMousePosition.y));
           //Debug.Log("direction: " + direction);
           currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y + 180, 0) * new Vector3(direction.x, 0f, direction.y);
        }


        //weapon
        if (weaponPressed)
        {
            weaponSystem.UseWeaponHold(pressedWeaponID);
        }

        if (interacting)
        {
            interactableShower.HoldInteract();

        }



    }

    private void FixedUpdate()
    {
        playerMovement.UpdateMovement(currentLookVector, movementVector);
    }
}
