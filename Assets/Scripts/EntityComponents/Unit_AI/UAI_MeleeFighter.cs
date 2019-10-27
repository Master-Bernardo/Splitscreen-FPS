using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_MeleeFighter : EC_UnitAI
{
    public B_MeleeFighter meleeBehaviour;

    public EC_Movement movement;
    public EC_Sensing sensing;
    //public MeleeWeapon weapon;
    public EC_HumanWeaponController weaponController;
    public Animator handsAnimator;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        if (handsAnimator != null)
        {
            meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weaponController,handsAnimator);

        }
        else
        {
            meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weaponController);

        }
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
