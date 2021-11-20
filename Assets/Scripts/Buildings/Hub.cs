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
    public AudioSource warningSound;
    public AudioSource music;
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
        laserPart = 0;
        cooldown = 0.5f;
        laserFiring = true;
        chargeParticle.Stop();
        laserSound.Stop();
        music.Pause();
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
                case 0:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        cooldown = 8f;
                        warningSound.Play();
                        laserPart = 1;
                    }
                    break;
                case 1:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        cooldown = 2f;
                        laserPart = 2;
                        warningSound.Stop();
                    }
                    break;
                case 2:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0) laserPart = 3;
                    break;
                case 3:
                    chargeParticle.Play();
                    laserSound.Play();
                    cooldown = 2.3f;
                    laserPart = 4;
                    break;
                case 4:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        laserParticles[laserIndex].Play();
                        cooldown = 2.5f;
                        laserPart = 5;
                        CameraShake.ShakeAll();
                    }
                    break;
                case 5:
                    cooldown -= Time.deltaTime;
                    if (cooldown <= 0)
                    {
                        cooldown = 11f;
                        laserPart = 6;
                    }
                    break;
                case 6:
                    cooldown -= Time.deltaTime;
                    if (border != null) border.PushBorder();
                    if (cooldown <= 0)
                    {
                        laserFiring = false;
                        UI.SetActive(true);
                        music.Play();
                        laserPart = 0;
                        EnemyHandler.active.SpawnGuardian();
                    }
                    break;
            }
        }
    }
}
