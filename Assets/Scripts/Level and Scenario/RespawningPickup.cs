using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningPickup : MonoBehaviour
{
    public float respawnTime;
    public GameObject pickup;
    bool spawnDelayed;
    float nextSpawnTime;

    private void Start()
    {
        OnPickup();
    }

    private void Update()
    {
        if (spawnDelayed)
        {
            if (Time.time > nextSpawnTime)
            {
                spawnDelayed = false;
                GameObject obj = Instantiate(pickup, transform.position, transform.rotation);
                obj.transform.GetChild(1).GetComponent<Interactable>().OnSucessfullyInteract.AddListener(delegate { OnPickup(); });
            }
        }
       
    }

    public void OnPickup()
    {
        spawnDelayed = true;
        nextSpawnTime = Time.time + respawnTime;
    }
}
