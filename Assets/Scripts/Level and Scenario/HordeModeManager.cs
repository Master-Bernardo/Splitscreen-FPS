using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeModeManager : MonoBehaviour
{
    public static HordeModeManager Instance;
    List<HordeSpawner> spawners = new List<HordeSpawner>();

    //public GameEntity[] players;
    public PlayerController[] playerControllers;
    public HordeUI[] hordeUI;
    // public float[] playerPoints;
    public PlayerRessources playerRessources;

    //List<GameEntity> activePlayers = new List<GameEntity>();
    List<PlayerController> activePlayerControllers = new List<PlayerController>();
    List<PlayerController> playersAlive = new List<PlayerController>();
    List<PlayerController> playersDead = new List<PlayerController>();

    public Transform playerSpawnPoint;

    HashSet<GameEntity> currentWaveEnemies = new HashSet<GameEntity>();

    public HordeScenario hordeScenario;
    public HordeEvents events;

    public float currentWaveRecruitmentPoints; // more expensive units will cost more - random
    public float unitsMultiplier; //how many units? general hardness
    public float waveRiser; //the units number gets bigger by this amount every wave
    public float pauseTime;
    public float prepTime;

    float nextWaveTime;
    int currentWaveNumber = 0;

   // public int startingScore;
    public float pointsForFirstWave = 10;
    float pointsForLastWave = 0;
    public float pointsMultiplier = 1.5f;

    enum HordeModeState
    {
        Pause,
        Wave,
        Victory,
        Defeat
    }

    HordeModeState state;

    [Header("Sound")]
    public AudioClip waveStartSound;
    public AudioClip waveEndSound;
    public AudioClip playerDiesSound;
    public AudioClip defeatSound;
    public AudioClip victorySound;

    public AudioSource audioSource;

    public AudioClip waveMusic;
    public AudioClip pauseMusic;

    public MusicManager musicManager;

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

        //set all the global settings
        unitsMultiplier = GlobalSettings.hordeModeHardness;
    }

    private void Start()
    {
        //playerPoints = new float[4];

        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].isActiveAndEnabled) activePlayerControllers.Add(playerControllers[i]);
            //playerPoints[i] = startingScore;
            //hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
        }

        for (int i = 0; i < activePlayerControllers.Count; i++)
        {
            playersAlive.Add(activePlayerControllers[i]);
            hordeUI[i].StartPause();
        }

        //Debug.Log("debug.log( count): " + activePlayers.Count);

        nextWaveTime = prepTime;

        //sound
        musicManager.PlayMusic(pauseMusic, true, 2);
    }

    public void AddSpawner(HordeSpawner spawner)
    {
        spawners.Add(spawner);
    }

    public void RemoveSpawner(HordeSpawner spawner)
    {
        spawners.Remove(spawner);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // RespawnPlayers();
            //AddPlayerPoints(activePlayerControllers[1].playerEntity, 200);


            HashSet<GameEntity> enemiesToKill = new HashSet<GameEntity>(currentWaveEnemies);
            foreach(GameEntity enemy in enemiesToKill)
            {
                enemy.Die(null);
            }
            //wave won
            RespawnPlayers();
            state = HordeModeState.Pause;
            nextWaveTime = Time.time + pauseTime;

            for (int i = 0; i < hordeUI.Length; i++)
            {
                hordeUI[i].StartPause();
                playerRessources.AddPlayerPoints(i, pointsForFirstWave * Mathf.Pow(pointsMultiplier, currentWaveNumber - 1));
                // hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
                //Debug.Log("add: " + pointsForFirstWave * Mathf.Pow(pointsMultiplier, waveNumber-1));
            }

            //sound 
            musicManager.StopMusic();
            //audioSource.loop = false;
            audioSource.clip = waveEndSound;
            audioSource.Play();
            musicManager.PlayMusic(pauseMusic, true, 4);
        }



        switch (state)
        {
            case HordeModeState.Pause:

                if (Time.time > nextWaveTime)
                {
                    currentWaveNumber++;

                    state = HordeModeState.Wave;
                    for (int i = 0; i < hordeUI.Length; i++)
                    {
                        hordeUI[i].StartWave(currentWaveNumber);
                    }
                    //spawn enemies:
                   

                    SpawnWave();

                    currentWaveRecruitmentPoints = currentWaveRecruitmentPoints * (1 + waveRiser*unitsMultiplier);

                    //sound wave start
                    musicManager.StopMusic();
                    //audioSource.loop = false;
                    audioSource.clip = waveStartSound;
                    audioSource.Play();
                    musicManager.PlayMusic(waveMusic, true, 2);

                }
                else
                {
                    for (int i = 0; i < hordeUI.Length; i++)
                    {
                        hordeUI[i].UpdatePauseTimeLeft((int)(nextWaveTime - Time.time));
                    }
                }

                break;

            case HordeModeState.Wave:

                if (currentWaveEnemies.Count == 0)
                {
                    if (currentWaveNumber == hordeScenario.waves.Length)
                    {
                        Victory();
                    }
                    else
                    {
                        //wave won
                        RespawnPlayers();
                        state = HordeModeState.Pause;
                        nextWaveTime = Time.time + pauseTime;

                        for (int i = 0; i < hordeUI.Length; i++)
                        {
                            hordeUI[i].StartPause();
                            playerRessources.AddPlayerPoints(i,pointsForFirstWave * Mathf.Pow(pointsMultiplier, currentWaveNumber - 1));
                           // hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
                            //Debug.Log("add: " + pointsForFirstWave * Mathf.Pow(pointsMultiplier, waveNumber-1));
                        }

                        //sound 
                        musicManager.StopMusic();
                        //audioSource.loop = false;
                        audioSource.clip = waveEndSound;
                        audioSource.Play();
                        musicManager.PlayMusic(pauseMusic, true, 4);
                    }


                }
                else
                {
                    //waveEnemiesLeft.text = currentWaveEnemies.Count.ToString();
                    //waveNumberUI.text = waveNumber.ToString();
                    for (int i = 0; i < hordeUI.Length; i++)
                    {
                        hordeUI[i].UpdateEnemiesLeft(currentWaveEnemies.Count);
                    }
                }
                break;
        }
    }

    public void OnEnemyFromThisWaveDies(GameEntity entity)
    {

        for (int i = 0; i < hordeUI.Length; i++)
        {
            hordeUI[i].UpdateEnemiesLeft(currentWaveEnemies.Count);
        }
            currentWaveEnemies.Remove(entity);
    }

    public GameEntity GetNearestPlayer(Vector3 position)
    {
        GameEntity nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;

        foreach(PlayerController player in playersAlive)
        {
            float currentDistance = (player.playerEntity.transform.position - position).sqrMagnitude;
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestPlayer = player.playerEntity;
            }
        }

        return nearestPlayer;

    }

    void SpawnWave()
    {
        HordeWave currentWave = hordeScenario.waves[currentWaveNumber-1];
        Debug.Log("--------------------------------current wave: " + currentWave + "------------------------------------------");
        //if this wave has events - call them:
        events.disableEnableEvents[currentWaveNumber - 1].CallEvent();


        //first spawn the boss units according to their number
        foreach (HordeUnitBoss bossUnit in currentWave.bossUnits)
        {
            if (!bossUnit.fixedNumber) bossUnit.number = (int)(bossUnit.number*unitsMultiplier);

            for (int i = 0; i < bossUnit.number; i++)
            {
                //spawn
                GameEntity entity = SelectRandomSpawner(bossUnit.size).Spawn(bossUnit.unitPrefab);
                currentWaveEnemies.Add(entity);
                entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
            }
        }

        //next spawn the hleper minions as long s there are points
        int spawnPointsLeft = (int)(currentWaveRecruitmentPoints * currentWave.unitsPointsMultiplier);
        List<HordeUnit> unitsWeCanAfford = new List<HordeUnit>();

        while (spawnPointsLeft > 0)
        {
            //1make a new List of units we can aford with the remaining points,
            unitsWeCanAfford.Clear();

            foreach (HordeUnit unit in currentWave.units)
            {
                if (unit.recruitmentCost <= spawnPointsLeft) unitsWeCanAfford.Add(unit);
            }

            //if theres no unit we can afford, break out of the look 
            if (unitsWeCanAfford.Count == 0)
            {
                spawnPointsLeft = 0;
                return;
            }

            //2.  choose on of them randomly
            HordeUnit unitToSpawn = unitsWeCanAfford[Random.Range(0, unitsWeCanAfford.Count)];
            //Debug.Log(unitToSpawn + " .unitToSpawn");
            GameEntity entity = SelectRandomSpawner(unitToSpawn.size).Spawn(unitToSpawn.unitPrefab);
            //Debug.Log(entity + " .entity");

            currentWaveEnemies.Add(entity);
            entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
            spawnPointsLeft -= unitToSpawn.recruitmentCost;
        }
    }

    //allows us to add enemies to the current wave from elsewhere - level events or special bosses spawning only in one area
    public void RegisterEnemyIntoWave(GameEntity enemy)
    {
        currentWaveEnemies.Add(enemy);
        enemy.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(enemy); });
    }

   /* public void AddPlayerPoints(GameEntity damagerEntity, float points)
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if(playerControllers[i].playerEntity == damagerEntity)
            {
                playerPoints[i] += points;
            }
            hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
        }
    }

    public void RemovePlayerPoints(GameEntity playerEntity, float points)
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].playerEntity == playerEntity)
            {
                playerPoints[i] -= points;
            }
            hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
        }
    }

    public bool DoesPlayerHaveEnoughPoints(GameEntity player, float points)
    {
        for (int i = 0; i < playerControllers.Length; i++)
        {
            if(playerControllers[i].playerEntity == player)
            {
                if (playerPoints[i] >= points)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        return false;
    }*/


    HordeSpawner SelectRandomSpawner(int unitSize)
    {
        List<HordeSpawner> possibleSpawners = new List<HordeSpawner>();

        foreach(HordeSpawner spawner in spawners)
        {
            if(spawner.size <= unitSize)
            {
                possibleSpawners.Add(spawner);
            }
        }

        if (possibleSpawners.Count > 0)
        {
            return possibleSpawners[Random.Range(0, possibleSpawners.Count)];
        }
        else
        {
            return null;
        }
    }

    #region playerSpawn and Death

    public void OnPlayerDies(PlayerController player)
    {
        playersAlive.Remove(player);
        playersDead.Add(player);
        if(playersAlive.Count == 0)
        {
            Defeat();
        }

        //sound
        audioSource.clip = playerDiesSound;
        audioSource.Play();
    }

    public void RespawnPlayers()
    {
        //determine respawn position - just randomly among live players?
        Vector3 spawnPoint = playersAlive[Random.Range(0,playersAlive.Count)].playerEntity.transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        

        Debug.Log("Respawn");
        foreach(PlayerController player in playersDead)
        {
            //player.TeleportPlayer(playerSpawnPoint.position + new Vector3(Random.Range(-1,1),0, Random.Range(-1, 1)));
            player.TeleportPlayer(spawnPoint);
            playersAlive.Add(player);
            player.ActivatePlayer();
        }
        playersDead.Clear();

    }

    void Victory()
    {
        state = HordeModeState.Victory;
        for (int i = 0; i < hordeUI.Length; i++)
        {
            hordeUI[i].ShowWinPanel();
        }

        //sound
        musicManager.StopMusic();
        audioSource.clip = victorySound;
        audioSource.Play();

    }

    void Defeat()
    {
        state = HordeModeState.Defeat;
        for (int i = 0; i < hordeUI.Length; i++)
        {
            hordeUI[i].ShowDefeatPanel();
        }

        //sound
        musicManager.StopMusic();
        audioSource.clip = defeatSound;
        audioSource.Play();
    }

    #endregion

}
