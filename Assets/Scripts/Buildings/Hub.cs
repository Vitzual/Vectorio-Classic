using UnityEngine;
using System.Collections.Generic;
using Thinksquirrel.CShake;

// This script is a WIP. Working on functionality first,
// then will clean it up to keep in sync with refactor.

public class Hub : BaseTile
{
    // Building scriptable
    public Building hub;

    // Other componenets
    public Border border;
    public ParticleSystem chargeParticle;
    public ParticleSystem[] laserParticles;
    public AudioSource laserSound;
    public GameObject UI;

    // Laser variables
    public bool laserFiring;
    public int laserPart;
    public int laserIndex;
    public float cooldown = 5f;

    // On start, assign weapon variables
    void Start()
    {
        chargeParticle.Stop();
        foreach(ParticleSystem laser in laserParticles) 
            laser.Stop();

        InstantiationHandler.active.SetCells(hub, transform.position, this);
    }

    public void FireLaser(int index)
    {
        // Disable UI
        UI.SetActive(false);

        // Initiate laser sequence 
        laserPart = 1;
        laserFiring = true;
        chargeParticle.Stop();
        laserSound.Stop();
        laserIndex = index;

        // Reset all lasers
        foreach (ParticleSystem laser in laserParticles)
            laser.Stop();
    }

    public void Update()
    {
        // Fire laser cinematic
        if (laserFiring)
        {
            // Controls laser animation
            switch (laserPart)
            {
                case 1:
                    chargeParticle.Play();
                    laserSound.Play();
                    cooldown = 2.3f;
                    laserPart = 2;
                    break;
                case 2:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        laserParticles[laserIndex].Play();
                        cooldown = 4f;
                        laserPart = 3;
                        CameraShake.ShakeAll();
                    }
                    break;
                case 3:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        cooldown = 6f;
                        laserPart = 4;
                    }
                    break;
                case 4:
                    cooldown -= Time.deltaTime;
                    if (border != null) border.PushBorder();
                    if (cooldown <= 0)
                    {
                        laserPart = 1;
                        laserFiring = false;
                        UI.SetActive(true);
                    }
                    break;
            }
        }
    }
}
