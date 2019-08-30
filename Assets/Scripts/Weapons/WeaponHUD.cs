using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    public Text weaponName;
    public Text currentAmmo;
    public Text ammoLeftInPoaches;

    public Image infinityPoach;
    public Image infinityPoachAndMagazine;
    public Text line;

    public GameObject reloadingPanel;

    WeaponSystem weaponSystem;

    public void SetUp(WeaponSystem weaponSystem)
    {
        this.weaponSystem = weaponSystem;
    }

    public void UpdateHUD(Weapon weapon)
    {
        MissileWeapon mw = weapon as MissileWeapon;

        weaponName.text = weapon.name;

        if (weaponSystem.IsReloading())
        {
            reloadingPanel.SetActive(true);
        }
        else
        {
            reloadingPanel.SetActive(false);

        }

        if (mw != null)
        {
            if(mw.ammoType == AmmoType.Infinite)
            {
                if (mw.infiniteMagazine)
                {
                    currentAmmo.text = "";
                    ammoLeftInPoaches.text = "";
                    infinityPoach.enabled = false;
                    infinityPoachAndMagazine.enabled = true;
                    line.enabled = false;
                }
                else
                {
                    currentAmmo.text = mw.currentMagazineAmmo.ToString();
                    ammoLeftInPoaches.text = "";
                    infinityPoach.enabled = true;
                    infinityPoachAndMagazine.enabled = false;
                    line.enabled = true;

                }
            }
            else
            {
                currentAmmo.text = mw.currentMagazineAmmo.ToString();
                ammoLeftInPoaches.text = weaponSystem.GetAmmo(mw.ammoType).ToString();
                infinityPoach.enabled = false;
                infinityPoachAndMagazine.enabled = false;
                line.enabled = true;

            }

        }
        else
        {
            currentAmmo.text = "";
            ammoLeftInPoaches.text = "";
            infinityPoach.enabled = false;
            infinityPoachAndMagazine.enabled = false;
            line.enabled = true;

        }
    }
}
