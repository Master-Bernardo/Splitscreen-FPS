using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameEntity, IDamageable<DamageInfo>
{
    public UnitData unitData;
    [SerializeField]
    public DamageManager damageManager;

    public void TakeDamage(DamageInfo damageInfo)
    {
        foreach (EntityComponent component in components)
        {
            component.OnTakeDamage(damageInfo);
        }
    }


    /*
     * public void TakeDamage(float damage)
    {
        foreach (EntityComponent component in components)
        {
            component.OnTakeDamage(damage);
        }
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        foreach (EntityComponent component in components)
        {
            component.OnTakeDamage(damage, force);
        }
    }

    public void TakeDamage(float damage, GameEntity damageGiver)
    {
        foreach (EntityComponent component in components)
        {
            component.OnTakeDamage(damage);
            component.OnTakeDamageFrom(damage, damageGiver);
        }
    }

    public void TakeDamage(float damage, Vector3 force, GameEntity damageGiver)
    {
        foreach (EntityComponent component in components)
        {
            component.OnTakeDamage(damage, force);
            component.OnTakeDamageFrom(damage, damageGiver);
        }
    }
    */
}
