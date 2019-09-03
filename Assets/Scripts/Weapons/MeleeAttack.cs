using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//some meleeWeapons have 
[System.Serializable]
public class MeleeAttack
{
    public float damage;
    public float meleeAttackInterval;

    //how long does it take for the swing to hit its target?
    public float attackDuration;

   // public bool drawDamageGizmo;
    [Tooltip("position relative to the unit")]
    public Vector3 hitPosition;
    public float hitSphereRadius;

    [Header("pushing")]
    public bool pushes;
    public float pushForce;
    public float pushKillForce;
    public Vector3 pushDirection;
    public bool defaultDirection = true; //if this is true we just push the enemy away from us

    public string animationName;

}
