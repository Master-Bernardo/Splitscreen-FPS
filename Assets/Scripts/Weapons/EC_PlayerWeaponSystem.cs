using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EC_PlayerWeaponSystem : EC_HumanWeaponSystem
{
    [Header("Visualisation")]
    public AimVisualiser aimVisualiser;
    public Vector3 aimLineStartPointOffset;

    public WeaponHUD weaponHUD;

    [Header("Starting Weapons")]
    public GameObject startingWeapon1;
    public GameObject startingWeapon2;
    public GameObject startingWeapon3;



    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);
        ResetWeaponsToStartingWeapons();
       
        if (weaponHUD != null) weaponHUD.SetUp(this);
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        // if (currentSelectedWeapon != null)
        //{
        //if (currentSelectedWeapon.usesAimingLine)
        //{
        /*for special weapon dependant cone
        //MissileWeapon mw = currentSelectedWeapon as MissileWeapon;
        //aimVisualiser.DrawCone(mw.GetProjectileSpawnPoint(), mw.transform.forward, 15, mw.bloom);
        */

        // aimVisualiser.DrawLine(myEntity.transform.TransformPoint(aimLineStartPointOffset), myEntity.transform.forward, 15);
        //}
        //}
        aimVisualiser.DrawLine(myEntity.transform.TransformPoint(aimLineStartPointOffset), myEntity.transform.forward, 15);

        if (weaponHUD != null)
        {
            weaponHUD.UpdateHUD(currentSelectedWeapon);
        }
    }

    public override void ChangeWeapon(int inventorySlot)
    {
        base.ChangeWeapon(inventorySlot);

        if (currentSelectedWeapon)
        {
            if (!currentSelectedWeapon.usesAimingLine)
            {
                aimVisualiser.HideLine();
            }
            else if (previousWeapon)
            {
                if (currentSelectedWeapon.usesAimingLine && !previousWeapon.usesAimingLine)
                {
                    aimVisualiser.ShowLine();
                }
            }
            else
            {
                if (currentSelectedWeapon.usesAimingLine)
                {
                    aimVisualiser.ShowLine();
                }
            }

        }
       
    }

    #region UseWeapon
    public void UseWeaponStart(int actionID)
    {
        if(state == WeaponSystemState.Default)
        {
            if (currentSelectedWeapon != null)
            {
                if(currentSelectedWeapon is MeleeWeapon)
                {
                    if(actionID == 0)
                    {
                        meleeWeaponControler.MeleeAttack();
                    }
                    else if (actionID == 1)
                    {
                        meleeWeaponControler.AbortMeleeAttack();
                    }
                   
                }
                else
                {
                    currentSelectedWeapon.HandleWeaponKeyDown(actionID);
                }
            }
        }
       
    }

    public void UseWeaponHold(int actionID)
    {
        if (state == WeaponSystemState.Default)
        {
            if (currentSelectedWeapon != null)
            {
                 currentSelectedWeapon.HandleWeaponKeyHold(actionID);
            }
        }
    }

    public void UseWeaponEnd(int actionID)
    {
        if (state == WeaponSystemState.Default)
        {
            if (currentSelectedWeapon != null)
            {
                 currentSelectedWeapon.HandleWeaponKeyUp(actionID);
            }
        }
    }

    #endregion

    #region SelectingWeapons

    public void SelectNextWeapon()
    {
        bool nextWeaponFound = false;

        int iterator = currentSelectedWeaponID + 1;
        while (!nextWeaponFound)
        {
            if (iterator == inventory.Length) iterator = 0;
            if (inventory[iterator])
            {
                if (currentSelectedWeaponID != iterator)
                {
                    ChangeWeapon(iterator);
                    nextWeaponFound = true;
                }
                else
                {
                    nextWeaponFound = true;
                }

            }
            iterator++;
        }
    }

    public void SelectPreviousWeapon()
    {
        bool previousWeaponFound = false;

        int iterator = currentSelectedWeaponID - 1;
        while (!previousWeaponFound)
        {
            if (iterator == -1) iterator = inventory.Length-1;
            if (inventory[iterator])
            {
                if (currentSelectedWeaponID != iterator)
                {
                    ChangeWeapon(iterator);
                    previousWeaponFound = true;
                }
                else
                {
                    previousWeaponFound = true;
                }

            }
            iterator--;
        }
    }

    #endregion

    //resets all weapons and ammo to player starting weapons and ammo
    public void ResetWeaponsToStartingWeapons()
    {
        //first delete all weapons currently there
        foreach(Transform transform in rightHand)
        {
            Destroy(transform.gameObject);
        }


        GameObject weapon1;
        GameObject weapon2;
        GameObject weapon3;


        if (startingWeapon1 != null)
        {
            weapon1 = Instantiate(startingWeapon1, rightHand);
            inventory[0] = weapon1.GetComponent<Weapon>();
        }
        if (startingWeapon2 != null)
        {
            weapon2 = Instantiate(startingWeapon2, rightHand);
            inventory[1] = weapon2.GetComponent<Weapon>();
        }
        if (startingWeapon3 != null)
        {
            weapon3 = Instantiate(startingWeapon3, rightHand);
            inventory[2] = weapon3.GetComponent<Weapon>();
        }
        currentSelectedWeaponID = 0;

        SetUpWeaponsAndAmmo();
    }

    //just changes weapon again? for animation
    public void ResetWeaponsRespawn()
    {
        ChangeWeapon(currentSelectedWeaponID);
        /*foreach (Weapon weapon in inventory)
        {
            MissileWeapon mw = weapon as MissileWeapon;
            if (mw)
            {
                mw.Reset();
            }
        }*/


    }

    public void SetCurrentWeaponToNull()
    {
        currentSelectedWeapon = null;
    }

    
}
