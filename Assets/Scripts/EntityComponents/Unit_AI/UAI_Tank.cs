using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAI_Tank : EC_UnitAI
{
    public EC_Movement movement;
    public Transform destination;

    // Start is called before the first frame update
    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);

        movement.MoveTo(destination.position);
    }
}
