using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    #region Fields
    public Camera topdownCam;
    public Camera topDownUICam;
    public Camera fpCam;
    public Camera fpUICam;
    public Canvas playerUICanvas;
    public Canvas aimingCross;

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
    public Transform rtsGhostTransform;
    public PlayerMovementRTS deadPlayerMovement;
    public PlayerInput playerInput;
    public EC_PlayerWeaponSystem weaponSystem;
    public InteractableShower interactableShower;

    public Vector3 desiredLookVektor;
    Vector3 movementVector;

    Vector3 movementInputVector;
    Vector2 lookInputVector; // for controller
    Vector2 lookInputVectorUsed;
    //Vector2 lookInputVectorLastFrame; //for better controls
    Vector2 currentMousePosition; //for keyboard & mouse
    Vector2 mouseDelta;
    //Vector2 mousePositionLastFrame;

    bool weaponKey1Pressed = false;
    bool weaponKey2Pressed = false;
    int pressedWeaponID;

    bool interacting = false;
    public bool looseWeaponOnDeath = true;

    public float lookAroundSensitivityMultiplayer = 1;
    public float xSensitivity = 0.3f;
    public float ySensitivity = 0.1f;

    public AimVisualiser aimVisualiser;
    public PlayerAudioListener playerAudioListener;

    #endregion

    #region Controls

    public void OnMovement(InputValue value)
    {
        movementInputVector = value.Get<Vector2>();
    }

    public void OnW1Press()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            pressedWeaponID = 0;
            weaponKey1Pressed = true;
            weaponSystem.UseWeaponStart(pressedWeaponID);
        }            
    }

    public void OnW1Release()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            pressedWeaponID = 0;
            weaponKey1Pressed = false;
            weaponSystem.UseWeaponEnd(pressedWeaponID);
        }
    }

    public void OnW2Press()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            pressedWeaponID = 1;
            weaponKey2Pressed = true;
            weaponSystem.UseWeaponStart(pressedWeaponID);
        }
    }

    public void OnW2Release()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            pressedWeaponID = 1;
            weaponKey2Pressed = false;
            weaponSystem.UseWeaponEnd(pressedWeaponID);
        }
    }

    public void OnReload()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            weaponSystem.ReloadWeapon();
        }
    }

    public void OnInteractStart()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            interacting = true;
            interactableShower.StartInteract();
        }
    }

    public void OnInteractStop()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            interacting = false;
            interactableShower.StopInteract();
        }
    }

    public void OnJump()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            playerMovement.Jump();
        }
    }

    //we dash in the direction our wasd or left stick is facing, if their diection is null, we dash in the current look direction
    public void OnDash()
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            if (movementVector != new Vector3(0, 0, 0))
            {
                playerMovement.Dash(movementVector.normalized);
                //Debug.Log("dash no move");
            }
            else
            {
                playerMovement.Dash(desiredLookVektor.normalized);
                //Debug.Log("dash MMove");
            }
        }
    }

    void OnRotateTowards(InputValue value)
    {
        if (controlMode != PlayerControlMode.RTS)
        {
            //Debug.Log("control sheme: " + playerInput.controlScheme);
            if (playerInput.controlScheme == "Gamepad")
            {
                lookInputVector = value.Get<Vector2>();
                //Debug.Log("gamepad");
                if (lookInputVector != new Vector2(0, 0))
                {
                    lookInputVectorUsed = lookInputVector;
                    //lookInputVector = lookInputVectorLastFrame;
                }
            }
            else
            {
                if (controlMode == PlayerControlMode.TopDown)
                {
                    currentMousePosition = value.Get<Vector2>();

                }

            }
        }
    }

    void OnNextWeapon(InputValue value)
    {
        if (controlMode != PlayerControlMode.RTS)
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
    }

    void OnPreviousWeapon(InputValue value)
    {
        if (controlMode != PlayerControlMode.RTS)
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
    }

    void OnSelectWeapon1()
    {
        if(controlMode != PlayerControlMode.RTS)  weaponSystem.ChangeWeapon(0);
    }

    void OnSelectWeapon2()
    {
        if (controlMode != PlayerControlMode.RTS) weaponSystem.ChangeWeapon(1);
    }

    void OnSelectWeapon3()
    {
        if (controlMode != PlayerControlMode.RTS) weaponSystem.ChangeWeapon(2);
    }

    void OnLookAroundFP(InputValue value)
    {
        mouseDelta = value.Get<Vector2>() * lookAroundSensitivityMultiplayer;
    }

    void OnToggleCamera()
    {
        if (controlMode != PlayerControlMode.RTS) ToogleCameraMode();
    }

    void OnRaiseLookAroundSensitivityMultiplier()
    {
        lookAroundSensitivityMultiplayer += 0.4f;
    }

    void OnLowerLookAroundSensitivityMultiplier()
    {
        lookAroundSensitivityMultiplayer -= 0.4f;
    }




    #endregion

    void Awake()
    {
        desiredLookVektor = playerEntity.transform.forward;

        SwitchToTopDownMode();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchToFPMode();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchToTopDownMode();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SwitchToRTSMode();
        }

        //get movement
        float hor = movementInputVector.x;
        float ver = movementInputVector.y;

        #region determine movementVector

        if (controlMode == PlayerControlMode.FirstPerson)
        {
            movementVector = playerEntity.transform.right*hor + playerEntity.transform.forward*ver; 
        }
        else
        {
            Vector3 camRight = (topdownCam.transform.right).normalized;
            Vector3 horV = camRight * hor;
            Vector3 verV = new Vector3(-camRight.z, 0f, camRight.x) * ver;
            movementVector = horV + verV;
        }       
        if (controlMode == PlayerControlMode.RTS)
        {
            deadPlayerMovement.UpdateMovement(movementVector);
        }

        #endregion

        #region determine desiredLookDirectionVector

        if (controlMode == PlayerControlMode.TopDown)
        {
            //rotate towards

            if (playerInput.controlScheme == "Gamepad")
            {
                if (lookInputVector != new Vector2(0, 0))
                {
                    desiredLookVektor = Quaternion.Euler(0, topdownCam.transform.localEulerAngles.y, 0) * new Vector3(lookInputVectorUsed.x, 0f, lookInputVectorUsed.y);
                }
                else
                {
                    if (movementVector != new Vector3(0, 0, 0))
                    {
                        desiredLookVektor = movementVector;
                    }
                }


            }
            else
            {
                Vector2 direction = new Vector2(Screen.width / 2, Screen.height / 2) - currentMousePosition;
                desiredLookVektor = Quaternion.Euler(0, topdownCam.transform.localEulerAngles.y + 180, 0) * new Vector3(direction.x, 0f, direction.y);
            }

           
        } 
        else if (controlMode == PlayerControlMode.FirstPerson)
        {
            if (playerInput.controlScheme == "Gamepad")
            {

                //TODO

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
                Vector3 desiredLookVektorHorizontal = (Quaternion.AngleAxis(mouseDelta.x * xSensitivity, playerEntity.transform.up) * playerEntity.transform.forward).normalized;
                Vector3 desiredLookVektorVertical = (Quaternion.AngleAxis(-mouseDelta.y * ySensitivity, fpCam.transform.right) * fpCam.transform.forward).normalized;
                desiredLookVektor = new Vector3(desiredLookVektorHorizontal.x, desiredLookVektorVertical.y, desiredLookVektorHorizontal.z);

            }
            else
            {
                Vector3 desiredLookVektorHorizontal = (Quaternion.AngleAxis(mouseDelta.x * xSensitivity, playerEntity.transform.up) * playerEntity.transform.forward).normalized;
                Vector3 desiredLookVektorVertical = (Quaternion.AngleAxis(-mouseDelta.y * ySensitivity, fpCam.transform.right) * fpCam.transform.forward).normalized;
                desiredLookVektor = new Vector3(desiredLookVektorHorizontal.x, desiredLookVektorVertical.y, desiredLookVektorHorizontal.z);
            }

            //first person mode gets rotation applied directly in update, not in fixedUpdate
            playerMovement.InstantRotateTo(desiredLookVektor);
        }

        #endregion

        //weapon
        if (weaponKey1Pressed)
        {
            pressedWeaponID = 0;
            weaponSystem.UseWeaponHold(pressedWeaponID);
        }
        if (weaponKey2Pressed)
        {
            pressedWeaponID = 1;
            weaponSystem.UseWeaponHold(pressedWeaponID);
        }

        if (interacting)
        {
            interactableShower.HoldInteract();
        }
    }

    private void FixedUpdate()
    {
        //top down gets player rotation applied only in fixedUpdate
        if (controlMode == PlayerControlMode.TopDown)
        {
            playerMovement.SmoothRotateTo(desiredLookVektor);
        }
        if(controlMode != PlayerControlMode.RTS)
        playerMovement.UpdateMovement(movementVector);
    }

    public void ActivatePlayer()
    {
        playerEntity.GetComponent<EC_Health>().ResetHealth();
        rtsGhostTransform.gameObject.SetActive(false);
        playerEntity.gameObject.SetActive(true);
        topdownCam.GetComponent<SmoothCameraFollow>().target = playerEntity.transform;

        if (looseWeaponOnDeath)
        {
            playerEntity.GetComponent<EC_PlayerWeaponSystem>().ResetWeaponsToStartingWeapons();
            playerEntity.GetComponent<EC_PlayerWeaponSystem>().SetUpWeaponsAndAmmo();
        }
        else
        {
            playerEntity.GetComponent<EC_PlayerWeaponSystem>().ResetWeaponsRespawn();
        }

        controlMode = PlayerControlMode.TopDown;

    }

    public void DeactivatePlayer()
    {
        playerEntity.gameObject.SetActive(false);
        playerEntity.GetComponent<EC_PlayerWeaponSystem>().SetCurrentWeaponToNull();
        SwitchToRTSMode();
    }

    public void ToogleCameraMode()
    {
        if (controlMode == PlayerControlMode.TopDown)
        {
            SwitchToFPMode();
        }
        else if (controlMode == PlayerControlMode.FirstPerson)
        {
            SwitchToTopDownMode();
        }
    }

    void SwitchToFPMode()
    {
        controlMode = PlayerControlMode.FirstPerson;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        topdownCam.gameObject.SetActive(false);
        fpCam.gameObject.SetActive(true);

        aimingCross.enabled = true;
        playerUICanvas.worldCamera = fpUICam;

        //aimVisualiser.ChangeVisualisationMode(AimVisualisationMode.FirstPerson);
        aimVisualiser.HideLineInstantly();
        if (playerAudioListener) playerAudioListener.SwitchToFpMode();
    }

    void SwitchToTopDownMode()
    {
        topdownCam.GetComponent<SmoothCameraFollow>().target = playerEntity.transform;
        controlMode = PlayerControlMode.TopDown;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        topdownCam.gameObject.SetActive(true);
        fpCam.gameObject.SetActive(false);

        aimingCross.enabled = false;
        playerUICanvas.worldCamera = topDownUICam;

        //aimVisualiser.ChangeVisualisationMode(AimVisualisationMode.TopDown);
        aimVisualiser.ShowLineInstantly();
        if(playerAudioListener) playerAudioListener.SwitchToTopDownMode();
    }

    void SwitchToRTSMode()
    {
        //TODO are some lines unnecessary?
        controlMode = PlayerControlMode.RTS;

        Vector3 rtsCamPosition = playerEntity.transform.position;
        rtsCamPosition.y = 0;
        rtsGhostTransform.position = rtsCamPosition;
        rtsGhostTransform.gameObject.SetActive(true);
        topdownCam.GetComponent<SmoothCameraFollow>().target = rtsGhostTransform;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        topdownCam.gameObject.SetActive(true);
        fpCam.gameObject.SetActive(false);

        aimingCross.enabled = false;
        playerUICanvas.worldCamera = topDownUICam;

        //aimVisualiser.ChangeVisualisationMode(AimVisualisationMode.TopDown);
        aimVisualiser.HideLineInstantly();
        if (playerAudioListener) playerAudioListener.SwitchToTopDownMode();

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

    public Camera GetActiveCamera()
    {
        if(controlMode == PlayerControlMode.FirstPerson)
        {
            return fpCam;
        }
        else
        {
            return topdownCam;
        }
    }
}
