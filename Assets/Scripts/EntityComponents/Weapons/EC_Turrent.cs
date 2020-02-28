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

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);

        defaultHorRotation = turrentHorizontalRotater.localRotation;
        defaultVerRotation = turrentVerticalRotater.localRotation;
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        if (sensing.nearestEnemy != null)
        {
            //calculate desired rotation direction
            Vector3 nearestEnemyPos = sensing.nearestEnemy.GetPositionForAiming();

            Vector3 desiredHorAimDirection = myEntity.transform.InverseTransformDirection(nearestEnemyPos - turrentHorizontalRotater.position);
            Quaternion desiredHorRotation = Quaternion.LookRotation(desiredHorAimDirection);
            desiredHorRotation.eulerAngles = new Vector3(0f, desiredHorRotation.eulerAngles.y, desiredHorRotation.eulerAngles.z);

            Vector3 desiredVerAimDirection = myEntity.transform.InverseTransformDirection(nearestEnemyPos - turrentVerticalRotater.position);
            Quaternion desiredVerRotation = Quaternion.LookRotation(desiredVerAimDirection);
            desiredVerRotation.eulerAngles = new Vector3(desiredVerRotation.eulerAngles.x,0f , 0f);
       

            //rotate
            turrentHorizontalRotater.localRotation = Quaternion.RotateTowards(turrentHorizontalRotater.localRotation, desiredHorRotation, horizontalRotationSpeed * Time.deltaTime);
            turrentVerticalRotater.localRotation = Quaternion.RotateTowards(turrentVerticalRotater.localRotation, desiredVerRotation, verticalRotationSpeed * Time.deltaTime);
        }
    }
}
