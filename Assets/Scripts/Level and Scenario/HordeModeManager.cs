using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeModeManager : MonoBehaviour
{
    public static HordeModeManager Instance;
    List<HordeSpawner> spawners = new List<HordeSpawner>();

    public GameEntity[] players;
    public HordeUI[] hordeUI;
    public float[] playerPoints;
    

    List<GameEntity> activePlayers = new List<GameEntity>();

    HashSet<GameEntity> currentWaveEnemies = new HashSet<GameEntity>();

    public HordeScenario hordeScenario;

    public float currentWaveRecruitmentPoints; // more expensive units will cost more - random
    public float unitsMultiplier; //how many units? general hardness
    public float waveRiser; //the units number gets bigger by this amount every wave
    public float pauseTime;
    public float prepTime;

    float nextWaveTime;
    int waveNumber = 0;

    public int startingScore;
    public float pointsForFirstWave = 10;
    float pointsForLastWave = 0;
    public float pointsMultiplier = 1.5f;

    enum HordeModeState
    {
        Pause,
        Wave
    }

    HordeModeState state;

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
        playerPoints = new float[4];

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActiveAndEnabled) activePlayers.Add(players[i]);
            hordeUI[i].UpdatePlayerScore(startingScore);
        }
        //Debug.Log("debug.log( count): " + activePlayers.Count);
        
        nextWaveTime = prepTime;
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
            foreach(HordeSpawner spawner in spawners)
            {
                //spawner.Spawn(5);
            }
        }

        switch (state)
        {
            case HordeModeState.Pause:

                if (Time.time > nextWaveTime)
                {
                    waveNumber++;

                    state = HordeModeState.Wave;
                    for (int i = 0; i < hordeUI.Length; i++)
                    {
                        hordeUI[i].StartWave(waveNumber);
                    }
                    //spawn enemies:
                   

                    SpawnWave();

                    currentWaveRecruitmentPoints = currentWaveRecruitmentPoints * (1 + waveRiser*unitsMultiplier);

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
                    state = HordeModeState.Pause;
                    nextWaveTime = Time.time + pauseTime;

                    for (int i = 0; i < hordeUI.Length; i++)
                    {
                        hordeUI[i].StartPause();
                        playerPoints[i] += pointsForFirstWave * Mathf.Pow(pointsMultiplier, waveNumber-1);
                        hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
                        //Debug.Log("add: " + pointsForFirstWave * Mathf.Pow(pointsMultiplier, waveNumber-1));
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
        currentWaveEnemies.Remove(entity);
    }

    public GameEntity GetNearestPlayer(Vector3 position)
    {
        GameEntity nearestPlayer = null;
        float nearestDistance = Mathf.Infinity;

        foreach(GameEntity player in activePlayers)
        {
            float currentDistance = (player.transform.position - position).sqrMagnitude;
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;

    }

    void SpawnWave()
    {
        HordeWave currentWave = hordeScenario.waves[waveNumber-1];

        //first spawn the boss units according to their number
        foreach(HordeUnitBoss bossUnit in currentWave.bossUnits)
        {
            for (int i = 0; i < bossUnit.number; i++)
            {
                //spawn
                GameEntity entity = SelectRandomSpawner(bossUnit.size).Spawn(bossUnit.unitPrefab);
                currentWaveEnemies.Add(entity);
                entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
            }
        }

        //next spawn the hleper minions as long s there are points
        int spawnPointsLeft = (int)currentWaveRecruitmentPoints;
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
            GameEntity entity = SelectRandomSpawner(unitToSpawn.size).Spawn(unitToSpawn.unitPrefab);
            currentWaveEnemies.Add(entity);
            entity.onDieEvent.AddListener(delegate { OnEnemyFromThisWaveDies(entity); });
            spawnPointsLeft -= unitToSpawn.recruitmentCost;
        }
    }

    public void AddPlayerPoints(GameEntity damagerEntity, float points)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i] == damagerEntity)
            {
                playerPoints[i] += points;
            }
            hordeUI[i].UpdatePlayerScore((int)playerPoints[i]);
        }
    }

    public bool DoesPlayerHaveEnoughPoints(GameEntity player, float points)
    {
        for (int i = 0; i < players.Length; i++)
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

        return false;
    }


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

}
