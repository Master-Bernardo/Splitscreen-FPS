using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EC_WeaponSystem : EntityComponent
{
    [Header("Visualisation")]
    public AimVisualiser aimVisualiser;

    [Header("Weapons")]
    [SerializeField]
    Weapon[] inventory; //will be set up in inspector
    Weapon currentSelectedWeapon;
    int currentSelectedWeaponID;
    //public Text weaponInfo; //shows weapon name and ammo

    public Transform rightHand;

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

    //[Tooltip("select only one UI layer here")]
    //public GameObject playerUILayerGO;
    //public int playerUILayer;

    [Header("Starting Weapons")]
    public GameObject startingWeapon1;
    public GameObject startingWeapon2;
    public GameObject startingWeapon3;


    enum WeaponSystemState
    {
        Default,
        Reloading,
        Changing
    }

    WeaponSystemState state = WeaponSystemState.Default;

    float reloadingEndTime;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);


        ammo = new Dictionary<AmmoType, int>();

        ResetWeapons();

       /* foreach (Weapon weapon in inventory)
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
        ammo[AmmoType.ShockGrenade] = startShockGrenadeAmmo;*/

        //Debug.Log("len: " + SortingLayer.layers.Length);
        //playerUILayer = playerUILayerGO.layer
        //playerUILayer = SortingLayer.layers[playerUILayerID];
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

    public override void UpdateComponent()
    {
        currentSelectedWeapon = inventory[currentSelectedWeaponID];


        //if(currentSelectedWeapon!=null)currentSelectedWeapon.UpdateAimingLine();
        if (currentSelectedWeapon != null)
        {
            if (currentSelectedWeapon.usesAimingLine)
            {
                MissileWeapon mw = currentSelectedWeapon as MissileWeapon;
                aimVisualiser.DrawLine(mw.GetProjectileSpawnPoint(), mw.transform.forward, 15, mw.bloom);
            }
        }



            switch (state)
        {        
            case WeaponSystemState.Reloading:

                if (Time.time > reloadingEndTime)
                {
                    EndReload();
                }

                break;
        }


        if (weaponHUD != null)
        {
            weaponHUD.UpdateHUD(currentSelectedWeapon);
        }
    }

    public void ChangeWeapon(int inventorySlot)
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
            aimVisualiser.HideLine();

        }

        currentSelectedWeaponID = inventorySlot;

        currentSelectedWeapon = inventory[currentSelectedWeaponID];

        if (currentSelectedWeapon != null)
        {
            currentSelectedWeapon.gameObject.SetActive(true);
            currentSelectedWeapon.OnWeaponSelect(myEntity);
            aimVisualiser.ShowLine();


        }

    }

    public void SelectNextWeapon()
    {
        if (currentSelectedWeaponID == inventory.Length-1)
        {
            ChangeWeapon(0);
        }
        else
        {
            ChangeWeapon(currentSelectedWeaponID + 1);
        }
    }

    public void SelectPreviousWeapon()
    {
        if(currentSelectedWeaponID == 0)
        {
            ChangeWeapon(inventory.Length - 1);
        }
        else
        {
            ChangeWeapon(currentSelectedWeaponID - 1);
        }
    }


    //returns the current selected weapon
    public Weapon SwapWeapon(Weapon newWeapon)
    {
        AbortReloading();

        Weapon oldWeapon = currentSelectedWeapon;
        if (oldWeapon != null)
        {
            oldWeapon.OnWeaponDeselect();
            aimVisualiser.HideLine();
        }

        currentSelectedWeapon = newWeapon;
        currentSelectedWeapon.OnWeaponSelect(myEntity);
        aimVisualiser.ShowLine();

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

    //resetWeaponsBackToStartingWeapons
    public void ResetWeapons()
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



        foreach (Weapon weapon in inventory)
        {
            if (weapon != null)
            {
                weapon.teamID = myEntity.teamID;
                weapon.gameObject.SetActive(false);
                weapon.SetUp(this);
            }
        }
        if (weaponHUD != null) weaponHUD.SetUp(this);
        ChangeWeapon(0);

        ammo[AmmoType.Rocket] = startRocketAmmo;
        ammo[AmmoType.Grenade] = startGrenadeAmmo;
        ammo[AmmoType.ShockGrenade] = startShockGrenadeAmmo;
    }
}
