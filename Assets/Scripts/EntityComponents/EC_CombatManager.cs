using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{
    Default,
    MeleeAttacking,
    MissileAttacking,
    Blocking,
    Reloading,
    Stunned
}

//Maybe betteer if it is just a class which shows the current state of this enemies, so enemies surrounging it know what he is doing
//takes care of all the combat logic, sends messages so other units about the current combat status of this unit and takes care of attacks, aiming of weapons, etc

    //if combat manager takes care of combat animations, then who takes care of normal animation? ADD EXTRA ANIMATIONMNAGER?
public class EC_CombatManager : EntityComponent
{
    public CombatState combatState;
    public GameEntity currentTarget;



}
