using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSpawner : MonoBehaviour
{
    public GameObject goToSpwan;
    public int number;
    public float interval;
    float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time +  Random.Range(0, interval);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            float j = 0;
            nextSpawnTime += interval;
            for (int i = 0; i < number; i++)
            {
                Instantiate(goToSpwan, transform.position + new Vector3(j, 0f, 0f), transform.rotation);
                j += 0.5f;
            }
        }
    }
}
