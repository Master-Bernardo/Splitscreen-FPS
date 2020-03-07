using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        E_Unit unit = other.GetComponent<E_Unit>();

        if (unit != null)
        {
            unit.TakeDamage(new DamageInfo(9999999999999));
        }
    }
}
