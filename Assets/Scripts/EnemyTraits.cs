using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTraits : MonoBehaviour
{
    
    // public bool AggroLostParticles;
    private ParticleSystem particles;
    private Func<bool> inRange;
    private bool vfx;
    public int aggroRange;
    public void EnableSearchingVFX(ParticleSystem particles, Func<bool> inRange)
    {
        vfx = true;
        this.particles = particles;
        this.inRange = inRange;
    }
    public void Update()
    {
        //When a target is outside of aggroRange, deploy the particles
        if (!(bool)vfx)
        {
            return;
        } 
        if (vfx && !inRange())
        {
            particles.gameObject.SetActive(true);
        }
        else
        {
            particles.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
}


