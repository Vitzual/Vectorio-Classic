// IDamageable Interface
// Apply to entities that should be able to receive damage

using UnityEngine;

public interface IDamageable
{
    float health { get; set; }
    float maxHealth { get; set; }
    void DamageEntity(int dmg);
    void DestroyEntity();
    void HealEntity(int amounnt);
}
