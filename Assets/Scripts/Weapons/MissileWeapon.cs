using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    [Header("Missile Wapon")]
    [SerializeField]
    protected Transform shootPoint; //point from which the projectiles are being shot
    [Tooltip("in rounds per second")]
    public float fireRate;
    public float bloom;
    [Tooltip("how many bullets will be spawned on one shot - usefull for shotguns")]
    public float bulletNumber = 1;
    public float reloadTime;
    public float initialLaunchSpeed;
    [Header("Ammo")]
    [SerializeField]
    public AmmoType ammoType;
    public string projectileTag;
    public int magazineSize;
    public bool infiniteMagazine;
    [SerializeField]
    public int currentMagazineAmmo;

    float nextShootTime;
    float shootingInterval;

    [Header("Sound")]
    public AudioSourceCustom customAudioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    [Tooltip("the reload sound is a bit delayed to allow the shooting sound to be heared decay more naturally")]
    public float reloadSoundDelay;

    //if we have a machine gun - how to do the logic where the start loop middle and end start
    [Tooltip("Splitting sounds is essential for a machine gun, it will sound strange without it")]
    public bool splitSounds; //hmm how to do this
    bool loppingMiddleSound = false; // is the miidle sound currently being looped
    public AudioClip shootSoundStart;
    public AudioClip shootSoundMid;
    public AudioClip shootSoundEnd;

    





    private void Start()
    {
        Reset();
        nextShootTime = 0;
        shootingInterval = 1 / fireRate;
        nextShootTime = Time.time + Random.Range(0, shootingInterval);
    }


    protected virtual void Shoot()
    {
            //Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
        for (int i = 0; i < bulletNumber; i++)
        {
            Projectile projectile = ProjectilePooler.Instance.SpawnFromPool(projectileTag, shootPoint.position, shootPoint.rotation * Quaternion.Euler(Random.Range(-bloom, bloom), Random.Range(-bloom, bloom), 0f)).GetComponent<Projectile>();
            projectile.Activate(initialLaunchSpeed, teamID, damage, weaponWieldingEntity);
        }
        
        if(!infiniteMagazine)currentMagazineAmmo--;
    }

    public override void HandleWeaponKeyDown(int weaponKey)
    {
        if (currentMagazineAmmo > 0)
        {
            if (Time.time > nextShootTime)
            {
                Shoot();

                //audio
                if (customAudioSource != null)
                {
                    if (!splitSounds)
                    {
                        customAudioSource.SetSound(shootSound);
                        customAudioSource.Play();
                    }
                    else
                    {
                        customAudioSource.SetSound(shootSoundStart);
                        customAudioSource.Play();
                    }
                }
               



                nextShootTime = Time.time + shootingInterval;
                if (currentMagazineAmmo == 0)
                {
                    if (weaponSystem != null) weaponSystem.StartReload();
                }
            }
        }
    }

   

    public override void HandleWeaponKeyHold(int weaponKey)
    {
        //here we should check if automatic trigger, not in weapon controller TODO
        if (automaticTrigger)
        {
            if (currentMagazineAmmo > 0)
            {
                if (Time.time > nextShootTime)
                {
                    Shoot();

                    if (customAudioSource != null)
                    {
                        if (splitSounds)
                        {
                            if (!loppingMiddleSound)
                            {
                                customAudioSource.SetSound(shootSoundMid);
                                customAudioSource.Play();
                                customAudioSource.SetLoop(true);
                            }
                        }
                        else
                        {
                            customAudioSource.SetSound(shootSound);
                            customAudioSource.Play();
                        }
                    }
                    



                    nextShootTime = Time.time + shootingInterval;
                    if (currentMagazineAmmo == 0)
                    {
                        if (weaponSystem != null)
                        {
                            weaponSystem.StartReload();

                            if (customAudioSource != null)
                            {
                                if (splitSounds)
                                {
                                    customAudioSource.SetLoop(false);
                                    customAudioSource.SetSound(shootSoundEnd);
                                    customAudioSource.Play();
                                }
                            }
                        }
                    }
                }
            }
        }

       
    }

    public override void HandleWeaponKeyUp(int weaponKey)
    {
        if (customAudioSource != null)
        {
            if (splitSounds)
            {
                customAudioSource.SetLoop(false);
                customAudioSource.SetSound(shootSoundEnd);
                customAudioSource.Play();
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

    //only for audio so far
    public virtual void StartReloading()
    {
        Debug.Log("startReload");
        if (splitSounds)
        {
            customAudioSource.SetLoop(false);
            customAudioSource.SetSound(shootSoundEnd);
            customAudioSource.Play();
        }
        Invoke("PlayStartReloadSoundDelayed", reloadSoundDelay);
    }

    void PlayStartReloadSoundDelayed()
    {
        customAudioSource.SetSound(reloadSound);
        customAudioSource.Play();
    }

    public virtual void AbortReloading()
    {
        customAudioSource.Stop();
    }

    public virtual void EndReloading(int value)
    {
        currentMagazineAmmo = value;
        customAudioSource.Stop();

    }

    /*public void EndReload()
    {
        currentMagazineAmmo = magazineSize;
        customAudioSource.Stop();
    }*/

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

    /*public override void UpdateAimingLine()
    {
        aimingLine.DrawLine();
    }*/
}
