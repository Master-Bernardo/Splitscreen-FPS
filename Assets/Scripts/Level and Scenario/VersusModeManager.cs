using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used for classical team deathmatch or football
public class VersusModeManager : MonoBehaviour
{
    /*public enum VersusMode
    {
        Football,
        TeamDeathmatch
    }*/

    //public VersusMode mode;

    enum VersusState
    {
        Default,
        Pause,
        End
    }

    [Tooltip("use this for football mode")]
    public bool respawnAfterPoint;
    public GameObject ball;
    public Transform ballSpawnPoint;

    public int teamAPoints;
    public int teamBPoints;
    public int pointsNeededToWin;

    public float respawnTime;

    public VersusModeUI[] versusUI;
    public PlayerController[] playerControllers;
    List<GameEntity> playerEntities = new List<GameEntity>();
    bool[] playerStatus;
    float[] nextRespawnTime;
    List<PlayerController> activePlayerControllers = new List<PlayerController>();

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePointsUI();

        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].isActiveAndEnabled)
            {
                activePlayerControllers.Add(playerControllers[i]);
                GameEntity entity = playerControllers[i].playerEntity;
                playerEntities.Add(entity);
                entity.onDieEvent.AddListener(delegate { OnPlayerDies(entity); });
            }
            Debug.Log("add");
        }

        playerStatus = new bool[activePlayerControllers.Count];
        
        nextRespawnTime = new float[activePlayerControllers.Count];

        RespawnPlayers();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            AddPoints(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            AddPoints(1, 1);

        }

        for (int i = 0; i < playerStatus.Length; i++)
        {
            //if player dead?))
            if (!playerStatus[i])
            {
                if (Time.time > nextRespawnTime[i])
                {
                    RespawnPlayer(i);
                }
            }
        }
    }

    public void AddPoints(int team, int number) 
    {
        if (team == 0)
        {
            teamAPoints += number;
            UpdatePointsUI();
            if (teamAPoints >= pointsNeededToWin)
            {
                Win(0);
            }
        }
        else
        {
            teamBPoints += number;
            UpdatePointsUI();
            if (teamBPoints >= pointsNeededToWin)
            {
                Win(1);
            }
        }

        if (respawnAfterPoint)
        {
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            ball.transform.position = ballSpawnPoint.position;
            RespawnPlayers();
        }
    }

    void UpdatePointsUI()
    {
        for (int i = 0; i < versusUI.Length; i++)
        {
            versusUI[i].UpdateScore(teamAPoints, teamBPoints);
        }
    }

    void Win(int team)
    {
        for (int i = 0; i < versusUI.Length; i++)
        {
            versusUI[i].Win(team);
        }
    }

    public void OnPlayerDies(GameEntity player)
    {
        for (int i = 0; i < playerEntities.Count; i++)
        {
            if(player == playerEntities[i])
            {
                playerStatus[i] = false;
                nextRespawnTime[i] = Time.time + respawnTime;
            }
        }

        //point
        if(player.teamID == 0)
        {
            AddPoints(1, 1);
        }
        else
        {
            AddPoints(0, 1);
        }
    }

    public void RespawnPlayers()
    {
        Debug.Log("Respawn");
        foreach (PlayerController player in activePlayerControllers)
        {
            player.TeleportPlayer(spawnPoints[player.playerEntity.teamID].position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
            Debug.Log("spawn: " + player.playerEntity.teamID);
            player.ActivatePlayer();
        }

        //log all as alive
        for (int i = 0; i < playerStatus.Length; i++)
        {
            playerStatus[i] = true;
        }
    }

    public void RespawnPlayer(int playerID)
    {
        PlayerController player = activePlayerControllers[playerID];
        player.TeleportPlayer(spawnPoints[player.playerEntity.teamID].position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)));
        player.ActivatePlayer();
        Debug.Log("respan");
        playerStatus[playerID] = true;

    }
}
