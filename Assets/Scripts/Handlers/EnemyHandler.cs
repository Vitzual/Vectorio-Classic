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
    public bool isMenu = false;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;
    }

    // Handles enemy movement every frame
    public void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].obj != null) 
            {
                if (enemies[i].target != null)
                {
                    enemies[i].enemy.Rotate(enemies[i].obj, enemies[i].target.barrel);
                    enemies[i].variant.Move(enemies[i].obj, enemies[i].enemy.moveSpeed);
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
                                {
                                    barrel.target = enemies[i];
                                    barrel.hasTarget = true;
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
    }

    // Registers an enemy to then be handled by the controller 
    public void RegisterEnemy(Transform obj, Enemy enemy, Variant variant)
    {
        enemies.Add(new ActiveEnemy(obj, enemy, variant));
    }

}
