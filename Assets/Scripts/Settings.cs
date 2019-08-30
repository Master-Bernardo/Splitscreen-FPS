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
    public int unitsLayer;

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
