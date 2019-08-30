using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    
    [SerializeField]
    protected Transform shootPoint; //point from which the projectiles are being shot
    public bool automaticTrigger;
    [Tooltip("in rounds per second")]
    public float fireRate;
    public float reloadTime;
    public int magazineSize;
    public bool infiniteMagazine;
    public float bloom;
    [SerializeField]
    public int currentMagazineAmmo;

    float nextShootTime;
    float shootingInterval;

    [SerializeField]
    protected GameObject projectilePrefab; //later only the children have this
    public float initialLaunchSpeed;

    public AmmoType ammoType;

    private void Start()
    {
        Reset();
        nextShootTime = 0;
        shootingInterval = 1 / fireRate;
    }


    public virtual void Shoot()
    {
        //Debug.Log("piu piu");

        Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
        projectile.startVelocity = initialLaunchSpeed;
        projectile.projectileTeamID = teamID;
        projectile.damage = damage;
        if(!infiniteMagazine)currentMagazineAmmo--;
    }

    public override void HandleLMBDown()
    {
        if (currentMagazineAmmo > 0)
        {
            if (Time.time > nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + shootingInterval;
                if (currentMagazineAmmo == 0)
                {
                    weaponSystem.StartReload();
                }
            }
        } 
    }

    public override void HandleLMBHold()
    {
        if (automaticTrigger)
        {
            if (currentMagazineAmmo > 0)
            {
                if (Time.time > nextShootTime)
                {
                    Shoot();
                    nextShootTime = Time.time + shootingInterval;
                    if (currentMagazineAmmo == 0)
                    {
                        weaponSystem.StartReload();
                    }
                }
            }
        }
    }

    public virtual void Reload(int value)
    {
        currentMagazineAmmo = value;
    }

    /*public int GetCurrentMagazineAmmo()
    {
        return currentMagazineAmmo;
    }*/

    public void Reset()
    {
        currentMagazineAmmo = magazineSize;
    }

    public bool IsMagazineFull()
    {
        if (currentMagazineAmmo == magazineSize) return true;
        else return false;
    }
}
