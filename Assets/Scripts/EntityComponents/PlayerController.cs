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

    public void OnMovement(InputValue value)
    {
        movementInputVector = value.Get<Vector2>();
    }

    public void OnW1Press()
    {
        weaponSystem.UseWeaponStart(0);
        pressedWeaponID = 0;
        weaponPressed = true;
    }

    public void OnW1Release()
    {
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
            lookInputVector = value.Get<Vector2>();
            //Debug.Log("gamepad");
            if(lookInputVector == new Vector2(0,0))
            {
                lookInputVector = lookInputVectorLastFrame;
            }
        }
        else
        {
            currentMousePosition = value.Get<Vector2>();
            //Debug.Log("keyboard");
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

        //rotate towards

        if (playerInput.controlScheme == "Gamepad")
        {
            //Debug.Log("look InputVector: " + lookInputVector);
            currentLookVector = Quaternion.Euler(0, cam.transform.localEulerAngles.y, 0) * new Vector3(lookInputVector.x, 0f, lookInputVector.y);

            lookInputVectorLastFrame = lookInputVector;
        }
        else
        {
           Vector2 playerPos = new Vector3(cam.WorldToScreenPoint(transform.position).x, cam.WorldToScreenPoint(transform.position).y);
           Vector2 direction = playerPos -  currentMousePosition;
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
