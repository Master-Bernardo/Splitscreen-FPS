using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_WeaponSystem : EntityComponent
{
    [Header("Weapons")]
    [SerializeField]
    protected Weapon[] inventory; //will be set up in inspector
    protected Weapon currentSelectedWeapon;
    protected int currentSelectedWeaponID;

    public Transform rightHand;

    [Header("Ammo")]
    //ammo in pockets - not in magazines

    protected Dictionary<AmmoType, int> ammo; //TODO this better
    public int startRocketAmmo;
    public int startGrenadeAmmo;
    public int startShockGrenadeAmmo;

    protected enum WeaponSystemState
    {
        Default,
        Reloading,
        Changing
    }

    protected WeaponSystemState state = WeaponSystemState.Default;

    protected float reloadingEndTime;


    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);


        ammo = new Dictionary<AmmoType, int>();

        //ResetWeapons();
        SetUpWeaponsAndAmmo();
    }


    public override void UpdateComponent()
    {
        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        switch (state)
        {
            case WeaponSystemState.Reloading:

                if (Time.time > reloadingEndTime)
                {
                    EndReload();
                }

                break;
        }



    }

    public void ReloadWeapon()
    {
        if (state == WeaponSystemState.Default)
        {
            MissileWeapon mw = currentSelectedWeapon as MissileWeapon;
            if (mw != null)
            {
                if (!mw.infiniteMagazine)
                {
                    if (!mw.IsMagazineFull())
                    {
                        StartReload();
                    }
                }
            }
        }
    }

    void SetUpWeaponsAndAmmo()
    {
        foreach (Weapon weapon in inventory)
        {
            if (weapon != null)
            {
                weapon.teamID = myEntity.teamID;
                weapon.gameObject.SetActive(false);
                weapon.SetUp(this);
            }
        }
        
        ChangeWeapon(0);

        ammo[AmmoType.Rocket] = startRocketAmmo;
        ammo[AmmoType.Grenade] = startGrenadeAmmo;
        ammo[AmmoType.ShockGrenade] = startShockGrenadeAmmo;
    }

    public virtual void ChangeWeapon(int inventorySlot)
    {
        //Debug.Log("changeWeapon");
        //animator.SetTrigger("changeWeapon");
        //animator.SetBool("reloading", false);
        if (state == WeaponSystemState.Reloading)
        {
            AbortReloading();
        }

        if (currentSelectedWeapon != null)
        {
            currentSelectedWeapon.gameObject.SetActive(false);
            currentSelectedWeapon.OnWeaponDeselect();
            //aimVisualiser.HideLine();

        }

        currentSelectedWeaponID = inventorySlot;

        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        if (currentSelectedWeapon != null)
        {
            currentSelectedWeapon.gameObject.SetActive(true);
            currentSelectedWeapon.OnWeaponSelect(myEntity);
            //aimVisualiser.ShowLine();
            Debug.Log("weaponSyelected");

        }

    }



    //returns the current selected weapon
    public virtual Weapon SwapWeapon(Weapon newWeapon)
    {
        AbortReloading();

        Weapon oldWeapon = currentSelectedWeapon;
        if (oldWeapon != null)
        {
            oldWeapon.OnWeaponDeselect();
            //aimVisualiser.HideLine();
        }

        currentSelectedWeapon = newWeapon;
        currentSelectedWeapon.OnWeaponSelect(myEntity);
        //aimVisualiser.ShowLine();

        inventory[currentSelectedWeaponID] = currentSelectedWeapon;

        currentSelectedWeapon.transform.SetParent(rightHand.transform);
        currentSelectedWeapon.transform.localPosition = new Vector3(0, 0, 0);
        currentSelectedWeapon.transform.forward = rightHand.transform.forward;
        currentSelectedWeapon.SetUp(this);
        currentSelectedWeapon.teamID = myEntity.teamID;

        return oldWeapon;

    }

    public void StartReload()
    {
        state = WeaponSystemState.Reloading;
        MissileWeapon currentMW = currentSelectedWeapon as MissileWeapon;
        reloadingEndTime = Time.time + currentMW.reloadTime;
        currentMW.StartReloading();
    }

    protected void AbortReloading()
    {
        state = WeaponSystemState.Default;
        MissileWeapon currentMW = currentSelectedWeapon as MissileWeapon;
        currentMW.AbortReloading();
    }


    public void EndReload()
    {
        state = WeaponSystemState.Default;

        if (currentSelectedWeapon is MissileWeapon)
        {
            MissileWeapon currentMW = currentSelectedWeapon as MissileWeapon;
            if (currentMW.ammoType == AmmoType.Infinite)
            {
                currentMW.EndReloading(currentMW.magazineSize);
            }
            else
            {
                ammo[currentMW.ammoType] += currentMW.currentMagazineAmmo;
                int clamped = Mathf.Clamp(ammo[currentMW.ammoType], 0, currentMW.magazineSize);
                currentMW.EndReloading(clamped);
                ammo[currentMW.ammoType] -= clamped;
            }
        }
    }



    //gets called by the weapons
    public int GetAmmo(AmmoType ammoType)
    {
        return ammo[ammoType];
    }

    //gets called by the weapons
    public void SetAmmo(AmmoType ammoType, int value)
    {
        ammo[ammoType] = value;
    }

    public void AddAmmo(AmmoType ammoType, int value)
    {
        //ammo[ammoType] += (int)Mathf.Round(value * currentSelectedWeapon.ammoMultiplier);
        ammo[ammoType] += value;
    }

    public bool IsReloading()
    {
        if (state == WeaponSystemState.Reloading)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
