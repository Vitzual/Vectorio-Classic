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

        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null) 
            {
                if (enemies[a].target != null)
                {
                    enemies[a].enemy.MoveTowards(enemies[a].transform, enemies[a].target.transform);

                    if (enemies[a].raycastCooldown == 0)
                    {
                        enemies[a].raycastCooldown = 5;

                        RaycastHit2D[] hit = Physics2D.RaycastAll(enemies[a].transform.position, enemies[a].transform.up, 2f, buildingLayer);

                        for (int b = 0; b < hit.Length; b++)
                        {
                            if (hit[b].collider != null)
                            {
                                if (Vector2.Distance(hit[b].collider.transform.position, enemies[a].transform.position) <= 2f)
                                {
                                    if (isMenu)
                                    {
                                        enemies[a].DestroyEntity();
                                        enemies.RemoveAt(a);
                                        a--;
                                    }
                                    else
                                    {
                                        DefaultBuilding building = hit[b].collider.GetComponent<DefaultBuilding>();
                                        if (building != null)
                                        {
                                            enemies[a].GiveDamage(building);
                                            if (building.transform != null)
                                            {
                                                enemies[a].DestroyEntity();
                                                enemies.RemoveAt(a);
                                                a--;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DefaultTurret turret = hit[b].collider.GetComponent<DefaultTurret>();
                                    if (turret != null)
                                        turret.AddTarget(enemies[a]);
                                }
                            }
                        }
                    }
                    else
                    {
                        enemies[a].raycastCooldown -= 1;
                    }
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

    public void DestroyAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i].gameObject);
        enemies = new List<DefaultEnemy>();
    }
}
