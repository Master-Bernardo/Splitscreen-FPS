using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dont do it with he dissolve shader?
public class DeathEffect : MonoBehaviour
{
    //TODO let the body fly on the ground, but make a seperate physics layer for this, which only collides with environemnt,
    //also add particle effect and a timer after which the body dissappears, maybe let the body slowly side to the ground
    public GameObject corpse;
    public Rigidbody rb;
    public float force;
    public Vector3 forcePosition;

    bool dead = false;
    bool timerStarted = false;
    public float dissapearTime;
    float nextDissapearTime;

    //bool defaultForce = true; //should the corpse just be pushed back or should we use the force, from the weapon?

    public void OnDie()
    {
        //defaultForce = true;
        corpse.SetActive(true);
        transform.SetParent(null);
        rb.AddForceAtPosition(-transform.forward * force, transform.InverseTransformPoint(forcePosition), ForceMode.Impulse);

        dead = true;
    }

    public void OnDie(Vector3 force)
    {
        //defaultForce = false;
        corpse.SetActive(true);
        transform.SetParent(null);
        Debug.Log("force:" + force);
        rb.AddForceAtPosition(force, transform.InverseTransformPoint(forcePosition), ForceMode.Impulse);

        dead = true;
    }


    void Update()
    {
        if (dead)
        {
            if (!timerStarted)
            {
                if (rb.velocity.magnitude < 0.1)
                {
                    nextDissapearTime = Time.time + dissapearTime;
                    timerStarted = true;
                }
            }
            
        }

        if (timerStarted)
        {
            if(Time.time> nextDissapearTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
