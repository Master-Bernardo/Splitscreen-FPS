using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HordeUI : MonoBehaviour
{
    public GameObject pausePanel;
    public TextMeshProUGUI pauseTimeLeft;
    public GameObject wavePanel;
    public TextMeshProUGUI waveEnemiesLeft;
    public TextMeshProUGUI waveNumberUI;
    public TextMeshProUGUI playerPoints;
    public GameObject winPanel;
    public GameObject defeatPanel;


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

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void ShowDefeatPanel()
    {
        defeatPanel.SetActive(true);
    }
}
