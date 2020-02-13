using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWeapon : Weapon
{
    #region Fields

    [Header("----------Missile Wapon---------")]
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

    [Header("Sound - Missile Weapon")]
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

    #endregion


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
                if (audioSource != null)
                {
                    if (!splitSounds)
                    {
                        audioSource.clip = shootSound;
                        audioSource.Play();
                    }
                    else
                    {
                        audioSource.clip = shootSoundStart;
                        audioSource.Play();
                    }
                }
               



                nextShootTime = Time.time + shootingInterval;
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

                    if (audioSource != null)
                    {
                        if (splitSounds)
                        {
                            if (!loppingMiddleSound)
                            {
                                audioSource.clip = shootSoundMid;
                                audioSource.Play();
                                audioSource.loop = true;
                            }
                        }
                        else
                        {
                            audioSource.clip = shootSound;
                            audioSource.Play();
                        }
                    }
                    



                    nextShootTime = Time.time + shootingInterval;
                    if (currentMagazineAmmo == 0)
                    {
                        if (weaponSystem != null)
                        {
                            weaponSystem.StartReloading();

                            if (audioSource != null)
                            {
                                if (splitSounds)
                                {
                                    audioSource.loop = false;
                                    audioSource.clip = shootSoundEnd;
                                    audioSource.Play();
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
        if (audioSource != null)
        {
            if (splitSounds)
            {
                audioSource.loop = false;
                audioSource.clip = shootSoundEnd;
                audioSource.Play();
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
        //Debug.Log("startReload");
        if (splitSounds)
        {
            audioSource.loop = false;
            audioSource.clip = shootSoundEnd;
            audioSource.Play();
        }
        Invoke("PlayStartReloadSoundDelayed", reloadSoundDelay);
    }

    void PlayStartReloadSoundDelayed()
    {
        if (audioSource)
        {
            audioSource.clip = reloadSound;
            audioSource.Play();
        }
      
    }

    public virtual void AbortReloading()
    {
       if(audioSource != null) audioSource.Stop();
    }

    public virtual void EndReloading(int value)
    {
        currentMagazineAmmo = value;
        audioSource.Stop();

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
