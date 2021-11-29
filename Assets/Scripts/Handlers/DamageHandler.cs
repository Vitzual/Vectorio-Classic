using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    // Damage bar
    public DamageBar damageBar;
    public Dictionary<BaseEntity, DamageBar> damagedEntities;

    // Start thing
    public void Start()
    {
        damagedEntities = new Dictionary<BaseEntity, DamageBar>();

        Events.active.onEnemyHurt += UpdateEnemyDamage;
        Events.active.onEnemyDestroyed += RemoveBar;
    }

    // Create damaged bar
    public void UpdateEnemyDamage(DefaultEnemy enemy)
    {
        if (damagedEntities.ContainsKey(enemy))
            damagedEntities[enemy].UpdateDamage();
        else 
        {
            Vector2 pos = new Vector2(enemy.transform.position.x, enemy.transform.position.y - 3.5f);
            DamageBar newBar = Instantiate(damageBar, pos, Quaternion.identity).GetComponent<DamageBar>();
            newBar.rect.SetParent(enemy.transform);
            newBar.entity = enemy;
            newBar.UpdateDamage();
            newBar.SetBarColor(enemy.variant);
            damagedEntities.Add(enemy, newBar);
        }
    }

    // Remove bar
    public void RemoveBar(BaseEntity entity)
    {
        if (damagedEntities.ContainsKey(entity))
            damagedEntities.Remove(entity);
    }
}
