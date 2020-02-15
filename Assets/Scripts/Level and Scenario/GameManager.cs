using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Default,
    Paused
}

//switched between paused and default state, also takes care of cursor
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public InGameMenu inGameMenu;

    public static GameManager Instance;

    public PlayerController[] playerControllers;

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Default)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        inGameMenu.EnableInGameMenu();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].BlockAllPlayerInput();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        inGameMenu.DisableInGameMenu();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < playerControllers.Length; i++)
        {
            playerControllers[i].UnBlockAllPlayerInput();
        }
    }
}
