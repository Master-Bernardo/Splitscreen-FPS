using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    public Camera topdownCam;
    public Camera fpCam;

    enum PlayerControlMode
    {
        TopDown,
        FirstPerson,
        RTS //we can move around the world as a dead ghost - also RTS Mode
    }

    PlayerControlMode controlMode;

    public UnityEvent onDieEvent;


    public PlayerMovement playerMovement;
    public GameEntity playerEntity;
    public Transform deadPlayerGhostTransform;
    public PlayerMovementRTS deadPlayerMovement;
    public PlayerInput playerInput;
    public EC_PlayerWeaponSystem weaponSystem;
    public InteractableShower interactableShower;

    public Vector3 currentLookVector;
    Vector3 movementVector;

    Vector3 movementInputVector;
    Vector2 lookInputVector; // for controller
    Vector2 lookInputVectorUsed;
    //Vector2 lookInputVectorLastFrame; //for better controls
    Vector2 currentMousePosition; //for keyboard & mouse
    Vector2 mouseDelta;
    Vector2 mousePositionLastFrame;

    bool weaponPressed = false;
    int pressedWeaponID;

    bool interacting = false;
    public bool looseWeaponOnDeactivate = true;



    #region controls

    public void OnMovement(InputValue value)
    {
        movementInputVector = value.Get<Vector2>();
    }

    public void OnW1Press()
    {
        if(controlMode == PlayerControlMode.TopDown) weaponSystem.UseWeaponStart(0);
        pressedWeaponID = 0;
        weaponPressed = true;
        weaponSystem.UseWeaponStart(pressedWeaponID);
    }

    public void OnW1Release()
    {
        weaponPressed = false;
        weaponSystem.UseWeaponEnd(pressedWeaponID);
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

    public void OnJump()
    {
        playerMovement.Jump();
    }

    //we dash in the direction our wasd or left stick is facing, if their diection is null, we dash in the current look direction
    public void OnDash()
    {

        if (movementVector != new Vector3(0, 0, 0))
        {
            playerMovement.Dash(movementVector.normalized);
            //Debug.Log("dash no move");
        }
        else
        {
            playerMovement.Dash(currentLookVector.normalized);
            //Debug.Log("dash MMove");
        }
    }

    void OnRotateTowards(InputValue value)
    {
        //Debug.Log("control sheme: " + playerInput.controlScheme);
        if(playerInput.controlScheme == "Gamepad")
        {
            lookInputVector = value.Get<Vector2>();
            //Debug.Log("gamepad");
            if(lookInputVector != new Vector2(0,0))
            {
                lookInputVectorUsed = lookInputVector;
                //lookInputVector = lookInputVectorLastFrame;
            }
        }
        else
        {      
            if(controlMode == PlayerControlMode.TopDown)
            {
                currentMousePosition = value.Get<Vector2>();

            }

        }
    }


    void OnNextWeapon(InputValue value)
    {
        if (playerInput.controlScheme == "Gamepad")
        {
            weaponSystem.SelectNextWeapon();
        }
        else
        {
            if (value.Get<float>() > 0)
            {
                weaponSystem.SelectNextWeapon();
            }
        }
    }

    void OnPreviousWeapon(InputValue value)
    {
        if (playerInput.controlScheme == "Gamepad")
        {
            weaponSystem.SelectPreviousWeapon();
        }
        else
        {
            if (value.Get<float>() < 0)
            {
                weaponSystem.SelectPreviousWeapon();
            }
        }
    }

    void OnSelectWeapon1()
    {
        weaponSystem.ChangeWeapon(0);
    }

    void OnSelectWeapon2()
    {
        weaponSystem.ChangeWeapon(1);
    }

    void OnSelectWeapon3()
    {
        weaponSystem.ChangeWeapon(2);
    }

   
 

    #endregion


    void Start()
    {
        currentLookVector = playerEntity.transform.forward;
    }



    void Update()
    {
        //constant input checks:

        if(controlMode == PlayerControlMode.FirstPerson)
        {
            currentMousePosition = Mouse.current.position.ReadValue();
            mouseDelta = currentMousePosition - mousePositionLastFrame;
            Debug.Log("mosue Delta = " + mouseDelta);
            // Debug.Log("mousePositionLastFrame = " + mousePositionLastFrame);
            //Debug.Log("mouseDelta: " + mouseDelta);
            mousePositionLastFrame = currentMousePosition;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ToogleCameraMode();
        }

        //get movement
        float hor = movementInputVector.x;
        float ver = movementInputVector.y;

        if (controlMode == PlayerControlMode.FirstPerson)
        {
            movementVector = playerEntity.transform.right*hor + playerEntity.transform.forward*ver;

            if (playerInput.controlScheme == "Gamepad")
            {
                /*if (lookInputVector != new Vector2(0, 0))
                {
                    currentLookVector = Quaternion.Euler(0, topdownCam.transform.localEulerAngles.y, 0) * new Vector3(lookInputVectorUsed.x, 0f, lookInputVectorUsed.y);
                }
                else
                {
                    if (movementVector != new Vector3(0, 0, 0))
                    {
                        currentLookVector = movementVector;
                    }
                }*/


            }
            else
            {
                //Vector2 playerPos = new Vector3(cam.WorldToScreenPoint(transform.position).x, cam.WorldToScreenPoint(transform.position).y);
                //Vector2 direction = playerPos -  currentMousePosition;
                //Debug.Log("playerPos: " + playerPos);
                //Vector2 direction = new Vector2(Screen.width / 2, Screen.height / 2) - currentMousePosition;
                float horDirection = mouseDelta.y;
               // Debug.Log("mouse Y: " + mouseDelta.y);
               // Debug.Log("mouse X: " + mouseDelta.x);
                float verDirection = mouseDelta.x;

                currentLookVector = Quaternion.Euler(mouseDelta.y, mouseDelta.x, 0) * fpCam.transform.forward;
                Debug.Log("dorward: " + fpCam.transform.forward);
                Debug.Log("currentLookVector: " + currentLookVector);
            }




        }
        else
        {
            Vector3 camRight = (topdownCam.transform.right).normalized;
            Vector3 horV = camRight * hor;
            Vector3 verV = new Vector3(-camRight.z, 0f, camRight.x) * ver;
            movementVector = horV + verV;
        }

           

        
        if(controlMode == PlayerControlMode.TopDown)
        {
            //rotate towards

            if (playerInput.controlScheme == "Gamepad")
            {
                if (lookInputVector != new Vector2(0, 0))
                {
                    currentLookVector = Quaternion.Euler(0, topdownCam.transform.localEulerAngles.y, 0) * new Vector3(lookInputVectorUsed.x, 0f, lookInputVectorUsed.y);
                }
                else
                {
                    if (movementVector != new Vector3(0, 0, 0))
                    {
                        currentLookVector = movementVector;
                    }
                }


            }
            else
            {
                //Vector2 playerPos = new Vector3(cam.WorldToScreenPoint(transform.position).x, cam.WorldToScreenPoint(transform.position).y);
                //Vector2 direction = playerPos -  currentMousePosition;
                //Debug.Log("playerPos: " + playerPos);
                Vector2 direction = new Vector2(Screen.width / 2, Screen.height / 2) - currentMousePosition;
                currentLookVector = Quaternion.Euler(0, topdownCam.transform.localEulerAngles.y + 180, 0) * new Vector3(direction.x, 0f, direction.y);
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
        else if (controlMode == PlayerControlMode.RTS)
        {
            deadPlayerMovement.UpdateMovement(movementVector);
        }

    }

    private void FixedUpdate()
    {
        if (controlMode == PlayerControlMode.TopDown)
        {
            playerMovement.UpdateMovement(currentLookVector, movementVector);
        }
        else if(controlMode == PlayerControlMode.FirstPerson)
        {
            playerMovement.UpdateMovement(currentLookVector, movementVector);
        }
        
    }

    public void TeleportPlayer(Vector3 position)
    {
        playerEntity.transform.position = position;
        topdownCam.GetComponent<SmoothCameraFollow>().TeleportToDesiredPosition();

    }

    public void OnDie()
    {
        DeactivatePlayer();
        onDieEvent.Invoke();
    }

    public void ActivatePlayer()
    {
        playerEntity.GetComponent<EC_Health>().ResetHealth();
        deadPlayerGhostTransform.gameObject.SetActive(false);
        playerEntity.gameObject.SetActive(true);
        topdownCam.GetComponent<SmoothCameraFollow>().target = playerEntity.transform;

        controlMode = PlayerControlMode.TopDown;

    }

    public void DeactivatePlayer()
    {
        if (looseWeaponOnDeactivate) playerEntity.GetComponent<EC_PlayerWeaponSystem>().ResetWeapons();

        playerEntity.gameObject.SetActive(false);
        Vector3 rtsCamPosition = playerEntity.transform.position;
        rtsCamPosition.y = 0;
        deadPlayerGhostTransform.position = rtsCamPosition;
        deadPlayerGhostTransform.gameObject.SetActive(true);
        topdownCam.GetComponent<SmoothCameraFollow>().target = deadPlayerGhostTransform;


        controlMode = PlayerControlMode.RTS;

    }

    public void ToogleCameraMode()
    {
        if (controlMode == PlayerControlMode.TopDown)
        {
            controlMode = PlayerControlMode.FirstPerson;
            topdownCam.gameObject.SetActive(false);
            fpCam.gameObject.SetActive(true);
        }
        else if (controlMode == PlayerControlMode.FirstPerson)
        {
            controlMode = PlayerControlMode.TopDown;
            topdownCam.gameObject.SetActive(true);
            fpCam.gameObject.SetActive(false);
        }
    }
}
