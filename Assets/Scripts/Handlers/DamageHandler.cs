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

        Events.active.onEnemyHurt += UpdateDamage;
        Events.active.onEnemyDestroyed += RemoveBar;
    }

    // Create damaged bar
    public void UpdateDamage(BaseEntity entity)
    {
        if (entity.GetComponent<DefaultGuardian>() != null) return;

        if (damagedEntities.ContainsKey(entity))
            damagedEntities[entity].UpdateDamage();
        else 
        { 
            DamageBar newBar = Instantiate(damageBar, entity.transform.position, Quaternion.identity).GetComponent<DamageBar>();
            newBar.rect.SetParent(entity.transform);
            newBar.rect.localPosition = new Vector2(0, -2.5f);
            newBar.entity = entity;
            newBar.UpdateDamage();
            damagedEntities.Add(entity, newBar);
        }
    }

    // Remove bar
    public void RemoveBar(BaseEntity entity)
    {
        if (damagedEntities.ContainsKey(entity))
            damagedEntities.Remove(entity);
    }
}
