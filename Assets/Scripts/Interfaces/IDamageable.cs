// IDamageable Interface
// Apply to entities that should be able to receive damage

public interface IDamageable
{
    float health { get; set; }
    float maxHealth { get; set; }
    void DamageEntity(float dmg);
    void DestroyEntity();
    void HealEntity(float amount);
}
