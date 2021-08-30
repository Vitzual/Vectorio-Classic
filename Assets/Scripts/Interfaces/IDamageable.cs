// IDamageable Interface
// Apply to entities that should be able to receive damage

using UnityEngine;

public interface IDamageable
{
    int health { get; set; }
    int maxHealth { get; set; }
    void DamageEntity(int dmg);
    void DestroyEntity();
    void HealEntity(int amounnt);
}
