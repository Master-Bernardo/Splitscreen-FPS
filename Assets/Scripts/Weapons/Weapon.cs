using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage; //could be expanded later to diffeent damages  
    public bool isEquipped = true;
    //public float ammoMultiplier=1;
    public int teamID;
    protected WeaponSystem weaponSystem;

    public void SetUp(WeaponSystem weaponSystem)
    {
        this.weaponSystem = weaponSystem;
    }

    public virtual void HandleLMBDown()
    {

    }

    public virtual void HandleLMBHold()
    {

    }

    /*public virtual void HandleRMBHold()
    {

    }*/

}
