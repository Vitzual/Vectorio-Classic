using UnityEngine;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using Michsky.UI.ModernUIPack;

// This script is a WIP. Working on functionality first,
// then will clean it up to keep in sync with refactor.

public class Hub : BaseTile
{
    // Building scriptable
    public Building hub;
    public ModalWindowManager endScreen;

    // Hub particles
    public AudioSource laserSound;
    public ParticleSystem chargeParticle;
    public ParticleSystem[] laserParticles;

    // On start, assign weapon variables
    public void Start()
    {
        chargeParticle.Stop();
        foreach(ParticleSystem laser in laserParticles) 
            laser.Stop();

        InstantiationHandler.active.SetCells(hub, transform.position, this);

        health = hub.health;
        maxHealth = health;
    }

    // Display charge particle
    public void PlayChargeParticle()
    {
        chargeParticle.Play();
        laserSound.Play();
    }

    // Fire laser
    public void FireLaser(Border.Direction direction)
    {
        CameraShake.ShakeAll();
        laserParticles[(int)direction].Play();
    }

    // Reset hub laser
    public void ResetLasers()
    {
        // Reset all lasers
        foreach (ParticleSystem laser in laserParticles)
            laser.Stop();
    }

    public override void DestroyEntity()
    {
        endScreen.OpenWindow();
        base.DestroyEntity();
    }
}
