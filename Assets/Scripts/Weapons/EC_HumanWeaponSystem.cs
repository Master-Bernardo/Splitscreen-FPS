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
    Weapon previousWeapon;
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
                Debug.Log("HidingWeapon");
                if (Time.time > hideWeaponEndTime)
                {   
                    EndHideWeapon();
                    DrawWeapon(currentSelectedWeapon);
                }

                break;

            case WeaponSystemState.DrawingWeapon:
                Debug.Log("Drawing epaon------------------------");
                if (Time.time > drawWeaponEndTime)
                {
                    state = WeaponSystemState.Default;
                    Debug.Log("Weapon Draw Completed ..+......+...........+............+..........+............");
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
        if(currentSelectedWeapon != inventory[inventorySlot])
        {
            previousWeapon = currentSelectedWeapon;
            currentSelectedWeaponID = inventorySlot;
            currentSelectedWeapon = inventory[currentSelectedWeaponID];


            if (state == WeaponSystemState.Reloading)
            {
                AbortReloading();
            }
            else if(state == WeaponSystemState.HidingWeapon)
            {
                //if we are in the hiding process, we continue it , without restarting it - so do nothing
            }
            else if(state == WeaponSystemState.DrawingWeapon)
            {
                //if we are currently in the drawing process, we just restart the drawing process, without repeating the hiding process
                EndHideWeapon();
                DrawWeapon(currentSelectedWeapon);
            }
            else if(state == WeaponSystemState.Default)
            {
                if (previousWeapon != null)
                {
                   // Debug.Log("current Selected: " + previousWeapon);
                    HideWeapon(previousWeapon);

                }
                else
                {
                   // Debug.Log("current weapon was null pon changeWeapon");

                    DrawWeapon(currentSelectedWeapon);
                }
            }      
        }
    }

    void DrawWeapon(Weapon weaponToDraw)
    {
        state = WeaponSystemState.DrawingWeapon;
        drawWeaponEndTime = Time.time + drawOrHideTime;

        weaponToDraw.gameObject.SetActive(true);
        weaponToDraw.OnWeaponSelect(myEntity);

        if (handsAnimator != null)
        {
            handsAnimator.SetTrigger("DrawWeapon");
            Debug.Log("trigger draw weaponset");
        }

    }

    void HideWeapon(Weapon weaponToHide)
    {
        state = WeaponSystemState.HidingWeapon;
        hideWeaponEndTime = Time.time + drawOrHideTime;
        previousWeapon = weaponToHide;

        if (handsAnimator != null)
        {
            handsAnimator.SetTrigger("HideWeapon");
        }
    }

    void EndHideWeapon()
    {
        previousWeapon.OnWeaponDeselect();
        previousWeapon.gameObject.SetActive(false);
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
