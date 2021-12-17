using UnityEngine;
//using Mirror;
using System.Collections.Generic;

public class BaseTile : BaseEntity
{
    [HideInInspector] public List<Vector2Int> cells;
    [HideInInspector] public Buildable buildable;
    public bool saveBuilding = true;
    public bool isSellable = true;

    public override void Setup() { SetupBase(); }

    public void SetupBase() 
    {
        // Update nearby ports and unlockables
        DroneManager.active.UpdateNearbyPorts(this, transform.position);

        // Check if buildable null
        if (buildable != null)
        {
            Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, 1);

            // Set death particle
            if (buildable.building.deathParticle != null)
                particle = buildable.building.deathParticle;

            // Set health
            health = buildable.building.health * Research.healthBoost;
            maxHealth = health;

            // Update storages
            foreach (Cost cost in buildable.building.resources)
                if (cost.storage) Resource.active.ApplyStorage(cost.type, cost.amount);
        }

        // Fire building placed event
        Events.active.BuildingPlaced(this);
    }

    public void CheckNearbyEnergizers()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, InstantiationHandler.active.aocbLayer);
        if (hit.collider == null) DestroyEntity();
    }

    public override void OnBoxCollision(DefaultEnemy enemy)
    {
        // If has health, damage entity
        if (enemy.enemy.largeEnemy) DestroyEntity();
        else
        {
            DamageEntity(enemy.damage);
            if (health > 0) enemy.DestroyEntity();
            else enemy.DamageEntity(buildable.building.health);
        }
    }

    public override void DestroyEntity()
    {
        // Check buildable
        bool isBuildableNull = buildable == null;

        // Update unlockables
        if(!isBuildableNull) Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.PlaceBuildingAmount, buildable.building, -1);

        // Remove cells
        if (InstantiationHandler.active != null)
        {
            foreach (Vector2Int cell in cells)
                InstantiationHandler.active.tileGrid.RemoveCell(cell);
        }

        // Refund cost
        if (!isBuildableNull && Gamemode.active.refundResources) 
            Resource.active.RefundResources(buildable.resources);

        // Update damage handler
        Events.active.BuildingDestroyed(this);

        // Create particle and destroy
        if (!isBuildableNull && buildable.cosmetic != null && buildable.cosmetic.useDeathEffect)
        {
            ParticleSystemRenderer newParticle = Instantiate(buildable.cosmetic.deathEffect.effect, transform.position, 
                transform.rotation).GetComponent<ParticleSystemRenderer>();
            newParticle.material = buildable.cosmetic.deathEffect.material;
            newParticle.trailMaterial = buildable.cosmetic.deathEffect.material;
        }
        if (particle != null)
        {
            ParticleSystemRenderer newParticle = Instantiate(particle, transform.position, transform.rotation).GetComponent<ParticleSystemRenderer>();
            newParticle.material = buildable.building.material;
            newParticle.trailMaterial = buildable.building.material;
        }
        Destroy(gameObject);

        // Update storages
        if (!isBuildableNull)
            foreach (Cost cost in buildable.building.resources)
                if (cost.storage) Resource.active.ApplyStorage(cost.type, -cost.amount);
    }

    // Get material
    public override Material GetMaterial()
    {
        return buildable.building.material;
    }
}
