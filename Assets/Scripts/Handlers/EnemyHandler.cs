using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class EnemyHandler : MonoBehaviour
{
    // Active instance
    public static EnemyHandler active;

    // Active variant
    public Variant variant;

    // Holds a reference to turret handler
    public TurretHandler turretHandler;

    // Holds a reference to guardian button
    public GuardianButton guardianButton;
    public bool guardianSpawned;

    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;

    // Contains all active enemies in the scene
    public List<DefaultEnemy> enemies = new List<DefaultEnemy>();
    public List<DefaultGuardian> guardians = new List<DefaultGuardian>();
    public LayerMask buildingLayer;
    private bool scan = false;

    // Is menu bool
    public bool isMenu;
    public Transform menuTarget;

    public void Awake() { active = this; }

    // Handles enemy movement every frame
    public void Update()
    {
        if (Settings.paused) return;

        // Move enemies each frame
        scan = true;
        if (isMenu)
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
        else
        {
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
    }

    // Rotates towards a target
    public void RotateTowards(Transform pos, Transform target)
    {
        Vector3 dir = pos.position - target.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pos.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    public void CreateEntity(Entity entity, Vector2 position, Quaternion rotation)
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
            holder.health = entity.health;
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
        for (int i = 0; i < guardians.Count; i++)
            Destroy(guardians[i].gameObject);
        enemies = new List<DefaultEnemy>();
    }

    // Updates the active variant (Survival only)
    public void UpdateVariant()
    {
        // Get heat currency
        Resource.Currency currency = Resource.active.currencies[Resource.CurrencyType.Heat];

        // Check currency
        if (currency.amount > currency.storage)
        {
            guardianButton.SetConfirmScreen(variant.guardian);
            guardianButton.gameObject.SetActive(true);
        }
        else
        {
            if (guardianButton.gameObject.activeSelf)
                guardianButton.gameObject.SetActive(false);

            foreach (Variant variant in ScriptableLoader.variants)
            {
                if (currency.amount >= variant.minHeat &&
                    currency.amount < variant.maxHeat)
                {
                    this.variant = variant;
                    return;
                }
            }
        }    
    }

    // Spawn guardian (NEEDS WORK)
    public void SpawnGuardian()
    {
        // Create the tile
        GameObject lastObj = Instantiate(variant.guardian.obj.gameObject, variant.guardian.guardianSpawnPosition, Quaternion.identity);
        lastObj.name = variant.guardian.name;

        // Move to next stage
        Resource.active.SetStorage(Resource.CurrencyType.Heat, variant.guardian.newMaxHeat);

        // Get guardian stuff
        DefaultGuardian guardian = lastObj.GetComponent<DefaultGuardian>();
        guardian.Setup();

        // Add to active guardians
        guardians.Add(guardian);
    }
}
