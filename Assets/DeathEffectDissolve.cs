using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//dont do it with he dissolve shader?
public class DeathEffectDissolve : MonoBehaviour
{
    public GameObject corpse;
    public Material dissolveMaterial;
    public Rigidbody rb;
    public float force;
    public Vector3 forcePosition;
    public float dissolveSpeed;
    float dissolveFactor = 0;

    bool dead = false;

    public void OnDie()
    {
        corpse.SetActive(true);
        transform.SetParent(null);
        dissolveMaterial.SetFloat("Vector1_6AEAFD48", dissolveFactor);
        //Debug.Log("direction before");
        //Debug.Log("direction: " + transform.InverseTransformDirection(force));
        //rb.AddForceAtPosition(transform.InverseTransformDirection(force), transform.InverseTransformPoint(forcePosition), ForceMode.Impulse);
       // rb.AddForce(-transform.forward * force, ForceMode.Impulse);
        rb.AddForceAtPosition(-transform.forward * force, transform.InverseTransformPoint(forcePosition), ForceMode.Impulse);


        dead = true;
    }


    void Update()
    {
        if (dead)
        {
            dissolveFactor += dissolveSpeed * Time.deltaTime;

            if (dissolveFactor >= 1)
            {
                Destroy(gameObject);
            }

            dissolveMaterial.SetFloat("Vector1_6AEAFD48", dissolveFactor);
        }      
    }
}
