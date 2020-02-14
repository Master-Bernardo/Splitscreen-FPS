using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    bool active = false;
    public GameObject menuPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!active)
            {
                EnableInGameMenu();
            }
            else
            {
                GoBackToGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (active)
            {
                GoBackToMainMenu();
            }
        }
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    void EnableInGameMenu()
    {
        active = true;
        Time.timeScale = 0;
        menuPanel.SetActive(true);
    }

    public void GoBackToGame()
    {
        Time.timeScale = 1;
        active = false;
        menuPanel.SetActive(false);
    }
}
