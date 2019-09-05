using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class EC_MeleeWeapon : EntityComponent
{
    //public float meleeRange;
    public float meleeDamage;
    public float meleeAttackInterval;
    float nextPrepareMeleeAttackTime;

    //public Transform weaponTransform;
    public Animator weaponAnimator;
  
    float nextMeleeAttackTime;
    public float attackDuration;
    bool attack;

    public bool drawDamageGizmo;
    [Tooltip("position relative to the unit")]
    public Vector3 hitPosition;
    public float hitSphereRadius;
    int weaponTeamID;

    [Header("pushing")]
    public bool pushes;
    public float pushForce;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        //meleeRange *= meleeRange; //because we do a square magnitude check
        weaponTeamID = entity.teamID;
    }

    public void Attack()
    {
        if (Time.time > nextPrepareMeleeAttackTime)
        {
            nextPrepareMeleeAttackTime = Time.time + meleeAttackInterval;

            //target.TakeDamage(meleeDamage);
            attack = true;
            nextMeleeAttackTime = Time.time + attackDuration;
            weaponAnimator.SetTrigger("Attack1");
            //currentTarget = target;
        }
    }

    public override void UpdateComponent()
    {
        if (attack)
        {
            if (Time.time > nextMeleeAttackTime)
            {
                ExecuteAttack();
                attack = false;
            }
        }
    }

    void ExecuteAttack()
    {
        // if (currentTarget != null) currentTarget.TakeDamage(meleeDamage);

        Collider[] visibleColliders = Physics.OverlapSphere(myEntity.transform.TransformPoint(hitPosition), hitSphereRadius);

        for (int i = 0; i < visibleColliders.Length; i++)
        {
            IDamageable<float> damageable = visibleColliders[i].gameObject.GetComponent<IDamageable<float>>();

           // Debug.Log("collider " + visibleColliders[i]);
            if (damageable != null)
            {
                // check who did we hit, check if he has an gameEntity
                GameEntity entity = visibleColliders[i].gameObject.GetComponent<GameEntity>();
               // Debug.Log("damegable entity: " + entity);
                if (entity != null)
                {
                    if (!Settings.Instance.friendlyFire)
                    {
                        DiplomacyStatus diplomacyStatus = Settings.Instance.GetDiplomacyStatus(weaponTeamID, entity.teamID);
                        if (diplomacyStatus == DiplomacyStatus.War)
                        {
                            damageable.TakeDamage(meleeDamage);
                        }

                    }
                    else
                    {
                        damageable.TakeDamage(meleeDamage);
                    }

                }
                else
                {
                    damageable.TakeDamage(meleeDamage);
                }

                if (pushes)
                {
                    IPusheable<Vector3> pusheable = visibleColliders[i].gameObject.GetComponent<IPusheable<Vector3>>();

                    if (pusheable != null)
                    {
   
                        Vector3 direction = (visibleColliders[i].gameObject.transform.position - myEntity.transform.position).normalized;
                       // Debug.Log("push: " + pusheable);
                        pusheable.Push(direction * pushForce);
                    }
                }
                

                return;
            }

        }




    }

    public bool CanAttack()
    {
        if (Time.time > nextPrepareMeleeAttackTime) return true;
        else return false;
    }

    public bool IsInMeleeRange(Vector3 target)
    {//refsctor use the hit box range

        /* if ((target - myEntity.transform.position).sqrMagnitude < meleeRange)
         {
             return true;
         }
         else
         {
             return false;
         }/*/
      /*  return false;
    }

    private void OnDrawGizmos()
    {
        if (myEntity != null)
        {
            if (drawDamageGizmo)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(myEntity.transform.TransformPoint(hitPosition), hitSphereRadius);
            }
        }   
    }
}
*/