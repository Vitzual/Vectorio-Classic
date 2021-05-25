using UnityEngine;
using System.Collections.Generic;

public class TurretAI : TurretClass
{
    // Engineer variables
    private float damageBoost = 1;

    // Default stuff
    public int SwitchBarrel = 0;

    // Targetting system
    void Update()
    {
        // If animation is playing, wait
        if (animPlaying)
        {
            PlayAnim();
            return;
        }

        if (isRotating)
            RotationHandler();

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {            
            // Unflag hasTarget
            hasTarget = false;
            if (AppliedModification.Contains(0))
            {
                bool wasFired = Shoot(Bullet, FirePoints[SwitchBarrel], damageBoost);
                if (wasFired)
                    if (SwitchBarrel == 1)
                        SwitchBarrel = 2;
                    else
                        SwitchBarrel = 1;
            } 
            else if (AppliedModification.Contains(1))
            {
                bool wasFired = Shoot(Bullet, FirePoints[SwitchBarrel], damageBoost);
                if (wasFired)
                    if (SwitchBarrel + 1 == 7) 
                        SwitchBarrel = 3;
                    else
                        SwitchBarrel += 1;
            }
            else Shoot(Bullet, FirePoints[SwitchBarrel], damageBoost);
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }

    // Applies a modification specific to this building
    public override void ApplyModification(int modID)
    {
        // Check to see if modification has already been applied
        if (AppliedModification.Count == 0)
        { 
            switch (modID)
            {
                // Dual barrel
                case 0:

                    // Enable / disable child objects
                    EngineerModifications[0].originalObj.SetActive(false);
                    EngineerModifications[0].engineerObj.SetActive(true);

                    // Apply variable modifications
                    Gun = EngineerModifications[0].engineerObj.transform.GetChild(0).GetComponent<Transform>();
                    fireRate = 0.2f;
                    damageBoost = 60;

                    // Set the barrel
                    SwitchBarrel = 1;

                    break;

                // Quadra barrel
                case 1:

                    // Enable / disable child objects
                    EngineerModifications[1].originalObj.SetActive(false);
                    EngineerModifications[1].engineerObj.SetActive(true);

                    // Apply variable modifications
                    Gun = EngineerModifications[1].engineerObj.transform.GetChild(0).GetComponent<Transform>();
                    fireRate = 0.05f;
                    damageBoost = 50;

                    // Set the barrel
                    isRotating = true;

                    break;

                // Iridium plating
                case 2:

                    // Enable / disable child objects
                    EngineerModifications[2].originalObj.SetActive(false);
                    EngineerModifications[2].engineerObj.SetActive(true);

                    damageBoost = 150;
                    fireRate = 0.2f;

                    break;

                // Heavy plating
                case 3:

                    // Enable / disable child objects
                    EngineerModifications[3].originalObj.SetActive(false);
                    EngineerModifications[3].engineerObj.SetActive(true);

                    health = 1000;
                    maxhp = 1000;

                    break;

                // Light plating
                case 4:

                    // Enable / disable child objects
                    EngineerModifications[4].originalObj.SetActive(false);
                    EngineerModifications[4].engineerObj.SetActive(true);

                    // Lower heat / power costs
                    GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power - 1);
                    GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(GetHeat() - 1);
                    power = 1;
                    heatStack.Push(1);
                    break;

                default:
                    return;
            }

            AppliedModification.Add(modID);
        }
    }

    //

    // Kill defense
    public override void DestroyTile()
    {
        Survival srv = GameObject.Find("Survival").GetComponent<Survival>();
        srv.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
