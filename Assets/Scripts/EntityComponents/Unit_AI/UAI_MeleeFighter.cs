using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_MeleeFighter : EC_UnitAI
{
    public B_MeleeFighter meleeBehaviour;

    public EC_Movement movement;
    public EC_Sensing sensing;
    public MeleeWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        //wanderBehaviour.SetUpBehaviour(entity, movement);
    }

    /*public void SetPositionToWanderAround(Transform position)
    {
        wanderBehaviour.SetPositionToWanderAround(position);
    }*/

    public override void CheckCurrentBehaviour()
    {
       // Debug.Log("nearest enemy: " + sensing.nearestEnemy);
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(meleeBehaviour);
        }
        else
        {
            SetCurrentBehaviour(null);
        }
    }
}
