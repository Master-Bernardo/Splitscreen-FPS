using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo 
{
    public float damage;
    public bool appliesForce;
    public Vector3 killPushForce; // ges only aplied to corpse
    public GameEntity damageGiver;
    public DamageType type;

    public DamageInfo(float damage)
    {
        this.damage = damage;
        appliesForce = false;
        damageGiver = null;
        type = DamageType.Default;
    }

    public DamageInfo(float damage, Vector3 damageForce, GameEntity damageGiver)
    {
        this.damage = damage;
        appliesForce = true;
        this.killPushForce = damageForce;
        this.damageGiver = damageGiver;
        type = DamageType.Default;
    }

    public DamageInfo(float damage, GameEntity damageGiver)
    {
        this.damage = damage;
        appliesForce = false;
        this.damageGiver = damageGiver;
        type = DamageType.Default;
    }
}
