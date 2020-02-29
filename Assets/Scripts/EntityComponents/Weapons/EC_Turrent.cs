using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_Turrent : EntityComponent
{
    public EC_Sensing sensing;
    Quaternion defaultHorRotation;
    Quaternion defaultVerRotation;

    public Transform turrentHorizontalRotater;
    public Transform turrentVerticalRotater;

    public float horizontalRotationSpeed;
    public float verticalRotationSpeed;

    public float initialLaunchSpeed;
    public int teamID;
    public Transform shootPoint;
    public float bloom;
    public string projectileTag;
    public float shootingInterval;
    float nextShootTime;
    public float damage;
    public float tolerableAimingAngleError;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);

        defaultHorRotation = turrentHorizontalRotater.localRotation;
        defaultVerRotation = turrentVerticalRotater.localRotation;

        nextShootTime = Time.timeSinceLevelLoad + Random.Range(0, shootingInterval);
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        Quaternion desiredHorRotation;
        Quaternion desiredVerRotation;

        if (sensing.nearestEnemy != null)
        {
            //calculate desired rotation direction
            Vector3 nearestEnemyPos = sensing.nearestEnemy.GetPositionForAiming();

            Vector3 desiredHorAimDirection = myEntity.transform.InverseTransformDirection(nearestEnemyPos - turrentHorizontalRotater.position);
            desiredHorRotation = Quaternion.LookRotation(desiredHorAimDirection);
            desiredHorRotation.eulerAngles = new Vector3(0f, desiredHorRotation.eulerAngles.y, desiredHorRotation.eulerAngles.z);

            Vector3 desiredVerAimDirection = myEntity.transform.InverseTransformDirection(nearestEnemyPos - turrentVerticalRotater.position);
            desiredVerRotation = Quaternion.LookRotation(desiredVerAimDirection);
            desiredVerRotation.eulerAngles = new Vector3(desiredVerRotation.eulerAngles.x,0f , 0f);
       

            //check if we want to shoot;

            //if rotation current desired > tolerableAimingAngleError -> shoot
            float aimingError = Quaternion.Angle(turrentVerticalRotater.rotation, Quaternion.LookRotation(nearestEnemyPos - turrentVerticalRotater.position));
          
            if(aimingError < tolerableAimingAngleError)
            {
                if (Time.timeSinceLevelLoad >= nextShootTime)
                {
                    nextShootTime = Time.timeSinceLevelLoad + shootingInterval;
                    Shoot();
                }
            }
        }
        else
        {
            //if no enemy return to desired look direction

            desiredHorRotation = defaultHorRotation;
            desiredVerRotation = defaultVerRotation;
        }

        //rotate
        turrentHorizontalRotater.localRotation = Quaternion.RotateTowards(turrentHorizontalRotater.localRotation, desiredHorRotation, horizontalRotationSpeed * Time.deltaTime);
        turrentVerticalRotater.localRotation = Quaternion.RotateTowards(turrentVerticalRotater.localRotation, desiredVerRotation, verticalRotationSpeed * Time.deltaTime);
    }

    void Shoot()
    {
            Projectile projectile = ProjectilePooler.Instance.SpawnFromPool(projectileTag, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
            projectile.Activate(initialLaunchSpeed, teamID, damage, myEntity);
    }
}
