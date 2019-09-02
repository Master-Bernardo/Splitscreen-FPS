using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_MissileFighter : Ab_UnitAI
{
    public B_MissileFighter missileBehaviour;

    public EC_Movement movement;
    public EC_ScanForEnemyUnits sensing;
    public EC_MissileWeapon weapon;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, movement, sensing, weapon);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(missileBehaviour);
        }
        else
        {
            SetCurrentBehaviour(null);
        }
    }
}
