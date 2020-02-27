using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersusMinionSpawner : MonoBehaviour
{
    public Transform[] teamAspawns;
    public Transform[] teamBspawns;

    bool minionsEnabled;
    float minionsMultiplier;
    public GameObject[] teamAUnits;
    public GameObject[] teamBUnits;
    public int defaultUnitsNumber;

    public float spawnInterval;
    public float spawningStartDelay;
    float nextSpawnTime;

    bool spawningStarted = false;

    void Start()
    {
        minionsEnabled = GlobalSettings.enableAIInVersus;
        minionsMultiplier = GlobalSettings.versusUnitsAmount;
    }

    void Update()
    {
        if (minionsEnabled)
        {
            if (!spawningStarted)
            {
                if(Time.timeSinceLevelLoad > spawningStartDelay)
                {
                    SpawnWave();
                    spawningStarted = true;
                    nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
                }
            }
            else
            {
                if(Time.timeSinceLevelLoad > nextSpawnTime)
                {
                    SpawnWave();
                    nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
                }
            }
        }
    }

    void SpawnWave()
    {
        int unitsNumber = (int)(defaultUnitsNumber * minionsMultiplier);

        for (int i = 0; i < unitsNumber; i++)
        {
            GameObject spawnedUnitA = Instantiate(teamAUnits[Random.Range(0, teamAUnits.Length)], teamAspawns[Random.Range(0,teamAspawns.Length)].position, Quaternion.identity);
            GameObject spawnedUnitB = Instantiate(teamBUnits[Random.Range(0, teamBUnits.Length)], teamBspawns[Random.Range(0,teamBspawns.Length)].position, Quaternion.identity);
           
        }
    }
}
