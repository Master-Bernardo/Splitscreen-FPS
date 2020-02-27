using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//maybe change this to sensing with scanning obptions or let all the scanning options derive from snesing later
public class EC_Sensing : EntityComponent
{
    public enum SensingType
    {
        RadiusSensing,
        HordeSensing, //radiusSensing + get player form hordemanager
        VersusSensing //radius sensing + get player from versus manager, if no enemy units present
    }

    public SensingType type;

    public HashSet<GameEntity> enemiesInRange = new HashSet<GameEntity>();
    public GameEntity nearestEnemy;
    public LayerMask layerMask;

    public float scanInterval;
    public float scanRadius;
    float nextScanTime;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        nextScanTime = Time.time + Random.Range(0, scanInterval);
    }

    public override void UpdateComponent()
    {
        if (Time.time > nextScanTime)
        {
            //time.time is not very accurate
            nextScanTime += scanInterval;
            Scan();
           
        }
    }

    void Scan()
    {

        Collider[] visibleColliders = Physics.OverlapSphere(transform.position, scanRadius, layerMask);
        enemiesInRange.Clear();

        for (int i = 0; i < visibleColliders.Length; i++)
        {
            GameEntity currentEntity = visibleColliders[i].GetComponent<GameEntity>();
            if(currentEntity.teamID != myEntity.teamID)
            {
                enemiesInRange.Add(currentEntity);
            }
        }

        //get the nearest
        float nearestDistance = Mathf.Infinity;
        nearestEnemy = null;
        foreach(GameEntity enemy in enemiesInRange)
        {
            float currentDistance = (transform.position - enemy.transform.position).sqrMagnitude;
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestEnemy = enemy;
            }
        }

        if(type == SensingType.HordeSensing)
        {
            //if theres no nemey in range, go for the player
            if (nearestEnemy == null)
            {
                nearestEnemy = HordeModeManager.Instance.GetNearestPlayer(transform.position);
            }
        }
        else if(type == SensingType.VersusSensing)
        {
            if (nearestEnemy == null)
            {
                nearestEnemy = VersusModeManager.Instance.GetNearestEnemyPlayer(transform.position, myEntity.teamID);
            }
        }


    }
}
