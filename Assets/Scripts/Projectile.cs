using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float startVelocity;
    public int projectileTeamID; //who shoot this projectile

    //public float radius;
    Rigidbody rb;
    [Tooltip("does this projectile push enemies back?")]
    public bool pushes;
    public float pushForce;
    [Tooltip("default pushForce is the one from the rb")]
    public bool defaultPushForce;

    Vector3 velocityLastFrame; //wen need to save this because it changes on collision

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * startVelocity;

    }

    private void FixedUpdate()
    {
        if (pushes)
        {
            velocityLastFrame = rb.velocity;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        IDamageable<float> damageable = collision.gameObject.GetComponent<IDamageable<float>>();
        IPusheable<Vector3> pusheable = collision.gameObject.GetComponent<IPusheable<Vector3>>();

        if (damageable != null)
        {
           // Debug.Log("hit");
            // check who did we hit, check if he has an gameEntity
            GameEntity entity = collision.gameObject.GetComponent<GameEntity>();
            if (entity != null)
            {
                if (!Settings.Instance.friendlyFire)
                {
                    DiplomacyStatus diplomacyStatus = Settings.Instance.GetDiplomacyStatus(projectileTeamID, entity.teamID);
                    if(diplomacyStatus == DiplomacyStatus.War)
                    {
                        damageable.TakeDamage(damage);
                    }

                }
                else
                {
                    damageable.TakeDamage(damage);
                }

            }
            else
            {
                damageable.TakeDamage(damage);
            }
        }

        if (pusheable != null)
        {
            if (defaultPushForce)
            {
                pusheable.Push(velocityLastFrame);
            }
            else
            {
                pusheable.Push(velocityLastFrame.normalized* pushForce);
               // Debug.Log(rb.velocity);
                //Debug.Log("actual: " + rb.velocity.normalized * pushForce);
                
            }
        }

        Destroy(gameObject);

    }


}
