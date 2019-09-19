using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float damage;
    public float startVelocity;
    public int projectileTeamID; //who shoot this projectile
    public GameEntity shooterEntity;

    //public float radius;
    [SerializeField]
    Rigidbody rb;
    [Tooltip("does this projectile push enemies back?")]
    public bool pushes;
    public float pushForce;
    [Tooltip("default pushForce is the one from the rb")]
    public bool defaultPushForce;
    [Tooltip("this Force gets pllied to the corpse")]
    public float killPushForce;

    Vector3 velocityLastFrame; //wen need to save this because it changes on collision


    public void SetVelocity(float startVelocity)
    {
        this.startVelocity = startVelocity;
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
        IDamageable<DamageInfo> damageable = collision.gameObject.GetComponent<IDamageable<DamageInfo>>();
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
                        GiveDamage(damageable);
                    }

                }
                else
                {
                    GiveDamage(damageable);
                }

            }
            else
            {
                GiveDamage(damageable);
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
            }
        }

        gameObject.SetActive(false);

    }

    void GiveDamage(IDamageable<DamageInfo> damageable)
    {
        if (pushes)
        {
            //damageable.TakeDamage(damage, velocityLastFrame.normalized * killPushForce);
            damageable.TakeDamage(new DamageInfo(damage, velocityLastFrame.normalized * killPushForce, shooterEntity));
        }
        else
        {
            damageable.TakeDamage(new DamageInfo(damage, shooterEntity));
        }                    
    }


}
