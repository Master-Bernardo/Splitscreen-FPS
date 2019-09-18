using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    public GameObject unitsToSpawn;
    bool spawn = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (spawn) unitsToSpawn.SetActive(true);
            else unitsToSpawn.SetActive(false);
        }
    }
}
