using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class EnemyHandler : MonoBehaviour
{
    // Active instance
    public static EnemyHandler active;

    // Holds a reference to turret handler
    public TurretHandler turretHandler;

    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;

    // Contains all active enemies in the scene
    public List<DefaultEnemy> enemies = new List<DefaultEnemy>();

    // Layering and scanning
    public LayerMask buildingLayer;
    private bool scan = false;

    // Is menu bool
    public bool isMenu;
    public Transform menuTarget;

    public void Awake() { active = this; }

    // Start method
    public void Start()
    {
        // Get active guardian
        if (GuardianHandler.active == null)
            Debug.Log("Guardian Handler is missing from scene! Boss battles will not fully work!");
    }

    // Handles enemy movement every frame
    public void Update()
    {
        if (Settings.paused) return;

        // Move enemies each frame
        if (isMenu) MoveMenuEnemies();
        else
        {
            // Reset scan
            scan = true;

            // Move enemies each frame towards their target
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
                        BaseTile building = InstantiationHandler.active.GetClosestBuilding(Vector2Int.RoundToInt(enemies[a].transform.position));

                        if (building != null)
                        {
                            enemies[a].target = building;
                            RotateTowards(enemies[a].rotator, building.transform);
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
    }

    public DefaultEnemy GetStrongestEnemy()
    {
        // Strongest
        DefaultEnemy strongest = null;
        float health = 0;

        // Move enemies each frame towards their target
        int max = enemies.Count;
        if (max > 50) max = 50;
        for (int a = 0; a < max; a++)
        {
            if (enemies[a] != null && enemies[a].health > health)
            {
                strongest = enemies[a];
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
            }
        }

        return strongest;
    }

    // Move menu enemies
    public void MoveMenuEnemies()
    {
        // Move enemies upward on meun
        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null)
            {
                float step = enemies[a].enemy.moveSpeed * Time.deltaTime;
                enemies[a].transform.position = Vector2.MoveTowards(enemies[a].transform.position, menuTarget.position, step);
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
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

    // Create a new active enemy instance
    public void CreateEntity(Entity entity, Variant variant, Vector2 position, Quaternion rotation, float health = -1)
    {
        // Create the tile
        GameObject lastObj = Instantiate(entity.obj.gameObject, position, rotation);
        lastObj.name = entity.name;

        // Attempt to set enemy variant
        DefaultEnemy enemy = lastObj.GetComponent<DefaultEnemy>();
        if (enemy != null) enemy.variant = variant;

        // Set the health for the entity
        BaseEntity holder = lastObj.GetComponent<BaseEntity>();
        if (entity != null)
        {
            if (health != -1) holder.health = health;
            else holder.health = entity.health;
            holder.maxHealth = holder.health;
        }

        // Setup entity
        lastObj.GetComponent<BaseEntity>().Setup();
        enemy.isMenu = isMenu;
        enemies.Add(enemy);
    }

    // Destroys all active enemies
    public void DestroyAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i].gameObject);
        enemies = new List<DefaultEnemy>();
    }

    // Updates the active variant (Survival only)
    public void UpdateVariant()
    {
        // Get heat currency
        Resource.Currency currency = Resource.active.currencies[Resource.CurrencyType.Heat];

        // Check currency
        if (currency.amount > currency.storage) GuardianHandler.active.OpenGuardianWarning();
        else GuardianHandler.active.CloseGuardianWarning();
    }
}
