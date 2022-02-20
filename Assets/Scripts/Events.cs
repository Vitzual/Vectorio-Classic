using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events active;

    // Start is called before the first frame update
    public void Awake()
    {
        active = this;
    }

    // Cosmetic applied
    public event Action<Cosmetic> onCosmeticApplied;
    public void CosmeticApplied(Cosmetic cosmetic)
    {
        if (onCosmeticApplied != null)
            onCosmeticApplied(cosmetic);
    }

    // On nameplate setup
    public event Action<Transform, string, Color> onSetupNameplate;
    public void SetupNameplate(Transform obj, string name, Color color)
    {
        if (onSetupNameplate != null)
            onSetupNameplate(obj, name, color);
    }

    // On metadata changed
    public event Action<int, int> onMetadataChanged;
    public void ChangeMetadata(int runtimeID, int metadata)
    {
        if (onMetadataChanged != null)
            onMetadataChanged(runtimeID, metadata);
    }

    // On enemy set difficulty
    public event Action<float, float> onSetEnemyDifficulty;
    public void SetEnemyDifficulty(float rate, float size)
    {
        if (onSetEnemyDifficulty != null)
            onSetEnemyDifficulty(rate, size);
    }

    // On client disconnect
    public event Action onClientDisconnect;
    public void ClientDisconnect()
    {
        if (onClientDisconnect != null)
            onClientDisconnect();
    }

    // On hub destroyed
    public event Action onHubDestroyed;
    public void HubDestroyed()
    {
        if (onHubDestroyed != null)
            onHubDestroyed();
    }

    // On hub destroyed
    public event Action onChargeHubLaser;
    public void ChargeHubLasers()
    {
        if (onChargeHubLaser != null)
            onChargeHubLaser();
    }

    // On hub destroyed
    public event Action<Border.Direction> onHubFireLaser;
    public void FireHubLaser(Border.Direction direction)
    {
        if (onHubFireLaser != null)
            onHubFireLaser(direction);
    }

    // Pipette
    public event Action<BaseTile> onPipette;
    public void Pipette(BaseTile baseTile)
    {
        if (onPipette != null)
            onPipette(baseTile);
    }

    // Auto save
    public event Action onAutoSave;
    public void AutoSave()
    {
        if (onAutoSave != null)
            onAutoSave();
    }

    // Auto save
    public event Action onRefreshSound;
    public void RefreshSound()
    {
        if (onRefreshSound != null)
            onRefreshSound();
    }

    // On building clicked
    public event Action onStartGuardianBattle;
    public void StartGuardianBattle()
    {
        if (onStartGuardianBattle != null)
            onStartGuardianBattle();
    }

    // On building clicked
    public event Action<Material, Color> onChangeBorderColor;
    public void ChangeBorderColor(Material border, Color fill)
    {
        if (onChangeBorderColor != null)
            onChangeBorderColor(border, fill);
    }

    // On building clicked
    public event Action<ResearchTech> onResearchUnlocked;
    public void ResearchUnlocked(ResearchTech research)
    {
        if (onResearchUnlocked != null)
            onResearchUnlocked(research);
    }

    // On building clicked
    public event Action<BaseEntity> onEntityClicked;
    public void EntityClicked(BaseEntity entity)
    {
        if (onEntityClicked != null)
            onEntityClicked(entity);
    }

    // On building clicked
    public event Action<ResearchLab> onLabClicked;
    public void LabClicked(ResearchLab lab)
    {
        if (onLabClicked != null)
            onLabClicked(lab);
    }

    // On building clicked
    public event Action<ResearchLab> onLabDestroyed;
    public void LabDestroyed(ResearchLab lab)
    {
        if (onLabDestroyed != null)
            onLabDestroyed(lab);
    }

    // On building clicked
    public event Action onCloseResearch;
    public void CloseResearch()
    {
        if (onCloseResearch != null)
            onCloseResearch();
    }

    // On building clicked
    public event Action<ResearchTech> onResearchButtonClicked;
    public void ResearchButtonClicked(ResearchTech tech)
    {
        if (onResearchButtonClicked != null)
            onResearchButtonClicked(tech);
    }

    // On blueprint collected
    public event Action<Buildable> onBuildingUnlocked;
    public void BuildingUnlocked(Buildable buildable)
    {
        if (onBuildingUnlocked != null)
            onBuildingUnlocked(buildable);
    }

    // On blueprint collected
    public event Action<EnemyData> onEnemyDiscovered;
    public void EnemyDiscovered(EnemyData enemy)
    {
        if (onEnemyDiscovered != null)
            onEnemyDiscovered(enemy);
    }

    // On blueprint collected
    public event Action<string> onEnemyGroupSpawned;
    public void EnemyGroupSpawned(string msg)
    {
        if (onEnemyGroupSpawned != null)
            onEnemyGroupSpawned(msg);
    }

    // On enemy destroyed
    public event Action<BaseEntity> onBuildingHurt;
    public void BuildingHurt(BaseEntity tile)
    {
        if (onBuildingHurt != null)
            onBuildingHurt(tile);
    }

    // On enemy destroyed
    public event Action<GhostTile> onGhostPlaced;
    public void GhostPlaced(GhostTile building)
    {
        if (onGhostPlaced != null)
            onGhostPlaced(building);
    }

    // On enemy destroyed
    public event Action<GhostTile> onGhostDestroyed;
    public void GhostDestroyed(GhostTile building)
    {
        if (onGhostDestroyed != null)
            onGhostDestroyed(building);
    }

    // On enemy destroyed
    public event Action<BaseTile> onBuildingDestroyed;
    public void BuildingDestroyed(BaseTile tile)
    {
        if (onBuildingDestroyed != null)
            onBuildingDestroyed(tile);
    }

    // On enemy destroyed
    public event Action<Enemy> onEnemyHurt;
    public void EnemyHurt(Enemy enemy)
    {
        if (onEnemyHurt != null)
            onEnemyHurt(enemy);
    }

    // On enemy destroyed
    public event Action<Enemy> onEnemyDestroyed;
    public void EnemyDestroyed(Enemy enemy)
    {
        if (onEnemyDestroyed != null)
            onEnemyDestroyed(enemy);
    }

    // On enemy destroyed
    public event Action<EnemyData, Vector2> onEnemySpawnOnDeath;
    public void EnemySpawnOnDeath(EnemyData spawn, Vector2 position)
    {
        if (onEnemySpawnOnDeath != null)
            onEnemySpawnOnDeath(spawn, position);
    }

    // On guardian destroyed
    public event Action<DefaultGuardian> onGuardianSpawned;
    public void GuardianSpawned(DefaultGuardian guardian)
    {
        if (onGuardianSpawned != null)
            onGuardianSpawned(guardian);
    }

    // On guardian destroyed
    public event Action<DefaultGuardian> onGuardianDestroyed;
    public void GuardianDestroyed(DefaultGuardian guardian)
    {
        if (onGuardianDestroyed != null)
            onGuardianDestroyed(guardian);
    }

    // Invoked when a bullet is fired
    public event Action<DefaultBullet> onBulletFired;
    public void BulletFired(DefaultBullet bullet)
    {
        if (onBulletFired != null)
            onBulletFired(bullet);
    }

    // Invoked when a building with a rotating piece is placed
    public event Action<Rotator> onRotatorPlaced;
    public void RotatorPlaced(Rotator rotator)
    {
        if (onRotatorPlaced != null)
            onRotatorPlaced(rotator);
    }

    // Invoked when a building with a collector script is placed
    public event Action<BaseTile> onBuildingPlaced;
    public void BuildingPlaced(BaseTile building)
    {
        if (onBuildingPlaced != null)
            onBuildingPlaced(building);
    }

    // Invoked when a building with a collector script is placed
    public event Action<Resource.Type, int> onCollectorHarvested;
    public void CollectorHarvested(Resource.Type type, int amount)
    {
        if (onCollectorHarvested != null)
            onCollectorHarvested(type, amount);
    }

    // Invoked when a building with a collector script is placed
    public event Action<DefaultCollector> onCollectorPlaced;
    public void CollectorPlaced(DefaultCollector collector)
    {
        if (onCollectorPlaced != null)
            onCollectorPlaced(collector);
    }

    // Invoked when a building with a storage script is placed
    public event Action<DefaultStorage> onStoragePlaced;
    public void StoragePlaced(DefaultStorage storage)
    {
        if (onStoragePlaced != null)
            onStoragePlaced(storage);
    }

    // Invoked when a building with a turret script is placed
    public event Action<DefaultTurret> onTurretRegistered;
    public void RegisterTurret(DefaultTurret turret)
    {
        if (onTurretRegistered != null)
            onTurretRegistered(turret);
    }

    // Invoked when a hotbar is set
    public event Action<Tile, int> onHotbarSet;
    public void HotbarSet(Tile tile, int slot)
    {
        if (onHotbarSet != null)
            onHotbarSet(tile, slot);
    }

    // Initializes variants
    public event Action<Variant> onVariantLoaded;
    public void VariantLoaded(Variant variant)
    {
        if (onVariantLoaded != null)
            onVariantLoaded(variant);
    }
}