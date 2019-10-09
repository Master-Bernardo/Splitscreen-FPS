using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRessources : MonoBehaviour
{
    public float[] playerPoints;
    public PointsUI[] pointsUI;
    public int startingScore;

    public static PlayerRessources Instance;

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

    void Start()
    {
        playerPoints = new float[4];

        for (int i = 0; i < playerPoints.Length; i++)
        {
            playerPoints[i] = startingScore;
            pointsUI[i].UpdatePlayerScore((int)playerPoints[i]);
        }
    }

    public void AddPlayerPoints(int playerID, float points)
    {
        playerPoints[playerID] += points;
        pointsUI[playerID].UpdatePlayerScore((int)playerPoints[playerID]);
    }

    public void RemovePlayerPoints(int playerID, float points)
    {
        playerPoints[playerID] -= points;
        pointsUI[playerID].UpdatePlayerScore((int)playerPoints[playerID]);
    }

    public bool DoesPlayerHaveEnoughPoints(int playerID, float points)
    {

        if (playerPoints[playerID] >= points)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
