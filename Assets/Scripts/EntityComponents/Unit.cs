using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameEntity, IDamageable<float>
{
    public UnitData unitData;
    [SerializeField]
    //float currentHealth;

    //public virtual void Awake()
    //{
        //currentHealth = unitData.healthPoints;
    //}

    public void TakeDamage(float damage)
    {
        foreach (EntityComponent ability in components)
        {
            ability.OnTakeDamage(damage);
        }
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        foreach (EntityComponent ability in components)
        {
            ability.OnTakeDamage(damage, force);
        }
    }
}
