using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HordeUnitBoss
{
    //boss units get spawned no matter what
    public GameObject unitPrefab;
    [Tooltip("How many of them will be spawned")]
    public int number;
    public int size;
}

[System.Serializable]
public class HordeUnit
{
    //normal Horde units get spawned randomly as long as there is enough points left
    public GameObject unitPrefab;
    public int recruitmentCost;
    public int size;
}

[System.Serializable]
public class HordeWave
{
    //we spawn all the boss units and then we spawn the normal Horde Units as long as there are enough points
    public HordeUnitBoss[] bossUnits;
    public HordeUnit[] units;
    [Tooltip("in some waves we may want to have fewer enemies because the boss is very strong")]
    public float unitsPointsMultiplier = 1;
}

//defines which units will be spawned and when
[CreateAssetMenu(fileName = "New HordeScenario", menuName = "HordeScenario")]
public class HordeScenario : ScriptableObject
{
    public HordeWave[] waves;

}
