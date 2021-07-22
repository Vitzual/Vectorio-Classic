// IDamageable Interface
//
// Apply to entities that should be able to receive damage

public interface IDamageable
{
    int health { get; set; }
    int maxHealth { get; set; }
    void damageEntity(int dmg);
    void destroyEntity();
    void healEntity(int amounnt);
}
