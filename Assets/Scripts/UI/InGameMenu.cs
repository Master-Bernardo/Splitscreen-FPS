using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameManager gameManager;

    

    public void EnableInGameMenu()
    {
        menuPanel.SetActive(true);
    }

    public void DisableInGameMenu()
    {
        menuPanel.SetActive(false);
    }

    public void OnGoBackToMainMenu()
    {
        gameManager.GoBackToMainMenu();
    }

    public void OnResumeGame()
    {
        gameManager.ResumeGame();
    }

   
}
