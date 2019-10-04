using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool spawnOnEnable;
    public int numberToSpawn;
    public GameObject unitToSpawn;
    public bool registerInHordeManager;

    private void Start()
    {
        if (spawnOnEnable)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameEntity unit = Spawn(unitToSpawn);
                if (registerInHordeManager)
                {
                    HordeModeManager.Instance.RegisterEnemyIntoWave(unit);
                }
            }
        }
    }

    public GameEntity Spawn(GameObject prefab)
    {
        return Instantiate(prefab, transform.position, transform.rotation).GetComponent<GameEntity>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
