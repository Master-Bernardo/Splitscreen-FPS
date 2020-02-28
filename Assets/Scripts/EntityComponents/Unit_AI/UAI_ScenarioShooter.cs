using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_ScenarioShooter : EC_UnitAI
{
    //Scenario Shooter goes from cover to cover, with orders, on his way between covers he can aslo shoot

    public B_MissileFighterInCover missileBehaviour;
    public B_HumanoidIdle idleBehaviour;

    public EC_Movement movement;
    public EC_Sensing sensing;
    public EC_MissileWeaponController weapon;
    public Animator handsAnimator;
    public EC_HumanWeaponSystem weaponSystem;

    Vector3 currentCoverPosition;

    public Transform firstCover;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        currentBehaviour = null;
        missileBehaviour.SetUpBehaviour(entity, sensing, weapon, handsAnimator, weaponSystem);
        idleBehaviour.SetUpBehaviour(handsAnimator);
        GoToCover(firstCover.position);
    }

    public override void CheckCurrentBehaviour()
    {
        if (sensing.nearestEnemy != null)
        {
            SetCurrentBehaviour(missileBehaviour);
        }
        else
        {
            SetCurrentBehaviour(idleBehaviour);
        }
    }

    public void GoToCover(Vector3 newCoverPosition)
    {
        currentCoverPosition = newCoverPosition;
        Debug.Log("move to " + currentCoverPosition);
        movement.MoveTo(currentCoverPosition);
        
    }
}
