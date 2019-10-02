using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiplomacyStatus
{
    War,
    Neutral,
    Friendly
}

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public bool friendlyFire;
    public bool showAimingLine;
    //public int unitsLayer;
    [Header("Physics")]
    [Tooltip("Only applies to units with ecMovement or PlayerMovement")]
    public float gravityMultiplier;
    [Tooltip("Because with changing the gravityMultiplier we would need to change all forces - we do it here . If gravityMult is 1, this should also be one , if gravity is 8 ,this hsoulkd be 2,6667 or 1/3 of 8")]
    public float forceMultiplier;

    //how to handle who is enemy with who
   
    

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

    public DiplomacyStatus GetDiplomacyStatus(int myTeam, int otherTeam)
    {
        if (myTeam != otherTeam) return DiplomacyStatus.War;
        else return DiplomacyStatus.Friendly;
    }


}
