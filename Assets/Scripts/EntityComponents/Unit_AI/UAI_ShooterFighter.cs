using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_ShooterFighter : EC_UnitAI
{
    public B_MissileFighter missileBehaviour;
    public B_MeleeFighter meleeBehaviour;
    public EC_PlayerWeaponSystem weaponSystem;


    public EC_Movement movement;
    public EC_Sensing sensing;
    public EC_MissileWeaponController weapon;
    public EC_MeleeWeaponController weaponController;
    public Animator handsAnimator;
    public float meleeRadius;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, movement, sensing, weapon, handsAnimator);
        meleeBehaviour.SetUpBehaviour(entity, movement, sensing, weaponController, handsAnimator);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            if (Vector3.Distance(sensing.nearestEnemy.transform.position, transform.position) < meleeRadius)
            {
                SetCurrentBehaviour(meleeBehaviour);
            }
            else
            {
                SetCurrentBehaviour(missileBehaviour);
            }
        }
        else
        {
            SetCurrentBehaviour(null);
        }
    }
}
