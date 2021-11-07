using UnityEngine;
using System.Collections.Generic;
using Thinksquirrel.CShake;

public class Hub : BaseTile
{
    public Building hub;

    public ParticleSystem chargeParticle;
    public ParticleSystem laserParticle;
    public AudioSource laserSound;
    public bool laserFiring;
    public int laserPart;
    public float cooldown = 5f;
    public GameObject UI;

    // On start, assign weapon variables
    void Start()
    {
        Events.active.fireHubLaser += FireLaser;

        chargeParticle.Stop();
        laserParticle.Stop();

        InstantiationHandler.active.SetCells(hub, transform.position, this);
    }

    public void FireLaser()
    {
        UI.SetActive(false);
        laserPart = 1;
        laserFiring = true;
        chargeParticle.Stop();
        laserParticle.Stop();
        laserSound.Stop();
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
                        laserParticle.Play();
                        cooldown = 10f;
                        laserPart = 3;
                        CameraShake.ShakeAll();
                    }
                    break;
                case 3:
                    cooldown -= Time.deltaTime;
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
