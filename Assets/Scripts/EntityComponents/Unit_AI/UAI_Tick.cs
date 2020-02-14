using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_Tick : EC_UnitAI
{
    public B_Tick meleeBehaviour;
    public Behaviour idleBehaviour;

    public EC_Movement movement;
    public EC_Sensing sensing;
    public Explosives explosives;


    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, explosives);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(meleeBehaviour);
        }
        else
        {
            SetCurrentBehaviour(idleBehaviour);
        }
    }
}
