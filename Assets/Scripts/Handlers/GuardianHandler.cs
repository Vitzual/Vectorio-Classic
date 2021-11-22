using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianHandler : MonoBehaviour
{
    // Active instance
    public static GuardianHandler active;

    // Hub instance
    public Hub hub;

    // Contains all active guardians in the scene
    public List<DefaultGuardian> guardians = new List<DefaultGuardian>();

    // Holds a reference to guardian button
    public GuardianButton guardianButton;
    public bool guardianSpawned;

    // Layering and scanning
    public LayerMask buildingLayer;
    private bool scan = false;

    // Guardian animation components
    public AudioSource laserSound;
    public AudioSource warningSound;
    public AudioSource music;
    public GameObject UI;

    // Laser variables
    public bool laserFiring;
    public int laserPart;
    public float cooldown = 5f;

    // Get active instance
    public void Awake() { active = this; }

    // Start method
    public void Start()
    {

    }

    // Update guardians
    public void Update()
    {
        // Check if animation playnig
        if (laserFiring) GuardianAnimation();

        // Move guardians each frame towards their target
        for (int i = 0; i < guardians.Count; i++)
        {
            if (guardians[i] != null)
            {
                if (guardians[i].target != null)
                {
                    guardians[i].MoveTowards(guardians[i].transform, guardians[i].target.transform);
                }
                else if (scan)
                {
                    BaseTile building = InstantiationHandler.active.GetClosestBuilding(Vector2Int.RoundToInt(guardians[i].transform.position));

                    if (building != null)
                        guardians[i].target = building;
                    else scan = false;
                }
            }
            else
            {
                guardians.RemoveAt(i);
                i--;
            }
        }
    }

    // Start animation
    public void StartGuardianBattle()
    {
        // Disable UI
        UI.SetActive(false);

        // Initiate laser sequence 
        laserPart = 0;
        cooldown = 0.5f;
        laserFiring = true;
        laserSound.Stop();
        music.Pause();

        // Reset all lasers
        hub.ResetLasers();
    }

    // Guardian animation sequence
    public void GuardianAnimation()
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
                hub.PlayChargeParticle();
                cooldown = 2.3f;
                laserPart = 4;
                break;
            case 4:
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    hub.FireLaser(Gamemode.stage.direction);
                    cooldown = 2.5f;
                    laserPart = 5;
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
                Border.PushBorder(Gamemode.stage.direction, 50f);
                if (cooldown <= 0)
                {
                    laserFiring = false;
                    UI.SetActive(true);
                    music.Play();
                    laserPart = 0;
                    SpawnGuardian();
                }
                break;
        }
    }

    // Proc guardian warning
    public void OpenGuardianWarning()
    {
        guardianButton.SetConfirmScreen(Gamemode.stage.guardian);
        guardianButton.gameObject.SetActive(true);
    }

    // Proc guardian warning
    public void CloseGuardianWarning()
    {
        guardianButton.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    public void SpawnGuardian()
    {
        // Get active stage
        Stage stage = Gamemode.stage;

        // Create the tile
        GameObject lastObj = Instantiate(stage.guardian.obj.gameObject, stage.guardianSpawnPos, Quaternion.identity);
        lastObj.name = stage.guardian.name;

        // Move to next stage
        Resource.active.SetStorage(Resource.CurrencyType.Heat, stage.nextStage.heat);

        // Get guardian stuff
        DefaultGuardian guardian = lastObj.GetComponent<DefaultGuardian>();
        guardian.Setup();

        // Add to active guardians
        guardians.Add(guardian);
    }

    // Destroys all active enemies
    public void DestroyAllGuardians()
    {
        for (int i = 0; i < guardians.Count; i++)
            Destroy(guardians[i].gameObject);
        guardians = new List<DefaultGuardian>();
    }
}
