using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//after a projectile hits its ground, the impact audioSource remains active a bit longer and returns to the bullet after x seconds
public class ProjectileImpactSound : MonoBehaviour
{
    public float stayActiveDelay;

    public void ResetPosition(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Deparent()
    {
        transform.SetParent(null);
        DisableAfterDelay(stayActiveDelay);
    }

    //on impact
    public void DisableAfterDelay(float delay)
    {
        Invoke("DisableSound", delay);
    }

    void DisableSound()
    {
        gameObject.SetActive(false);
    }
}
