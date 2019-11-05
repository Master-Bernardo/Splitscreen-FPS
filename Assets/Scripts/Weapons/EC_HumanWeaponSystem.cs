using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HumanWeaponSystem : EntityComponent
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

    [Header("Animation")]
    public Animator handsAnimator;

    [Header("SwitchWeapon time")]
    [Tooltip("this value times 2 is the time it takes to change a weapon completely")]
    public float drawOrHideTime = 0.35f;
    Weapon weaponToHide;
    float hideWeaponEndTime;
    float drawWeaponEndTime;

    protected enum WeaponSystemState
    {
        Default,
        Reloading,
        HidingWeapon,
        DrawingWeapon
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
        //currentSelectedWeapon = inventory[currentSelectedWeaponID];

        switch (state)
        {
            case WeaponSystemState.Reloading:

                if (Time.time > reloadingEndTime)
                {
                    EndReload();
                }

                break;

            case WeaponSystemState.HidingWeapon:

                if (Time.time > hideWeaponEndTime)
                {
                    state = WeaponSystemState.DrawingWeapon;
                    drawWeaponEndTime = Time.time + drawOrHideTime;
                    weaponToHide.gameObject.SetActive(false);

                    if (currentSelectedWeapon != null)
                    {
                        currentSelectedWeapon.gameObject.SetActive(true);
                    }
                    if (handsAnimator != null)
                    {
                        handsAnimator.SetTrigger("DrawWeapon");
                    }
                }

                break;

            case WeaponSystemState.DrawingWeapon:

                if (Time.time > drawWeaponEndTime)
                {
                    state = WeaponSystemState.Default;
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
        
       // ChangeWeapon(0);

        ammo[AmmoType.Rocket] = startRocketAmmo;
        ammo[AmmoType.Grenade] = startGrenadeAmmo;
        ammo[AmmoType.ShockGrenade] = startShockGrenadeAmmo;
    }

    public virtual void ChangeWeapon(int inventorySlot)
    {

        Debug.Log("changeWeapon");
        //animator.SetTrigger("changeWeapon");
        //animator.SetBool("reloading", false);
        if (state == WeaponSystemState.Reloading)
        {
            AbortReloading();
        }

        if (currentSelectedWeapon != null)
        {
            Debug.Log("current Selected: " + currentSelectedWeapon);
            //currentSelectedWeapon.gameObject.SetActive(false);
            currentSelectedWeapon.OnWeaponDeselect();
            state = WeaponSystemState.HidingWeapon;
            hideWeaponEndTime = Time.time + drawOrHideTime;
            weaponToHide = currentSelectedWeapon;

            if (handsAnimator != null)
            {
                handsAnimator.SetTrigger("HideWeapon");
            }
        }
        else
        {
            Debug.Log("current weapon was null pon changeWeapon");
            state = WeaponSystemState.DrawingWeapon;
            drawWeaponEndTime = Time.time + drawOrHideTime;
            if (handsAnimator != null)
            {
                handsAnimator.SetTrigger("DrawWeapon");
                Debug.Log("trigger draw weaponset");
            }
        }

        currentSelectedWeaponID = inventorySlot;

        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        if (currentSelectedWeapon != null)
        {
            //currentSelectedWeapon.gameObject.SetActive(true);
            if(state == WeaponSystemState.DrawingWeapon)
            {
                currentSelectedWeapon.gameObject.SetActive(true);
            }
            currentSelectedWeapon.OnWeaponSelect(myEntity);
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
        if (handsAnimator != null)
        {
            handsAnimator.SetTrigger("Reload");
        }
    }

    protected void AbortReloading()
    {
        state = WeaponSystemState.Default;
        MissileWeapon currentMW = currentSelectedWeapon as MissileWeapon;
        currentMW.AbortReloading();
        if (handsAnimator != null)
        {
            handsAnimator.SetTrigger("AbortReload");
        }
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

    public bool CanShoot()
    {
        if(state == WeaponSystemState.Default)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
