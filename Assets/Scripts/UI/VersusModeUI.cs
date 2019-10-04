using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersusModeUI : MonoBehaviour
{
    public TextMeshProUGUI teamAScore;
    public TextMeshProUGUI teamBScore;

    public GameObject teamAWinPanel;
    public GameObject teamBWinPanel;

    public void UpdateScore(int aScore, int bScore)
    {
        teamAScore.text = aScore.ToString();
        teamBScore.text = bScore.ToString();
    }

    public void Win(int winner)
    {
        if(winner == 0)
        {
            teamAWinPanel.SetActive(true);
        }
        else
        {
            teamBWinPanel.SetActive(true);
        }
    }
}
