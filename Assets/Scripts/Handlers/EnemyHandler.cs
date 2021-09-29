using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Holds a reference to turret handler
    public TurretHandler turretHandler;

    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;

    // Contains all active enemies in the scene
    public List<DefaultEnemy> enemies;
    public List<DefaultGuardian> guardians;

    public LayerMask buildingLayer;
    private bool scan = false;

    public void Start()
    {
        enemies = new List<DefaultEnemy>();
        Events.active.onEnemySpawned += RegisterEnemy;
        Events.active.onGuardianSpawned += RegisterGuardian;
    }

    // Handles enemy movement every frame
    public void Update()
    {
        if (Settings.paused) return;

        scan = true;

        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null) 
            {
                if (enemies[a].target != null)
                {
                    enemies[a].MoveTowards(enemies[a].transform, enemies[a].target.transform);
                }
                else if (scan)
                {
                    DefaultBuilding building = BuildingSystem.active.GetClosestBuilding(Vector2Int.RoundToInt(enemies[a].transform.position));

                    if (building != null)
                    {
                        enemies[a].target = building;
                        RotateTowards(enemies[a].transform, building.transform);
                    }
                    else scan = false;
                }
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
            }
        }

        for (int i = 0; i < guardians.Count; i++)
        {
            if (guardians[i] != null)
            {
                if (guardians[i].target != null)
                {
                    guardians[i].MoveTowards(guardians[i].transform, guardians[i].target.transform);
                }
                else
                {
                    DefaultBuilding building = BuildingSystem.active.GetClosestBuilding(Vector2Int.RoundToInt(guardians[i].transform.position));

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

    // Rotates towards a target
    public void RotateTowards(Transform pos, Transform target)
    {
        Vector3 dir = pos.position - target.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pos.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    // Registers an enemy to then be handled by the controller 
    public void RegisterEnemy(DefaultEnemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RegisterGuardian(DefaultGuardian guardian)
    {
        guardians.Add(guardian);
    }

    // Destroys all active enemies
    public void DestroyAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i].gameObject);
        enemies = new List<DefaultEnemy>();
    }
}
