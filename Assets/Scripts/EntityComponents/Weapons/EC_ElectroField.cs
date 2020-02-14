using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_ElectroField : EntityComponent
{
    public LayerMask collisionLayers;
    public AudioSource audioSource;
    public AudioClip idleSound;
    public AudioClip electrocuttingSoundStart;
    public AudioClip electrocuttingSoundMiddle;
    public AudioClip electrocuttingSoundEnd;
    public float loopSoundStartDelay;
    public float getBackToIdleSoundDelay;

    public float damageperSecond;
    //HashSet<IDamageable<DamageInfo>> targetsInsideCollider = new HashSet<IDamageable<DamageInfo>>();
    HashSet<GameEntity> targetsInsideCollider = new HashSet<GameEntity>();
    HashSet<GameEntity> targetsToRemove = new HashSet<GameEntity>();

    bool startedPlayingElectrocuteSound = false;
    float beginnPlayLoopSoundTime;

    bool endedPlayingElectrocuteSound = false;
    float startPlayingIdleSoundTime;

    public override void SetUpComponent(GameEntity entity)
    {
        base.SetUpComponent(entity);

        audioSource.loop = true;
        audioSource.clip = idleSound;
        audioSource.Play();
    }

    public override void UpdateComponent()
    {
        foreach(GameEntity  entity in targetsInsideCollider)
        {
            if (entity!=null) //check if not dead
            {
                entity.GetComponent<IDamageable<DamageInfo>>().TakeDamage(new DamageInfo(damageperSecond * Time.deltaTime, myEntity));
            }
            else
            {
                targetsToRemove.Add(entity);   
            }
        }

        foreach(GameEntity entityToRemove in targetsToRemove)
        {
            targetsInsideCollider.Remove(entityToRemove);
        }

        if (targetsToRemove.Count > 0)
        {
            targetsToRemove.Clear();
            if (targetsInsideCollider.Count == 0) StopPlayingElectrocuteSound();
        }
        

        if (startedPlayingElectrocuteSound)
        {
            if (Time.time > beginnPlayLoopSoundTime)
            {
                startedPlayingElectrocuteSound = false;
                audioSource.loop = true;
                audioSource.clip = electrocuttingSoundMiddle;
                audioSource.Play();
            }
        }
        else if (endedPlayingElectrocuteSound)
        {
            if(Time.time > startPlayingIdleSoundTime)
            {
                endedPlayingElectrocuteSound = false;
                audioSource.loop = true;
                audioSource.clip = idleSound;
                audioSource.Play();
            }
        }
    }

   
        
  
private void OnTriggerEnter(Collider collider)
{

    IDamageable<DamageInfo> damageable = collider.gameObject.GetComponent<IDamageable<DamageInfo>>();

        if (damageable != null)
        {
            GameEntity entity = collider.gameObject.GetComponent<GameEntity>();
            if (entity != null)
            {
                if (!Settings.Instance.friendlyFire)
                {
                    DiplomacyStatus diplomacyStatus = Settings.Instance.GetDiplomacyStatus(myEntity.teamID, entity.teamID);
                    if (diplomacyStatus == DiplomacyStatus.War)
                    {
                        if (targetsInsideCollider.Count == 0) StartPlayingElectrocuteSound();
                        targetsInsideCollider.Add(entity);
                    }
                }
                else
                {
                    if (targetsInsideCollider.Count == 0) StartPlayingElectrocuteSound();
                    targetsInsideCollider.Add(entity);
                }
            }
            else
            {
                if (targetsInsideCollider.Count == 0) StartPlayingElectrocuteSound();
                targetsInsideCollider.Add(entity);
            }

        }

    }

    private void OnTriggerExit(Collider collider)
    {
        GameEntity entity = collider.gameObject.GetComponent<GameEntity>();
        if (targetsInsideCollider.Contains(entity))
        {
            targetsInsideCollider.Remove(entity);
            if (targetsInsideCollider.Count == 0) StopPlayingElectrocuteSound();
        }
    }

    void StartPlayingElectrocuteSound()
    {
        startedPlayingElectrocuteSound = true;
        audioSource.loop = false;
        audioSource.clip = electrocuttingSoundStart;
        audioSource.Play();

        beginnPlayLoopSoundTime = Time.time + loopSoundStartDelay;
    }

    void StopPlayingElectrocuteSound()
    {
        endedPlayingElectrocuteSound = true;
        audioSource.loop = false;
        audioSource.clip = electrocuttingSoundEnd;
        audioSource.Play();

        startPlayingIdleSoundTime = Time.time + getBackToIdleSoundDelay;

    }
}
