using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MeleeAttackSet", menuName = "SO_MeleeAttackSet")]
public class SO_MeleeAttackSet : ScriptableObject
{
    public string setName;
    public SO_MeleeAttack[] attacks;
}