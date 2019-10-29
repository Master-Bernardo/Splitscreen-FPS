using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EC_PlayerWeaponSystem : EC_WeaponSystem
{
    [Header("Visualisation")]
    public AimVisualiser aimVisualiser;

    /*[Header("Weapons")]
    [SerializeField]
    Weapon[] inventory; //will be set up in inspector
    Weapon currentSelectedWeapon;
    int currentSelectedWeaponID;

    public Transform rightHand;*/

    /*[Header("Ammo")]
    //ammo in pockets - not in magazines

    Dictionary<AmmoType, int> ammo; //TODO this better
    public int startRocketAmmo;
    public int startGrenadeAmmo;
    public int startShockGrenadeAmmo;*/

    public WeaponHUD weaponHUD;

  

    [Header("Starting Weapons")]
    public GameObject startingWeapon1;
    public GameObject startingWeapon2;
    public GameObject startingWeapon3;


    //public Camera rayCastCamera; //fp camera
    //[Header("Animation")]
    //public Animator animator;

    //[Tooltip("select only one UI layer here")]
    //public GameObject playerUILayerGO;
    //public int playerUILayer;


    /*enum WeaponSystemState
    {
        Default,
        Reloading,
        Changing
    }

    WeaponSystemState state = WeaponSystemState.Default;

    float reloadingEndTime;*/

    public override void SetUpComponent(GameEntity entity)
    {
        ResetWeapons();
        base.SetUpComponent(entity);


        //ammo = new Dictionary<AmmoType, int>();

       
        if (weaponHUD != null) weaponHUD.SetUp(this);
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
                currentSelectedWeapon.HandleWeaponKeyDown(actionID);
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

    public override void ChangeWeapon(int inventorySlot)
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
        if (currentSelectedWeaponID == inventory.Length - 1)
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
        if (currentSelectedWeaponID == 0)
        {
            ChangeWeapon(inventory.Length - 1);
        }
        else
        {
            ChangeWeapon(currentSelectedWeaponID - 1);
        }
    }

    //returns the current selected weapon
    public override Weapon SwapWeapon(Weapon newWeapon)
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


    public override void UpdateComponent()
    {
        base.UpdateComponent();

        if (currentSelectedWeapon != null)
        {
            if (currentSelectedWeapon.usesAimingLine)
            {
                MissileWeapon mw = currentSelectedWeapon as MissileWeapon;
                aimVisualiser.DrawLine(mw.GetProjectileSpawnPoint(), mw.transform.forward, 15, mw.bloom);
            }
        }


        if (weaponHUD != null)
        {
            weaponHUD.UpdateHUD(currentSelectedWeapon);
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

 
    }

    
}
