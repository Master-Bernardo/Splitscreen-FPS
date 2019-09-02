using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string unitName;
    public string description;
    public int recruitingPoints; // how long is the recruiting Process?
    public int healthPoints;
    public int populationValue;

    public int cost;

    [Tooltip("the image which gets shown in the UI")]
    public Sprite menuImage;
    public GameObject prefab;
}
