using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : EC_Movement, IPusheable<Vector3>
{

    public override void SetUpComponent(GameEntity entity)
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log("setup");

    }

    public override void UpdateComponent()
    {
    }

    public override void Push(Vector3 force)
    {
        if (canBePushed)
        {
            rb.AddForce(force);
        }
    }

    public override bool IsMoving()
    {
        return rb.velocity.magnitude > 0.1;
    }

    public override Vector3 GetCurrentVelocity()
    {
        return rb.velocity;
    }
}
