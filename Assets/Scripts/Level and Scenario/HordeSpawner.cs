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


    void OnEnable()
    {
        HordeModeManager.Instance.AddSpawner(this);
    }

    private void OnDisable()
    {
        HordeModeManager.Instance.RemoveSpawner(this);

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
