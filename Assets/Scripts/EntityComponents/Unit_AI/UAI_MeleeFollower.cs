using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_MeleeFollower : EC_UnitAI
{
    public B_MeleeFighter meleeBehaviour;
    public B_WanderAroundPosition wanderBehaviour;
    public float maxDistanceToPlayer;

    public EC_Movement movement;
    public EC_Sensing sensing;
    public MeleeWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
        wanderBehaviour.SetUpBehaviour(entity, movement);
        maxDistanceToPlayer *= maxDistanceToPlayer;
    }

    public void SetPositionToWanderAround(Transform position)
    {
        wanderBehaviour.SetPositionToWanderAround(position);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
                if ((sensing.nearestEnemy.transform.position - transform.position).sqrMagnitude > maxDistanceToPlayer)
                {
                    SetCurrentBehaviour(wanderBehaviour);

                }
                else
                {
                    SetCurrentBehaviour(meleeBehaviour);
                }
            }
        else
        {
            SetCurrentBehaviour(wanderBehaviour);
        }
    }
}
