using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 

public class Server : NetworkBehaviour
{
    // Active instance
    public static Server active;

    // Add self to syncer list
    public static RpcReceiver primaryReceiver;

    // All active entities in scene
    public static Dictionary<int, BaseEntity> entities = new Dictionary<int, BaseEntity>();

    // Awake
    public void Awake()
    {
        entities = new Dictionary<int, BaseEntity>();

        active = this;
        Setup();
    }

    // Setup new list
    public void Setup()
    {
        entities = new Dictionary<int, BaseEntity>();
    }

    [Server]
    public void SrvSyncCollector(int collector_id, int amount)
    {
        // Reset the tile 
        if (entities.ContainsKey(collector_id))
        {
            entities[collector_id].SyncEntity(amount);
            if (Communicator.active != null) Communicator.active.RpcCollect(collector_id, amount);
            else Debug.Log("[SERVER] No active resource bin found to broadcast to!");
        }
        else Debug.Log("[SERVER] Server received a runtime ID that doesn't exist. This could cause major " +
            "issues with desyncing! Recommend restarting the game.");
    }

    [Server]
    public void SrvSyncEnemy(string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Create new entity
        BaseEntity newEntity = InstantiationHandler.active.CreateEnemy(enemy_id, variant_id, position, rotation, health, speed);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            primaryReceiver.RpcSyncEnemy(newEntity.runtimeID, enemy_id, variant_id, position, rotation, health, speed);
        }
    }

    [Server]
    public void SrvSyncBuildable(string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Create new buildable
        BaseEntity newEntity = InstantiationHandler.active.CreateBuilding(id, position, rotation, metadata);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            primaryReceiver.RpcSyncBuildable(newEntity.runtimeID, id, position, rotation, metadata, Gamemode.active.useDroneConstruction);
        }
    }

    [Server]
    public void SrvSyncGhost(int old_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Reset the tile 
        if (entities.ContainsKey(old_id))
        {
            entities[old_id].ResetTile();
            entities.Remove(old_id);
        }
        else Debug.Log("[SERVER] Received runtime ID that does not exist on the server. This will" +
            " cause issues with desyncing! Recommend restarting the game to avoid further problems");

        // Get building SO via ID request
        Building building = ScriptableLoader.buildings[id];
        if (building == null) return;

        // Get buildable via building SO
        Buildable buildable = Buildables.RequestBuildable(building);
        if (buildable == null) return;

        // Create buildable
        BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateBuilding(buildable, position, rotation, metadata, -1);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            primaryReceiver.RpcSyncGhost(old_id, newEntity.runtimeID, id, position, rotation, metadata);
        }
    }
    
    [Server]
    public void SrvSyncDestroy(int id)
    {
        // Attempt to destroy an active entity. If no entity found, attempt override on position
        if (entities.ContainsKey(id))
        {
            entities[id].DestroyEntity();
            entities.Remove(id);
        }
        primaryReceiver.RpcSyncDestroy(id);
    }

    // Assigns a unique runtime ID to an entity    
    public static int AssignRuntimeID(BaseEntity entity)
    {
        int maxLoop = 100;
        while (maxLoop != 0)
        {
            maxLoop -= 1;
            int genID = Random.Range(0, 99999999);
            if (!entities.ContainsKey(genID))
            {
                entities.Add(genID, entity);
                entity.runtimeID = genID;
                return genID;
            }
        }
        Debug.LogError("[SERVER] A runtime ID could not be created for " + entity + ", this will cause desync issues");
        return -1;
    }
}
