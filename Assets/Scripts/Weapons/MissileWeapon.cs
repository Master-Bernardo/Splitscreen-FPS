using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    [SerializeField]
    protected Transform shootPoint; //point from which the projectiles are being shot
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
    public string projectileTag;
    public float initialLaunchSpeed;

    public AmmoType ammoType;

    private void Start()
    {
        Reset();
        nextShootTime = 0;
        shootingInterval = 1 / fireRate;
        nextShootTime = Time.time + Random.Range(0, shootingInterval);
    }


    protected virtual void Shoot()
    {
        //Debug.Log("piu piu");

        //Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
        Projectile projectile = ProjectilePooler.Instance.SpawnFromPool(projectileTag, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
        projectile.SetVelocity(initialLaunchSpeed);
        projectile.projectileTeamID = teamID;
        projectile.damage = damage;
        projectile.shooterEntity = weaponWieldingEntity;
        if(!infiniteMagazine)currentMagazineAmmo--;
    }


    public override void HandleWeaponKey(int weaponKey)
    {
        if (currentMagazineAmmo > 0)
        {
            if (Time.time > nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + shootingInterval;
                if (currentMagazineAmmo == 0)
                {
                    if(weaponSystem!=null)weaponSystem.StartReload();
                }
            }
        } 
    }


    public Vector3 GetProjectileSpawnPoint()
    {
        return shootPoint.position;
    }

    /*public float GetInitialLaunchSpeed()
    {
        initialLaunchSpeed
    }*/

    public virtual void Reload(int value)
    {
        currentMagazineAmmo = value;
    }

    public void Reload()
    {
        currentMagazineAmmo = magazineSize;
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
