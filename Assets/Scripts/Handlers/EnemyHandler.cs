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
        public ActiveEnemies(Transform obj, Enemy enemy)
        {
            this.obj = obj;
            this.enemy = enemy;
        }

        // Class variables
        public Transform obj;
        public Enemy enemy;
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
                enemies[i].enemy.Move(enemies[i].obj);
                RaycastHit2D hit = Physics2D.Raycast(enemies[i].obj.position, enemies[i].obj.up, 2f, buildingLayer);

                if (hit.collider != null)
                {
                    if (isMenu)
                    {
                        enemies[i].enemy.Kill(enemies[i].obj);
                        enemies.RemoveAt(i);
                        return;
                    }
                    else
                    {
                        DefaultBuilding building = hit.collider.GetComponent<DefaultBuilding>();
                        if (building != null) enemies[i].enemy.GiveDamage(building);
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
    public void RegisterEnemy(Transform obj, Enemy enemy)
    {
        enemies.Add(new ActiveEnemies(obj, enemy));
    }

    // Request information about an enemy using the transform attached
    public int RequestID(Transform obj)
    {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i].obj == obj) return i;
        return -1;
    }

    public Transform findClosest(Vector3 pos)
    {
        Transform result = null;
        float closest = float.PositiveInfinity;
        foreach (ActiveEnemies enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.obj.position, pos);
            if (distance < closest)
            {
                result = enemy.obj;
                closest = distance;
            }
        }
        return result;
    }
}
