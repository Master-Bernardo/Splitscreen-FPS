using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiveEnvironment : MonoBehaviour, IDamageable<DamageInfo>//, IPusheable<Vector3>
{
    //public bool isPusheable;
    public EC_Health health;
    public Rigidbody rb;

    public void TakeDamage(DamageInfo info)
    {
        health.OnTakeDamage(info);
    }

    public virtual void TakeDamage(float damage, Vector3 force)
    {

    }

    /*public void Push(Vector3 force)
    {
        if (isPusheable)
        {
            rb.AddForce(force);
        }
    }*/
}
