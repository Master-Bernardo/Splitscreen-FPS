using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_HumanWeaponSystem : EntityComponent
{
    #region Fields

    [Header("Weapons")]
    [SerializeField]
    protected Weapon[] inventory; //will be set up in inspector
    protected Weapon currentSelectedWeapon;
    protected int currentSelectedWeaponID;
    public EC_MeleeWeaponController meleeWeaponControler;
    public EC_MissileWeaponController missileWeaponController; //todo also needs to be set up during weapon Change
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
    Weapon weaponToHide; //sometimes the weapon we need to hide is not the previous weapon, because we switched weapons to fast
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

    #endregion



    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);


        ammo = new Dictionary<AmmoType, int>();

        //ResetWeapons();
        SetUpWeaponsAndAmmo();
    }

    public override void UpdateComponent()
    {
        switch (state)
        {
            case WeaponSystemState.Reloading:

                if (Time.time > reloadingEndTime)
                {
                    EndReloading();
                }

                break;

            case WeaponSystemState.HidingWeapon:
               // Debug.Log("HidingWeapon");
                if (Time.time > hideWeaponEndTime)
                {   
                    EndHidingWeapon(weaponToHide);
                    StartDrawingWeaponWeapon(currentSelectedWeapon);
                }

                break;

            case WeaponSystemState.DrawingWeapon:
               // Debug.Log("Drawing epaon------------------------");
                if (Time.time > drawWeaponEndTime)
                {
                    state = WeaponSystemState.Default;
                    //Debug.Log("Weapon Draw Completed ..+......+...........+............+..........+............");
                }

                break;
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

    #region ChangingWeapons

    public virtual void ChangeWeapon(int inventorySlot)
    {
        Debug.Log("changing to slot : " + inventorySlot);
        if(currentSelectedWeapon != inventory[inventorySlot])
        {
            Weapon newWeapon = inventory[inventorySlot];

            if (state == WeaponSystemState.Reloading)
            {
                Debug.Log("Change while reloading");
                AbortReloading();

                StartHidingWeapon(currentSelectedWeapon);
            }
            else if(state == WeaponSystemState.HidingWeapon)
            {
                //previousWeapon.gameObject.SetActive(false);
                //previousWeapon.OnWeaponDeselect();
                //weaponToHide = currentSelectedWeapon;
                Debug.Log("changed while hiding");
                //if we are in the hiding process, we continue it , without restarting it - so do nothing
            }
            else if(state == WeaponSystemState.DrawingWeapon)
            {
                Debug.Log("changed while drawing");
                //if we are currently in the drawing process, we just restart the drawing process, without repeating the hiding process
                EndHidingWeapon(currentSelectedWeapon);
                StartDrawingWeaponWeapon(newWeapon);
            }
            else if(state == WeaponSystemState.Default)
            {
                Debug.Log("changed default");

                if (currentSelectedWeapon != null)
                {
                    // Debug.Log("current Selected: " + previousWeapon);
                    if (meleeWeaponControler)
                    {
                        if (meleeWeaponControler.meleeAttackInitiated)
                        {
                            meleeWeaponControler.AbortMeleeAttack();
                        }
                    }

                    StartHidingWeapon(currentSelectedWeapon);

                }
                else
                {
                   // Debug.Log("current weapon was null pon changeWeapon");

                    StartDrawingWeaponWeapon(newWeapon);
                }
            }

            previousWeapon = currentSelectedWeapon;
            currentSelectedWeaponID = inventorySlot;
            currentSelectedWeapon = newWeapon;
        }
    }

    protected virtual void StartDrawingWeaponWeapon(Weapon weaponToDraw)
    {     
        state = WeaponSystemState.DrawingWeapon;
        drawWeaponEndTime = Time.time + drawOrHideTime;

        if (weaponToDraw != null)
        {
            weaponToDraw.gameObject.SetActive(true);
            weaponToDraw.OnWeaponSelect(myEntity);
            if(weaponToDraw is MeleeWeapon)
            {
                meleeWeaponControler.SetWeapon(weaponToDraw as MeleeWeapon);
            }
        }
        if (handsAnimator != null)
        {
            if (weaponToDraw != null)
            {
                //Debug.Log("draw weapon + " + weaponToDraw.stanceAnimationTypeID.ToString());
                handsAnimator.SetTrigger("DrawWeapon" + weaponToDraw.stanceAnimationTypeID);
            }
            else
            {
                handsAnimator.SetTrigger("DrawWeapon" + 0);
            }
            //Debug.Log("trigger draw weaponset");
        }

    }

    protected void StartHidingWeapon(Weapon weapon)
    {
        this.weaponToHide = weapon;
        state = WeaponSystemState.HidingWeapon;
        hideWeaponEndTime = Time.time + drawOrHideTime;
        //previousWeapon = weaponToHide;

        if (handsAnimator != null)
        {
            handsAnimator.SetTrigger("HideWeapon");
        }
    }

    protected void EndHidingWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            weapon.OnWeaponDeselect();
            weapon.gameObject.SetActive(false);
        }    
    }

    //returns the current selected weapon
    public virtual Weapon SwapWeapon(Weapon newWeapon)
    {
        if (state == WeaponSystemState.Reloading)
        {
            AbortReloading();
        }

        previousWeapon = currentSelectedWeapon;
        if (previousWeapon != null)
        {
            previousWeapon.OnWeaponDeselect();
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

        StartDrawingWeaponWeapon(currentSelectedWeapon);

        return previousWeapon;

    }

    #endregion

    #region Reloading

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
                        StartReloading();
                    }
                }
            }
        }
    }

    public void StartReloading()
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

    public void EndReloading()
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

    #endregion

    #region Getts n Setters

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

    #endregion
}
