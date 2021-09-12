using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;

    // Contains all active enemies in the scene
    [System.Serializable]
    public class ActiveEnemies
    {
        // Constructor
        public ActiveEnemies(Transform obj, Enemy enemy, Variant variant)
        {
            this.obj = obj;
            this.enemy = enemy;
            variant = enemy.variant;
        }

        // Class variables
        public Transform obj;
        public Enemy enemy;
        public Variant variant;
    }
    public List<ActiveEnemies> enemies;

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
                enemies[i].variant.Move(enemies[i].obj, enemies[i].enemy.moveSpeed);
                RaycastHit2D hit = Physics2D.Raycast(enemies[i].obj.position, enemies[i].obj.up, 2f, buildingLayer);

                if (hit.collider != null)
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
        enemies.Add(new ActiveEnemies(obj, enemy, variant));
    }
}
