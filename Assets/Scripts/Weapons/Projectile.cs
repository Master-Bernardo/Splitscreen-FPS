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

    [Header("Sound")]
    public AudioClip flySound; //TODO implement this for fireballs or similar
    public AudioClip impactSound;
    public AudioSource audioSource;
    //public Transform audioSourceTransform;
    public ProjectileImpactSound impactSoundDisableController;



    public void SetVelocity(float startVelocity)
    {
        this.startVelocity = startVelocity;
        rb.velocity = transform.forward * startVelocity;
    }

    //used when we get this element from the pool
    public void Activate(float startVelocity, int teamID, float damage, GameEntity shooterEntity)
    {
        this.startVelocity = startVelocity;
        rb.velocity = transform.forward * startVelocity;
        projectileTeamID = teamID;
        this.damage = damage;
        this.shooterEntity = shooterEntity;
        
        if (audioSource != null)
        {
            if (flySound != null)
            {
                audioSource.loop = true;
                audioSource.clip = flySound;
                audioSource.Play();
            }
            impactSoundDisableController.ResetPosition(transform);
           
        }


    }


    private void FixedUpdate()
    {
        if (pushes)
        {
            velocityLastFrame = rb.velocity;
        }
    }

    protected void PlayImpactSound()
    {
        if (audioSource != null)
        {
            if (impactSound != null)
            {
                impactSoundDisableController.Deparent();

                if (audioSource)
                {
                    audioSource.loop = false;
                    audioSource.clip = impactSound;
                    audioSource.Play();
                }
              
            }
        }
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {
        PlayImpactSound();



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
                pusheable.Push(velocityLastFrame.normalized* pushForce * Settings.Instance.forceMultiplier);              
            }
        }

        gameObject.SetActive(false);

    }

    protected virtual void GiveDamage(IDamageable<DamageInfo> damageable)
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
