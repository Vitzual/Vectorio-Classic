// IDamageable Interface
// Apply to entities that should be able to receive damage

using UnityEngine;

public interface IDamageable
{
    float health { get; set; }
    float maxHealth { get; set; }
    bool DamageEntity(float dmg);
    void DestroyEntity();
    void HealEntity(float amount);
}
