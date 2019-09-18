using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public class HordeSpawnerUnitContainer
{
    public GameObject unitPrefab;
    public int spawnCost;
}*/

/*HordeSpawnerType

   1 - Small,
   2 - Medium,
   3 - Large,
   4 - Extreme

*/
public class HordeSpawner : MonoBehaviour
{
    

    public int size;

    //every horde spawner has specific units it spawns with their own cost and propability - propability is achieved by dublicating specific units in the list
    //there should always be some unit with 1 cost, so we 

    //public HordeSpawnerUnitContainer[] unitsToSpawn;

    void OnEnable()
    {
        HordeModeManager.Instance.AddSpawner(this);
    }

    private void OnDisable()
    {
        HordeModeManager.Instance.RemoveSpawner(this);

    }

    /*public void Spawn(int spawnPoints)
    {
        int spawnPointsLeft = spawnPoints;
        List<HordeSpawnerUnitContainer> unitsWeCanAfford = new List<HordeSpawnerUnitContainer>();

        while (spawnPointsLeft > 0)
        {
            //1make a new List of units we can aford with the remaining points,
            unitsWeCanAfford.Clear();

            foreach (HordeSpawnerUnitContainer unit in unitsToSpawn)
            {
                if (unit.spawnCost <= spawnPointsLeft) unitsWeCanAfford.Add(unit);
            }

            //if theres no unit we can afford, break out of the look 
            if(unitsWeCanAfford.Count == 0)
            {
                spawnPointsLeft = 0;
                return;
            }

            //2.  choose on of them randomly
            HordeSpawnerUnitContainer unitToSpawn = unitsWeCanAfford[Random.Range(0, unitsWeCanAfford.Count)];

            Instantiate(unitToSpawn.unitPrefab, transform.position, transform.rotation);
            spawnPointsLeft -= unitToSpawn.spawnCost;
        }
    }*/

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
