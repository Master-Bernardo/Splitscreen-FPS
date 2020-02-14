using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_ShooterFighter : EC_UnitAI
{
    public B_MissileFighter missileBehaviour;
    public B_MeleeFighter meleeBehaviour;
    public B_HumanoidIdle idleBehaviour;
    public EC_HumanWeaponSystem weaponSystem;


    public EC_Movement movement;
    public EC_Sensing sensing;
    public EC_MissileWeaponController missileWeaponController;
    public EC_MeleeWeaponController meleeWeaponController;
    public Animator handsAnimator;
    public float meleeRadius;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, movement, sensing, missileWeaponController, handsAnimator, weaponSystem);
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, meleeWeaponController, handsAnimator);
        idleBehaviour.SetUpBehaviour(handsAnimator);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            if (Vector3.Distance(sensing.nearestEnemy.transform.position, transform.position) < meleeRadius)
            {
                if (currentBehaviour !=meleeBehaviour)
                {
                    //switch weapon if the behaviour was changed
                    weaponSystem.ChangeWeapon(0);
                    SetCurrentBehaviour(meleeBehaviour);
                }
            }
            else
            {
                if (currentBehaviour != missileBehaviour)
                {
                    //switch weapon if the behaviour was changed
                    weaponSystem.ChangeWeapon(1);
                    SetCurrentBehaviour(missileBehaviour);
                    Debug.Log("changedWeapon");

                }
            }
        }
        else
        {
            SetCurrentBehaviour(idleBehaviour);
        }
    }
}
