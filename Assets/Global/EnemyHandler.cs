using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // If the enemy destroys a building, play this sound
    public AudioSource BuildingGoDeadSound;
    public Survival survival;

    // Contains all active enemies in the scene
    [System.Serializable]
    public class ActiveEnemies
    {
        // Constructor
        public ActiveEnemies(Transform Object, EnemyClass ObjectClass, float Speed, int Damage, float RayLength, int Tracker)
        {
            this.Object = Object;
            this.Speed = Speed;
            this.Tracker = Tracker;
            this.Damage = Damage;
            this.RayLength = RayLength;
            this.ObjectClass = ObjectClass;
        }

        // Class variables
        public Transform Object { get; set; }
        public float Speed { get; set; }
        public int Tracker { get; set; }
        public int Damage { get; set; }
        public float RayLength { get; set; }
        public EnemyClass ObjectClass { get; set; }

    }
    public List<ActiveEnemies> Enemies;

    public LayerMask BuildingLayer;
    public bool isMenu = false;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;
    }


    // Handles enemy movement every frame
    public void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            try
            {
            Enemies[i].Object.position += Enemies[i].Object.up * Enemies[i].Speed * Time.deltaTime;
            if (Enemies[i].RayLength == 0)
            {
                continue;
            }
            else if (Enemies[i].Tracker == 3)
            {
                Enemies[i].Tracker = 1;
                RaycastHit2D hit = Physics2D.Raycast(Enemies[i].Object.position, Enemies[i].Object.up, Enemies[i].RayLength, BuildingLayer);
                if (hit.collider != null)
                    if (OnHit(i, hit.collider.transform)) { i--; continue; }
            }
            else
            {
                Enemies[i].Tracker += 1;
                continue;
            }
            }
            catch
            {
                Enemies.RemoveAt(i);
                i--;
            }
        }
    }

    // Registers an enemy to then be handled by the controller 
    public void RegisterEnemy(Transform Object, EnemyClass Script, float Speed, int Damage, float RayLength)
    {
        Enemies.Add(new ActiveEnemies(Object, Script, Speed, Damage, RayLength, 1));
    }

    // Called when a hit is detected in the updater 
    public bool OnHit(int enemyID, Transform other)
    {
        if (isMenu)
        {
            Enemies[enemyID].ObjectClass.KillEntity();
            Enemies.RemoveAt(enemyID);
            return true;
        }

        Vector3 pos = new Vector3(other.position.x, other.position.y, other.position.z);

        other.GetComponent<TileClass>().DamageTile(Enemies[enemyID].Damage);

        if (other != null && other.GetComponent<TileClass>().health > 0)
        {
            Enemies[enemyID].ObjectClass.KillEntity();
            Enemies.RemoveAt(enemyID);

            return true;
        }
        BuildingGoDeadSound.Play();
        survival.SetLastHit(pos);
        return false;
    }

    public Transform findClosest(Vector3 pos)
    {
        Transform result = null;
        float closest = float.PositiveInfinity;
        foreach (ActiveEnemies enemy in Enemies)
        {
            float distance = Vector2.Distance(enemy.Object.position, pos);
            if (distance < closest)
            {
                result = enemy.Object;
                closest = distance;
            }
        }
        return result;
    }
}
