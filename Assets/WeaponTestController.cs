using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTestController : MonoBehaviour
{
    public MissileWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.HandleLMBDown();

        }
        {

            if (Input.GetMouseButton(0))
            {
                weapon.HandleLMBHold();
            }
        }
    }
   
}
