using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Controls;

public class SpiltscreenManager : MonoBehaviour
{
    public Camera[] topDownCameras;
    public Camera[] firstPersonCameras;
    public Camera[] UICamerasTopDown;
    public Camera[] UICamerasFirstPerson;
    public GameObject[] playerContainers;
    public int playerNumber;
    public bool useMultipleMonitors;

    public PlayerInput[] playerInputs;
    //public PlayerInputManager playerInputManager;

    bool p1UsesKeyboard = true;

    // Start is called before the first frame update
    void Awake()
    {
        //Display.displays[1].Activate();
        //Display.displays[2].Activate();


        //set up the cameras depending on the playernumber
        switch (playerNumber)
        {
            case 1:

                //enable disable the players
                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(false);
                playerContainers[2].SetActive(false);
                playerContainers[3].SetActive(false);

                topDownCameras[0].rect = new Rect(0, 0, 1, 1);
                firstPersonCameras[0].rect = new Rect(0, 0, 1, 1);
                UICamerasTopDown[0].rect = new Rect(0, 0, 1, 1);
                UICamerasFirstPerson[0].rect = new Rect(0, 0, 1, 1);


                //playerInputManager.JoinPlayer(1,1, playerInputs[0].controlScheme, playerInputs[0].par)
                //playerInputManager.JoinPlayer(1);
                playerInputs[0].enabled = true;

                break;

            case 2:

                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(false);
                playerContainers[3].SetActive(false);


                topDownCameras[0].rect = new Rect(0, 0, 0.5f, 1);
                firstPersonCameras[0].rect = new Rect(0, 0, 0.5f, 1);
                UICamerasTopDown[0].rect = new Rect(0, 0, 0.5f, 1);
                UICamerasFirstPerson[0].rect = new Rect(0, 0, 0.5f, 1);

                topDownCameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                firstPersonCameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                UICamerasTopDown[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                UICamerasFirstPerson[1].rect = new Rect(0.5f, 0, 0.5f, 1);


                topDownCameras[0].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[1].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();


                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;

                //if we have 2 monitors, we seperate them
                if (useMultipleMonitors)
                {
                    if (Display.displays.Length > 1)
                    {
                        Display.displays[1].Activate();
                        topDownCameras[1].targetDisplay = 1;
                        firstPersonCameras[1].targetDisplay = 1;
                        UICamerasTopDown[1].targetDisplay = 1;
                        UICamerasFirstPerson[1].targetDisplay = 1;

                        topDownCameras[0].rect = new Rect(0, 0, 1, 1);
                        firstPersonCameras[0].rect = new Rect(0, 0, 1, 1);
                        UICamerasTopDown[0].rect = new Rect(0, 0, 1, 1);
                        UICamerasFirstPerson[0].rect = new Rect(0, 0, 1, 1);

                        topDownCameras[1].rect = new Rect(0, 0, 1, 1);
                        firstPersonCameras[1].rect = new Rect(0, 0, 1, 1);
                        UICamerasTopDown[1].rect = new Rect(0, 0, 1, 1);
                        UICamerasFirstPerson[1].rect = new Rect(0, 0, 1, 1);

                    }
                }

                break;

            case 3:

                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(true);
                playerContainers[3].SetActive(false);

                topDownCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                firstPersonCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                UICamerasTopDown[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                UICamerasFirstPerson[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);

                topDownCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                firstPersonCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                UICamerasTopDown[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                UICamerasFirstPerson[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                topDownCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                firstPersonCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                UICamerasTopDown[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                UICamerasFirstPerson[2].rect = new Rect(0, 0, 0.5f, 0.5f);

                topDownCameras[0].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[1].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[2].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();



                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;
                playerInputs[2].enabled = true;

                //if we have 2 monitors, we seperate them
                if (useMultipleMonitors)
                {
                    if (Display.displays.Length > 1)
                    {
                        Display.displays[1].Activate();
                        topDownCameras[1].targetDisplay = 1;
                        firstPersonCameras[1].targetDisplay = 1;
                        topDownCameras[2].targetDisplay = 1;
                        firstPersonCameras[2].targetDisplay = 1;
                        UICamerasTopDown[1].targetDisplay = 1;
                        UICamerasFirstPerson[1].targetDisplay = 1;
                        UICamerasTopDown[2].targetDisplay = 1;
                        UICamerasFirstPerson[2].targetDisplay = 1;

                        topDownCameras[0].rect = new Rect(0, 0, 1, 1);
                        firstPersonCameras[0].rect = new Rect(0, 0, 1, 1);
                        UICamerasTopDown[0].rect = new Rect(0, 0, 1, 1);
                        UICamerasFirstPerson[0].rect = new Rect(0, 0, 1, 1);

                        topDownCameras[1].rect = new Rect(0, 0, 0.5f, 1);
                        firstPersonCameras[1].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasTopDown[1].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasFirstPerson[1].rect = new Rect(0, 0, 0.5f, 1);

                        topDownCameras[2].rect = new Rect(0.5f, 0, 0.5f, 1);
                        firstPersonCameras[2].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasTopDown[2].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasFirstPerson[2].rect = new Rect(0.5f, 0, 0.5f, 1);


                        topDownCameras[1].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                        topDownCameras[2].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                    }
                }

                break;

            case 4:

                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(true);
                playerContainers[3].SetActive(true);

                topDownCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                firstPersonCameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                UICamerasTopDown[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                UICamerasFirstPerson[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);

                topDownCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                firstPersonCameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                UICamerasTopDown[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                UICamerasFirstPerson[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                topDownCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                firstPersonCameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                UICamerasTopDown[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                UICamerasFirstPerson[2].rect = new Rect(0, 0, 0.5f, 0.5f);

                topDownCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                firstPersonCameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                UICamerasTopDown[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                UICamerasFirstPerson[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);

                topDownCameras[0].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[1].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[2].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                topDownCameras[3].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();


                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;
                playerInputs[2].enabled = true;
                playerInputs[3].enabled = true;

                //if we have 2 monitors, we seperate them
                if (useMultipleMonitors)
                {
                    if (Display.displays.Length > 1)
                    {
                        Display.displays[1].Activate();
                        topDownCameras[2].targetDisplay = 1;
                        firstPersonCameras[2].targetDisplay = 1;
                        UICamerasTopDown[2].targetDisplay = 1;
                        UICamerasFirstPerson[2].targetDisplay = 1;
                        topDownCameras[3].targetDisplay = 1;
                        firstPersonCameras[3].targetDisplay = 1;
                        UICamerasTopDown[3].targetDisplay = 1;
                        UICamerasFirstPerson[3].targetDisplay = 1;

                        topDownCameras[0].rect = new Rect(0, 0, 0.5f, 1);
                        firstPersonCameras[0].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasTopDown[0].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasFirstPerson[0].rect = new Rect(0, 0, 0.5f, 1);

                        topDownCameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                        firstPersonCameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasTopDown[1].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasFirstPerson[1].rect = new Rect(0.5f, 0, 0.5f, 1);

                        topDownCameras[2].rect = new Rect(0, 0, 0.5f, 1);
                        firstPersonCameras[2].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasTopDown[2].rect = new Rect(0, 0, 0.5f, 1);
                        UICamerasFirstPerson[2].rect = new Rect(0, 0, 0.5f, 1);

                        topDownCameras[3].rect = new Rect(0.5f, 0, 0.5f, 1);
                        firstPersonCameras[3].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasTopDown[3].rect = new Rect(0.5f, 0, 0.5f, 1);
                        UICamerasFirstPerson[3].rect = new Rect(0.5f, 0, 0.5f, 1);

                        topDownCameras[0].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                        topDownCameras[1].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                        topDownCameras[2].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                        topDownCameras[3].GetComponent<SmoothCameraFollow>().SetUpForSplitscreen();
                    }
                }

                break;
        }
        
    }

    private void Update()
    {
        if (p1UsesKeyboard)
        {
            //check if there is a gamepad free, which we could use

            bool isThereAFreeGamepadLeft = true;
            foreach (InputDevice device in playerContainers[playerNumber - 1].GetComponent<PlayerInput>().user.pairedDevices)
            {
                //Debug.Log("device");
                if (device == InputSystem.devices[InputSystem.devices.Count - 1]) isThereAFreeGamepadLeft = false; 
            }
            if(playerContainers[playerNumber - 1].GetComponent<PlayerInput>().user.pairedDevices.Count == 0)
            {
                isThereAFreeGamepadLeft = false;
            }

            if (isThereAFreeGamepadLeft)
            {
                //check if a button was pressed on the last, free controller
                foreach (InputControl control in InputSystem.devices[InputSystem.devices.Count - 1].allControls)
                {
                    if (control is ButtonControl)
                    {
                        if ((control as ButtonControl).wasPressedThisFrame)
                        {
                            Debug.Log("player 1 changed to gamepad");
                            InputUser user = playerContainers[0].GetComponent<PlayerInput>().user;
                            InputUser.PerformPairingWithDevice(InputSystem.devices[InputSystem.devices.Count - 1], user);
                            //InputUser.PerformPairingWithDevice(InputSystem.devices[1], user);
                            user.ActivateControlScheme("Gamepad");
                            p1UsesKeyboard = false;
                        }
                    }

                }
            }



        }
        else
        {
            //if we are currently using this gamepad, we chekc if we pressed something on the keyboard or mouse

            bool keyOnKeyboardOrMouseWerePressed = false;

            foreach (InputControl control in InputSystem.devices[0].allControls)
            {
                if (control is ButtonControl)
                {
                    if ((control as ButtonControl).wasPressedThisFrame)
                    {
                        keyOnKeyboardOrMouseWerePressed = true;
                    }
                }
            }


            foreach (InputControl control in InputSystem.devices[1].allControls)
            {
                if (control is ButtonControl)
                {
                    if ((control as ButtonControl).wasPressedThisFrame)
                    {
                        keyOnKeyboardOrMouseWerePressed = true;
                    }
                }
            }

            if (keyOnKeyboardOrMouseWerePressed)
            {
                Debug.Log("player 1 changed to keyboard");
                InputUser user = playerContainers[0].GetComponent<PlayerInput>().user;
                InputUser.PerformPairingWithDevice(InputSystem.devices[0], user);
                InputUser.PerformPairingWithDevice(InputSystem.devices[1], user);
                user.ActivateControlScheme("Keyboard and Mouse");
                p1UsesKeyboard = true;
            }
        }




           

        //Gamepad.current.aButton
      




        /*if (Input.GetKeyDown(KeyCode.C))
        {
            if (!p1UsesKeyboard)
            {
                Debug.Log("player 1 changed to keyboard");
                InputUser user = playerContainers[0].GetComponent<PlayerInput>().user;
                InputUser.PerformPairingWithDevice(InputSystem.devices[0], user);
                InputUser.PerformPairingWithDevice(InputSystem.devices[1], user);
                user.ActivateControlScheme("Keyboard and Mouse");
                p1UsesKeyboard = true;
            }
            else
            {
                bool isThereAFreeGamepadLeft = true;
                foreach (InputDevice device in playerContainers[playerNumber - 1].GetComponent<PlayerInput>().user.pairedDevices)
                {
                    Debug.Log("device");
                    if (device == InputSystem.devices[InputSystem.devices.Count - 1]) isThereAFreeGamepadLeft = false; ;
                }


                if (isThereAFreeGamepadLeft)
                {
                    //if(InputSystem.devices[InputSystem.devices.Count])
                    Debug.Log("player 1 changed to gamepad");
                    InputUser user = playerContainers[0].GetComponent<PlayerInput>().user;
                    InputUser.PerformPairingWithDevice(InputSystem.devices[InputSystem.devices.Count-1], user);
                    //InputUser.PerformPairingWithDevice(InputSystem.devices[1], user);
                    user.ActivateControlScheme("Gamepad");
                    p1UsesKeyboard = false;
                }
         
               
            }
        }*/
           
    }



}
