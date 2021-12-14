using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class EnemyHandler : MonoBehaviour
{
    // Active instance
    public static EnemyHandler active;

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
        if (active == null) active = this;
    }

    // Handles enemy movement every frame
    public void Update()
    {
        if (Settings.paused) return;

        // Move enemies each frame
        if (isMenu) MoveMenuEnemies();
        else MoveEnemies();
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

    // Move normal enemies
    public virtual void MoveEnemies()
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
                        enemies[a].SetTarget(building, false);
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

    // Create a new active enemy instance
    public virtual BaseEntity CreateEntity(Entity entity, Variant variant, Vector2 position, Quaternion rotation, float health = -1, float speed = -1)
    {
        // Create the tile
        GameObject lastObj = Instantiate(entity.obj.gameObject, position, rotation);
        lastObj.name = entity.name;

        // Attempt to set enemy variant
        DefaultEnemy enemy = lastObj.GetComponent<DefaultEnemy>();
        if (enemy != null)
        {
            // Setup enemy
            enemy.variant = variant;
            enemy.Setup();

            // Override health
            if (health != -1) enemy.health = health;
            if (speed != -1) enemy.moveSpeed = speed;

            // Set difficulty values
            enemy.health *= Gamemode.difficulty.enemyHealthModifier;
            enemy.maxHealth = enemy.health;
            enemy.moveSpeed *= Gamemode.difficulty.enemySpeedModifier;

            // Setup entity
            enemy.internalID = entity.InternalID;
            enemy.isMenu = isMenu;
            enemies.Add(enemy);

            // Assign runtime ID
            Server.AssignRuntimeID(enemy);

            // Return enemy
            return enemy;
        }
        else return null;
    }

    // Destroys all active enemies
    public virtual void DestroyAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i].gameObject);
        enemies = new List<DefaultEnemy>();
    }

    // Updates the active variant (Survival only)
    public virtual void UpdateVariant(int heatAmount, int heatStorage)
    {
        // Check currency
        if (heatAmount >= heatStorage) GuardianButton.active.ShowButton(Gamemode.stage.guardian);
        else GuardianButton.active.HideButton();
    }
}
