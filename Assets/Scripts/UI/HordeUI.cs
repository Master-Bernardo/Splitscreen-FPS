using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordeUI : MonoBehaviour
{
    public GameObject pausePanel;
    public Text pauseTimeLeft;
    public GameObject wavePanel;
    public Text waveEnemiesLeft;
    public Text waveNumberUI;
    public Text playerPoints;


    void Start()
    {

    }

    void Update()
    {
        
    }

    public void UpdatePlayerScore(int playerScore)
    {
        playerPoints.text = playerScore.ToString();
    }

    public void UpdatePauseTimeLeft(int pauseTimeLeft)
    {
        this.pauseTimeLeft.text = pauseTimeLeft.ToString(); ;
    }

    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        waveEnemiesLeft.text = enemiesLeft.ToString();
    }

    public void StartPause()
    {
        pausePanel.SetActive(true);
        wavePanel.SetActive(false);
    }

    public void StartWave(int waveNumber)
    {
        pausePanel.SetActive(false);
        wavePanel.SetActive(true);
        waveNumberUI.text = waveNumber.ToString();
    }
}
