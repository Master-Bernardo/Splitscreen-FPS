﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField]
    Weapon[] inventory; //will be set up in inspector
    Weapon currentSelectedWeapon;
    int currentSelectedWeaponID;
    //public Text weaponInfo; //shows weapon name and ammo

    public Transform weaponHolder;

    [Header("Ammo")]
    //ammo in pockets - not in magazines

    Dictionary<AmmoType, int> ammo; //TODO this better
    public int startRocketAmmo;
    public int startGrenadeAmmo;
    public int startShockGrenadeAmmo;

    public WeaponHUD weaponHUD;

    //public Camera rayCastCamera; //fp camera
    //[Header("Animation")]
    //public Animator animator;


    enum WeaponSystemState
    {
        Default,
        Reloading,
        Changing
    }

    WeaponSystemState state = WeaponSystemState.Default;

    float reloadingEndTime;

    void Awake()
    {
        ammo = new Dictionary<AmmoType, int>();
    }

    void Start()
    {
        foreach (Weapon weapon in inventory)
        {
            if (weapon != null)
            {
                weapon.gameObject.SetActive(false);
                weapon.SetUp(this);
            }
            
        }
        if(weaponHUD!=null)weaponHUD.SetUp(this);
        ChangeWeapon(0);


        ammo[AmmoType.Rocket] = startRocketAmmo;
        ammo[AmmoType.Grenade] = startGrenadeAmmo;
        ammo[AmmoType.ShockGrenade] = startShockGrenadeAmmo;

    }

    public void UseWeaponStart(int actionID)
    {
        if(state == WeaponSystemState.Default)
        {
            if (currentSelectedWeapon != null)
            {
                currentSelectedWeapon.HandleWeaponKey(actionID);
            }
        }
       
    }

    public void UseWeaponHold(int actionID)
    {
        if (state == WeaponSystemState.Default)
        {
            if (currentSelectedWeapon != null)
            {
                if (currentSelectedWeapon.automaticTrigger)
                {
                    currentSelectedWeapon.HandleWeaponKey(actionID);
                }
            }
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

    // Update is called once per frame
    void Update()
    {
        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(2);
        }


        switch (state)
        {
           /* case WeaponSystemState.Default:

                if (currentSelectedWeapon != null)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentSelectedWeapon.HandleWeaponKey(0);

                    }
                    else
                    {
                        if (Input.GetMouseButton(0))
                        {
                            if (currentSelectedWeapon.automaticTrigger)
                            {
                                currentSelectedWeapon.HandleWeaponKey(0);
                            }
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.R))
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
               
                break;*/

            case WeaponSystemState.Reloading:

                //Debug.Log("Reloading");
                if (Time.time > reloadingEndTime)
                {
                    EndReload();
                }

                break;
        }

        //point the weapon to aim correctly
        /*if (currentSelectedWeapon is MissileWeapon)
        {
            //raycast
            Ray ray = rayCastCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //print("I'm looking at " + hit.transform.name);
                currentSelectedWeapon.transform.forward = hit.point - currentSelectedWeapon.transform.position;
            }
            else
            {
                currentSelectedWeapon.transform.forward = ray.direction;
            }
        }*/

        if (weaponHUD != null)
        {
            if (currentSelectedWeapon != null)
            {
                weaponHUD.UpdateHUD(currentSelectedWeapon);
            }
        }
    }

    void ChangeWeapon(int inventorySlot)
    {
        
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
        }

        currentSelectedWeaponID = inventorySlot;

        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        if (currentSelectedWeapon != null)
        {
            currentSelectedWeapon.gameObject.SetActive(true);
            currentSelectedWeapon.OnWeaponSelect();
        }
    }


    //returns the current selected weapon
    public Weapon SwapWeapon(Weapon newWeapon)
    {
        AbortReloading();

        Weapon oldWeapon = currentSelectedWeapon;
        if(oldWeapon != null)oldWeapon.OnWeaponDeselect();

        currentSelectedWeapon = newWeapon;
        currentSelectedWeapon.OnWeaponSelect();
        inventory[currentSelectedWeaponID] = currentSelectedWeapon;

        currentSelectedWeapon.transform.SetParent(weaponHolder.transform);
        currentSelectedWeapon.transform.localPosition = new Vector3(0, 0, 0);
        currentSelectedWeapon.transform.forward = weaponHolder.transform.forward;
        currentSelectedWeapon.SetUp(this);

        return oldWeapon;

    }

    public void StartReload()
    {
        state = WeaponSystemState.Reloading;
        reloadingEndTime = Time.time + (currentSelectedWeapon as MissileWeapon).reloadTime;
    }

    void AbortReloading()
    {
        state = WeaponSystemState.Default;
    }


    public void EndReload()
    {
        state = WeaponSystemState.Default;

        if (currentSelectedWeapon is MissileWeapon)
        {
            MissileWeapon currentMW = currentSelectedWeapon as MissileWeapon;
            if (currentMW.ammoType == AmmoType.Infinite)
            {
                currentMW.Reload(currentMW.magazineSize);
            }
            else
            {
                ammo[currentMW.ammoType] += currentMW.currentMagazineAmmo;
                int clamped = Mathf.Clamp(ammo[currentMW.ammoType], 0, currentMW.magazineSize);
                currentMW.Reload(clamped);
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
