using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float damage; //could be expanded later to diffeent damages  
    public bool automaticTrigger;
    //public bool isEquipped = true;
    //public float ammoMultiplier=1;
    public int teamID;
    protected EC_WeaponSystem weaponSystem;
    public GameEntity weaponWieldingEntity;

    public void SetUp(EC_WeaponSystem weaponSystem)
    {
        this.weaponSystem = weaponSystem;
    }

    public virtual void OnWeaponSelect(GameEntity selectingEntity)
    {
        weaponWieldingEntity = selectingEntity;
    }

    public virtual void OnWeaponDeselect()
    {

    }

    public virtual void HandleWeaponKey(int weaponKey)
    {

    }

    /*public virtual void HandleLMBHold()
    {

    }*/

    /*public virtual void HandleRMBHold()
    {

    }*/

}
