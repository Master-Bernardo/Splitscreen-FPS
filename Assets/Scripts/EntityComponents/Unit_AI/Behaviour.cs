using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Behaviour 
{
    public float behaviourUpdateInterval;
    float nextBehaviourUpdateTime;
    [HideInInspector]
    //every behaviour needs some components that the unitAI needs to have saved
    public GameEntity entity;

    /*public Behaviour(Ab_UnitAI unitAI)
    {
        this.unitAI = unitAI;
    }*/

    public virtual void SetUpBehaviour(GameEntity entity)
    {
        this.entity = entity;
    }

    public virtual void UpdateBehaviour()
    {
        if(Time.time > nextBehaviourUpdateTime)
        {
            nextBehaviourUpdateTime = Time.time + behaviourUpdateInterval;
            Update();
        }
    }

    protected virtual void Update()
    {

    }

    public virtual void OnBehaviourEnter()
    {

    }

    public virtual void OnBehaviourExit()
    {

    }

    public virtual void OnDie()
    {

    }
}

[System.Serializable]
public class B_WanderAroundPosition: Behaviour
{
    public float maxDistanceToPositionWhileWandering;
    EC_Movement movement;
    public Transform positionToWanderAround;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement)
    {
        this.entity = entity;
        this.movement = movement;
        if (positionToWanderAround == null)
        {
            positionToWanderAround = entity.transform;
        }
    }

    public void SetPositionToWanderAround(Transform newPositionToWanderAround)
    {
        positionToWanderAround = newPositionToWanderAround;
        Debug.Log("Sette 3");
        Debug.Log("pos: " + newPositionToWanderAround);
    }

    protected override void Update()
    {

            Vector3 wanderPoint = UnityEngine.Random.insideUnitSphere * 4;
            wanderPoint.y = entity.transform.position.y;

            wanderPoint += entity.transform.forward * 4 + entity.transform.position;

            //if he would stray off to far, bring him back to base
            if (Vector3.Distance(positionToWanderAround.position, wanderPoint) > maxDistanceToPositionWhileWandering)
            {
                wanderPoint += (positionToWanderAround.position - wanderPoint) / 4;
            }

            movement.MoveTo(wanderPoint);
        
    }
}

[System.Serializable]
public class B_Flee: Behaviour
{
    EC_ScanForEnemyUnits sensing;
    EC_Movement movement;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits sensing)
    {
        this.entity = entity;
        this.movement = movement;
        this.sensing = sensing;
    }

    protected override void Update()
    {
        if (sensing.nearestEnemy != null)
        {
            movement.MoveTo((entity.transform.position - sensing.nearestEnemy.transform.position).normalized*5);
        }
        else
        {
            movement.Stop();
        }
    }


}

[System.Serializable]
public class B_MeleeFighter : Behaviour
{
    EC_ScanForEnemyUnits enemySensing;
    EC_Movement movement;

    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    //the fighter will be between this distances, he does not need to stop to attack
    public float perfectMeleeDistance;
    public float maxMeleeDistance;
    //public float minMeleeDistance;

    float myWidth;
    float enemyWidth;

    //meleefighting
    EC_MeleeWeapon weapon;
    [SerializeField]
    bool inRange;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits enemySensing, EC_MeleeWeapon weapon)
    {
        this.entity = entity;
        this.movement = movement;
        this.enemySensing = enemySensing;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);
        maxMeleeDistance *= maxMeleeDistance;
    }

    /*enum MeleeFighterState
    {
        TooFar,
        TooNear,
        InMeleeDistance
    }

    MeleeFighterState state;*/

    protected override void Update()
    {
        if (Time.time > nextDistanceCheckTime)
        {
            nextDistanceCheckTime = Time.time + distanceCheckingInterval;

            myWidth = entity.width;
            enemyWidth = enemySensing.nearestEnemy.width;

            Vector3 nearestEnemyPosition = enemySensing.nearestEnemy.transform.position;
            Vector3 myPosition = entity.transform.position;

            float widthFactor = myWidth + enemyWidth; //multiply the resulting distanceVectorBythisFactor to also use width
            Vector3 distanceVector = nearestEnemyPosition - myPosition;
            //float distanceToEnemySquared = (distanceVector - distanceVector.normalized * widthFactor).sqrMagnitude;
            float distanceToEnemy = (distanceVector - distanceVector.normalized * widthFactor).magnitude;

            //if the enemy is moving, we move to the position he will be at the time we arrive
            EC_Movement enemyMovement = enemySensing.nearestEnemy.GetComponent<EC_Movement>();


            if (enemyMovement.IsMoving())
            {
                    //heuristically calculae future position
                    //1. how long will it take for me to reach the enemy?
                    float timeToReachEnemy = distanceToEnemy / movement.GetMaxSpeed();
                    //2. where will the enemy be after this time
                    Vector3 futurePosition = nearestEnemyPosition + enemyMovement.GetCurrentVelocity() * timeToReachEnemy;
 
                    movement.MoveTo(futurePosition);
            }   
            else
            {
                movement.MoveTo(nearestEnemyPosition + (myPosition - nearestEnemyPosition).normalized * (perfectMeleeDistance+myWidth+enemyWidth));
            }

            if ((nearestEnemyPosition - myPosition).sqrMagnitude > maxMeleeDistance)
            {
                inRange = false;
                movement.StopLookAt();
            }
            else
            {
                inRange = true;
                movement.LookAt(enemySensing.nearestEnemy.transform);

            }
        }

        //Debug.Log("in range? : " + inRange);

        if (inRange)
        {
            if (weapon.CanAttack())
            {
                weapon.Attack();
            }
        }

    }

    public override void OnBehaviourExit()
    {     
        movement.Stop();
        movement.StopLookAt();
        inRange = false;
    }
}

[System.Serializable]
public class B_MissileFighter : Behaviour
{
    //refactor and add distance checking with width

    EC_Movement movement;
    EC_ScanForEnemyUnits enemySensing;
    EC_MissileWeapon weapon;
    //goes to nearest enemy, shoots  and looks at them when in range, tries not to get too close

    public float perfectShootingDistance;
    public float maxShootingDistance;
    bool inRange;

    float myWidth;
    float enemyWidth;


    public float distanceCheckingInterval;
    float nextDistanceCheckTime;

    public void SetUpBehaviour(GameEntity entity, EC_Movement movement, EC_ScanForEnemyUnits enemySensing, EC_MissileWeapon weapon)
    {
        this.enemySensing = enemySensing;
        this.entity = entity;
        this.movement = movement;
        this.weapon = weapon;

        nextDistanceCheckTime = UnityEngine.Random.Range(0, distanceCheckingInterval);

        maxShootingDistance *= maxShootingDistance;

    }

    protected override void Update()
    {
        if (Time.time > nextDistanceCheckTime)
        {
            nextDistanceCheckTime = Time.time + distanceCheckingInterval;

            myWidth = entity.width;
            enemyWidth = enemySensing.nearestEnemy.width;

            Vector3 nearestEnemyPosition = enemySensing.nearestEnemy.transform.position;
            Vector3 myPosition = entity.transform.position;

            float widthFactor = myWidth + enemyWidth; //multiply the resulting distanceVectorBythisFactor to also use width
            Vector3 distanceVector = nearestEnemyPosition - myPosition;
            float distanceToEnemy = (distanceVector - distanceVector.normalized * widthFactor).sqrMagnitude;

            movement.MoveTo(nearestEnemyPosition + (myPosition - nearestEnemyPosition).normalized * (perfectShootingDistance + myWidth + enemyWidth));
     
            if ((nearestEnemyPosition - myPosition).sqrMagnitude > maxShootingDistance)
            {
                inRange = false;
                movement.StopLookAt();
                weapon.StopAiming();
                //Debug.Log("stop aim");
            }
            else
            {
                inRange = true;
               // Debug.Log("start aim");

                movement.LookAt(enemySensing.nearestEnemy.transform);
                weapon.AimAt(enemySensing.nearestEnemy);

            }
        }

        if (inRange)
        {
            if (weapon.CanShoot())
            {
                weapon.Shoot();
            }
        }

       
    }

    public override void OnBehaviourExit()
    {
        movement.Stop();
        movement.StopLookAt();
        inRange = false;
    }
}