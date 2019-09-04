using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour
{
    int particleDecalDataIndex = 0;
    ParticleDecalData[] particleData;
    ParticleSystem.Particle[] particles;

    public int maxDecals = 100;
    public float decalSizeMin = 0.5f;
    public float decalSizeMax = 1.5f;
    public ParticleSystem decalParticleSystem;

    public static ParticleDecalPool Instance;

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        particleData = new ParticleDecalData[maxDecals];
        particles = new ParticleSystem.Particle[maxDecals];

        for (int i = 0; i < maxDecals; i++)
        {
            particleData[i] = new ParticleDecalData();
        }
    }

    public void ParticleHit(ParticleCollisionEvent particleCollisionEvent)
    {
        SetParticleData(particleCollisionEvent);
        DisplayParticles();
    }


    void SetParticleData(ParticleCollisionEvent particleCollisionEvent)
    {
        if(particleDecalDataIndex >= maxDecals)
        {
            particleDecalDataIndex = 0;
        }
        particleData[particleDecalDataIndex].position = particleCollisionEvent.intersection;
        Vector3 particleRotationEuler = Quaternion.LookRotation(-particleCollisionEvent.normal).eulerAngles;
        particleRotationEuler.z = Random.Range(0, 360);
        particleData[particleDecalDataIndex].rotation = particleRotationEuler;
        particleData[particleDecalDataIndex].size = Random.Range(decalSizeMin, decalSizeMax);

        particleDecalDataIndex++;
    }

    void DisplayParticles()
    {
        //read through the data array and turn it into an array of particles
        for (int i = 0; i < maxDecals; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].rotation3D = particleData[i].rotation;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = Color.red;
        }

        decalParticleSystem.SetParticles(particles, particles.Length);
    }

   

}
