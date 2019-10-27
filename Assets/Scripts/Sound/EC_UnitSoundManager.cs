using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_UnitSoundManager : EntityComponent
{
    public VoicelinesSO voicelineSO;
    public AudioSourceCustom audioSource;
   
    public int currentStateID = 0;

    public AudioClip damageSound;
    public AudioClip deathSound;

    float nextPlayVoicelineSoundTime;

    [Header("Player only")]
    public bool isPlayer;
    public Transform playerTransform; //we only need this if is Player to locate the sound back to player on respawn

    public void SetState(int newState)
    {
        currentStateID = newState;
    }

    public override void SetUpComponent(GameEntity entity)
    {
        if (voicelineSO != null)
        {
            nextPlayVoicelineSoundTime = Time.time + Random.Range(0, voicelineSO.states[currentStateID].soundInterval);
        }
    }

    public override void UpdateComponent()
    {
        if (voicelineSO != null)
        {
            if(Time.time>= nextPlayVoicelineSoundTime)
            {
                VoicelineState currentState = voicelineSO.states[currentStateID];
                nextPlayVoicelineSoundTime = Time.time + currentState.soundInterval + Random.Range(-currentState.soundIntervallRandomiser, currentState.soundIntervallRandomiser);

                audioSource.SetSound(currentState.stateSounds[Random.Range(0, currentState.stateSounds.Length)]);
                audioSource.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeVoicelinesState(1);
        }

    }

    public void ChangeVoicelinesState(int newState)
    {
        if (voicelineSO != null)
        {
            currentStateID = newState;
            //Debug.Log("check state: " + voicelineSO.states[currentStateID].name);
            //Debug.Log("check sound interval: " + voicelineSO.states[currentStateID].soundInterval);
            //nextPlayVoicelineSoundTime = Time.time + Random.Range(0, voicelineSO.states[currentStateID].soundInterval);
        }

    }

    //IMPORTANT this should be before the ECHealth component in the GameEntity components array, then it will play the die Sound correctly

    public override void OnTakeDamage(DamageInfo damageInfo)
    {
        if (damageSound != null)
        {
            audioSource.SetSound(damageSound);
            audioSource.Play();
        }    
    }


    public override void OnDie(GameEntity killer)
    {
        transform.SetParent(null);
        if (!isPlayer) Destroy(gameObject, 10); //let the die sound stay a while  - but what should we do with the player?
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
