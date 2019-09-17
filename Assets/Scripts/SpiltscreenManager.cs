using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiltscreenManager : MonoBehaviour
{
    public Camera[] cameras;
    public GameObject[] playerContainers;
    public int playerNumber;

    public PlayerInput[] playerInputs;
    //public PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Awake()
    {
        //set up the cameras depending on the playernumber
        switch (playerNumber)
        {
            case 1:

                //enable disable the players
                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(false);
                playerContainers[2].SetActive(false);
                playerContainers[3].SetActive(false);

                cameras[0].rect = new Rect(0, 0, 1, 1);

                //playerInputManager.JoinPlayer(1,1, playerInputs[0].controlScheme, playerInputs[0].par)
                //playerInputManager.JoinPlayer(1);
                playerInputs[0].enabled = true;

                break;

            case 2:
                Debug.Log("2");
                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(false);
                playerContainers[3].SetActive(false);

                cameras[0].rect = new Rect(0, 0, 0.5f, 1);
                cameras[1].rect = new Rect(0.5f, 0, 0.5f, 1);

                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;

                // playerInputManager.JoinPlayer(1);
                // playerInputManager.JoinPlayer(2);



                break;

            case 3:

                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(true);
                playerContainers[3].SetActive(false);

                cameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);

                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;
                playerInputs[2].enabled = true;

                break;

            case 4:

                playerContainers[0].SetActive(true);
                playerContainers[1].SetActive(true);
                playerContainers[2].SetActive(true);
                playerContainers[3].SetActive(true);

                cameras[0].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cameras[1].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cameras[2].rect = new Rect(0, 0, 0.5f, 0.5f);
                cameras[3].rect = new Rect(0.5f, 0, 0.5f, 0.5f);

                playerInputs[0].enabled = true;
                playerInputs[1].enabled = true;
                playerInputs[2].enabled = true;
                playerInputs[3].enabled = true;

                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
