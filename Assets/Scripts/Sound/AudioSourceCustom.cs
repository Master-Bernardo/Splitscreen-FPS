using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will be later used as a prefab to switch between splitscreen audio and normal audio
public class AudioSourceCustom : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
    }   

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetSound(AudioClip clip)
    {
        audioSource.clip = clip;
       
    }

    public bool IsPlayingSound()
    {
        return audioSource.isPlaying;

    }

    public void SetLoop(bool loop)
    {
        audioSource.loop = loop;
    }
}
