using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable<T>
{
    /* void TakeDamage(T damage);

     //the other form of take damage, we also give a force to throw the corpse by
     void TakeDamage(T damage, Vector3 force);*/
    void TakeDamage(T damageInfo);
}


//buildings and Units derive from this class - should i use it instead of damageable interface
public class GameEntity : MonoBehaviour
{
    public int teamID;
    public EntityComponent[] components;
    public Vector3 aimingCorrector; //correctes the aiming, sets it higher, because every units 0 is at the bottom for distance chekcs
    public UnityEvent onDieEvent;
    public float width;

    private void Start()
    {
        foreach (EntityComponent component in components)
        {
            component.SetUpComponent(this);
        }
    }

    protected void Update()
    {
        foreach (EntityComponent ability in components)
        {
            ability.UpdateComponent();
        }
    }

    protected void FixedUpdate()
    {
        foreach (EntityComponent ability in components)
        {
            ability.FixedUpdateComponent();
        }
    }

    public Vector3 GetPositionForAiming()
    {
        return (transform.position + aimingCorrector);
    }

    /*public virtual void Die()
    {
        onDieEvent.Invoke();
        foreach (EntityComponent ability in components)
        {
            ability.OnDie();
        }
        Destroy(gameObject);
    }*/

    public virtual void Die(GameEntity killer)
    {
        onDieEvent.Invoke();
        foreach (EntityComponent component in components)
        {
            component.OnDie(killer);
        }
        Destroy(gameObject);
    }
}
