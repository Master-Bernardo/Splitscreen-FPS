using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBKiller : MonoBehaviour
{
    public Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 0.1)
        {
            if (collision.gameObject.tag != "Player")
            {
                IDamageable<float> damageable = collision.gameObject.GetComponent<IDamageable<float>>();
                if(damageable!=null)damageable.TakeDamage(500);
            }
        }
        
    }
}
