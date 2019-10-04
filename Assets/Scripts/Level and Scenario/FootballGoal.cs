using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballGoal : MonoBehaviour
{
    public int id;
    public VersusModeManager versusModeManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            if(id == 0)
            {
                versusModeManager.AddPoints(1, 1);

            }
            else
            {
                versusModeManager.AddPoints(0, 1);

            }
        }
    }
}
