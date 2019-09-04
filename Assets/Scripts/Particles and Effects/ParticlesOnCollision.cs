using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnCollision : MonoBehaviour
{
    ParticleDecalPool bloodDecalPool;

    public ParticleSystem bloodParticles;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        bloodDecalPool = ParticleDecalPool.Instance;
    }


    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(bloodParticles, other, collisionEvents); //this method fills the coliisionEvents list

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            bloodDecalPool.ParticleHit(collisionEvents[i]);
        }
    }

}
