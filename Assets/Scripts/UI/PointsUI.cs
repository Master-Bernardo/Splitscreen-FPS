using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    public TextMeshProUGUI playerPoints;

    public void UpdatePlayerScore(int playerScore)
    {
        playerPoints.text = playerScore.ToString();
    }
}
