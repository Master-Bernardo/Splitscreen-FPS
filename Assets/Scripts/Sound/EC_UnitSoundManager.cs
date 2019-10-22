using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_UnitSoundManager : EntityComponent
{
    public VoicelinesSO voicelineSO;
    public AudioSourceCustom audioSource;
   
    public int currentState = 0;

    public AudioClip damageSound;
    public AudioClip deathSound;

    float nextPlaySoundTime;

    [Header("Player only")]
    public bool isPlayer;
    public Transform playerTransform; //we only need this if is Player to locate the sound back to player on respawn

    public void SetState(int newState)
    {
        currentState = newState;
    }

    public override void UpdateComponent()
    {
        base.UpdateComponent();

        if (voicelineSO != null)
        {

        }

    }
    //IMPORTANT this should be before the ECHealth component in the GameEntity components array, then it will play the die Sound correctly

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        Debug.Log("takeDamage");
        audioSource.SetSound(damageSound);
        audioSource.Play();
    }


    public override void OnDie(GameEntity killer)
    {
        Debug.Log("Die");

        transform.SetParent(null);
        if (!isPlayer) Destroy(this.gameObject, 10); //let the die sound stay a while  - but what should we do with the player?
        audioSource.SetSound(deathSound);
        audioSource.Play();
    }


    private void OnEnable()
    {
        if (isPlayer)
        {
            transform.SetParent(playerTransform);
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }

}
