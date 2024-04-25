using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    public void SpawnDebris()
    {
        if (!ParticleSystem.isPlaying)
        {
            ParticleSystem.Play();
        }
    }

    public void StopDebris()
    {
        if (ParticleSystem.isPlaying)
        {
            ParticleSystem.Stop();
        }
    }
}
