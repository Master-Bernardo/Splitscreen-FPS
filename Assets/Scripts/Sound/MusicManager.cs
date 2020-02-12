using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//is responsible for diffeent music played in the background
public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    AudioClip currentClip;
    bool loop;


    public void PlayMusic(AudioClip clip, bool loop, float delay)
    {
        Invoke("PlayMusicDelayed", delay);
        currentClip = clip;
        this.loop = loop;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    void PlayMusicDelayed()
    {
        if (loop)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }

        audioSource.clip = currentClip;
        audioSource.Play();
    }
}
