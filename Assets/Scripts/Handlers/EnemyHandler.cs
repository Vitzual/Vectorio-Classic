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

    public LayerMask buildingLayer;
    private bool isMenu = false;
    private bool scan = false;

    public void Start()
    {
        enemies = new List<DefaultEnemy>();
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;

        Events.active.onEnemySpawned += RegisterEnemy;
    }

    // Handles enemy movement every frame
    public void Update()
    {
        scan = true;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null) 
            {
                if (enemies[i].target != null)
                {
                    enemies[i].enemy.MoveTowards(enemies[i].transform, enemies[i].target.transform);
                    RaycastHit2D hit = Physics2D.Raycast(enemies[i].transform.position, enemies[i].transform.up, 2f, buildingLayer);

                    if (hit.collider != null)
                    {
                        if (Vector2.Distance(hit.collider.transform.position, enemies[i].transform.position) <= enemies[i].enemy.rayLength)
                        {
                            if (isMenu)
                            {
                                enemies[i].DestroyEntity();
                                enemies.RemoveAt(i);
                                i--;
                            }
                            else
                            {
                                DefaultBuilding building = hit.collider.GetComponent<DefaultBuilding>();
                                if (building != null)
                                {
                                    enemies[i].GiveDamage(building);
                                    if (building.transform != null)
                                    {
                                        enemies[i].DestroyEntity();
                                        enemies.RemoveAt(i);
                                        i--;
                                    }
                                }
                            }
                        }
                        else 
                        {
                            DefaultTurret turret = hit.collider.GetComponent<DefaultTurret>();
                            if (turret != null)
                                turret.AddTarget(enemies[i]);
                        }
                    }
                }
                else if (scan)
                {
                    DefaultBuilding building = BuildingSystem.active.GetClosestBuilding(Vector2Int.RoundToInt(enemies[i].transform.position));

                    if (building != null)
                    {
                        enemies[i].target = building;
                        RotateTowards(enemies[i].transform, building.transform);
                    }
                    else scan = false;
                }
            }
            else
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
    }

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
}
