using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_DestructiveEnvironment : MonoBehaviour, IDamageable<float>//, IPusheable<Vector3>
{
    //public bool isPusheable;
    public EC_Health health;
    public Rigidbody rb;

    public void TakeDamage(float damage)
    {
        health.OnTakeDamage(damage);
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
