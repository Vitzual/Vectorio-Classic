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
    public List<ActiveEnemy> enemies;

    public LayerMask buildingLayer;
    private bool isMenu = false;
    private bool scan = false;

    public void Start()
    {
        enemies = new List<ActiveEnemy>();
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;

        Events.active.onEnemySpawned += RegisterEnemy;
    }

    // Handles enemy movement every frame
    public void Update()
    {
        scan = true;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].obj != null) 
            {
                if (enemies[i].target != null)
                {
                    enemies[i].enemy.MoveTowards(enemies[i].obj, enemies[i].target.transform);
                    RaycastHit2D hit = Physics2D.Raycast(enemies[i].obj.position, enemies[i].obj.up, 2f, buildingLayer);

                    if (hit.collider != null)
                    {
                        if (Vector2.Distance(hit.collider.transform.position, enemies[i].obj.position) <= enemies[i].enemy.rayLength)
                        {
                            if (isMenu)
                            {
                                enemies[i].variant.Kill(enemies[i].obj);
                                enemies.RemoveAt(i);
                                i--;
                            }
                            else
                            {
                                DefaultBuilding building = hit.collider.GetComponent<DefaultBuilding>();
                                if (building != null && enemies[i].variant.GiveDamage(building, enemies[i].enemy.damage))
                                {
                                    enemies[i].variant.Kill(enemies[i].obj);
                                    Destroy(enemies[i].obj.gameObject);
                                    enemies.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        else 
                        {
                            DefaultTurret turret = hit.collider.GetComponent<DefaultTurret>();
                            if (turret != null)
                            {
                                turretHandler.turretEntities.TryGetValue(turret, out ActiveTurret barrel);
                                if (barrel != null)
                                    barrel.target = enemies[i];
                            }
                        }
                    }
                }
                else if (scan)
                {
                    DefaultBuilding building = BuildingSystem.active.GetClosestBuilding(Vector2Int.RoundToInt(enemies[i].obj.transform.position));

                    if (building != null)
                    {
                        enemies[i].target = building;
                        RotateTowards(enemies[i].obj, building.transform);
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
    public void RegisterEnemy(DefaultEnemy enemy, Transform rotator)
    {
        enemies.Add(new ActiveEnemy(enemy.transform, enemy, enemy.enemy, rotator));
    }
}
