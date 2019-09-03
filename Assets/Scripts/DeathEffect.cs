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
    bool timer1Started = false; //timer 1 is how long it will stay and dont move
    bool timer2Started = false; //timer 2 is for going underground slowly
    public float stayOnEarthTime;
    public float dissapearTime;
    float stayOnEarthTimeEnd;
    public float descendSpeed;
    float dissapearTimeEnd;

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
        //Debug.Log("force:" + force);
        rb.AddForceAtPosition(force, transform.InverseTransformPoint(forcePosition), ForceMode.Impulse);

        dead = true;
    }


    void Update()
    {
        if (dead)
        {
            if (!timer1Started)
            {
                if (rb.velocity.magnitude < 0.1)
                {
                    stayOnEarthTimeEnd = Time.time + stayOnEarthTime;
                    timer1Started = true;
                }
            }
            
        }

        if (timer2Started)
        {
            if (Time.time > dissapearTimeEnd)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position -= Vector3.up * descendSpeed;
            }
        }
        else if (timer1Started)
        {
            if(Time.time> stayOnEarthTimeEnd)
            {
                timer2Started = true;
                dissapearTimeEnd = Time.time + dissapearTime;
                //timer1Started = false;
                rb.isKinematic = true;


            }
        }
       
    }
}
