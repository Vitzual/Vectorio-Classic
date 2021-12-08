using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RpcReceiver : NetworkBehaviour
{
    // Enemy spawner
    //
    // Only instantiates if a client is actively connected
    // If just server, will not be instantiated 
    public EnemySpawner spawner;

    // Connect to syncer
    public void Start()
    {
        if (Syncer.ConnectReceiver(this))
            Instantiate(spawner, Vector2.zero, Quaternion.identity);
    }

    [ClientRpc]
    public void RpcSyncEnemy(int entity_id, string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Check if client
        if (isClientOnly && !Syncer.active.entities.ContainsKey(entity_id))
        {
            // Get scriptable data
            Entity entity = ScriptableLoader.enemies[enemy_id];
            Variant variant = ScriptableLoader.variants[variant_id];

            // Create entity
            BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position, rotation, health, speed);

            // Check if key already exists
            Syncer.active.entities.Add(entity_id, newEntity);
        }
    }

    [ClientRpc]
    public void RpcSyncBuildable(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata, bool useDrones)
    {
        // Check if client
        if (isClientOnly && !Syncer.active.entities.ContainsKey(entity_id))
        {
            // Create networked building
            CreateNetworkedBuilding(entity_id, id, position, rotation, metadata, useDrones);
        }
    }

    [ClientRpc]
    public void RpcSyncGhost(int old_id, int entity_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Check if client
        if (isClientOnly && !Syncer.active.entities.ContainsKey(entity_id))
        {
            // Remove ghost tile
            if (Syncer.active.entities.ContainsKey(old_id))
            {
                Syncer.active.entities[old_id].ResetTile();
                Syncer.active.entities.Remove(old_id);
            }
            else InstantiationHandler.active.RpcDestroyBuilding(position);

            // Sync buildable for all clients
            CreateNetworkedBuilding(entity_id, id, position, rotation, metadata, false);
        }
    }

    // Create a networked building
    public void CreateNetworkedBuilding(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata, bool useDrone)
    {
        // Create network buildable
        Debug.Log("[SERVER] Receiving request to create networked buildable " + entity_id);

        // Get building SO via ID request
        Building building = ScriptableLoader.buildings[id];
        if (building == null) return;

        // Get buildable via building SO
        Buildable buildable = Buildables.RequestBuildable(building);
        if (buildable == null) return;

        // Create the new entity
        BaseEntity newEntity;
        if (useDrone) newEntity = InstantiationHandler.active.RpcInstatiateGhost(buildable, position, rotation, metadata);
        else newEntity = InstantiationHandler.active.RpcInstantiateBuilding(buildable, position, rotation, metadata, -1);

        // Check if key already exists
        if (!Syncer.active.entities.ContainsKey(entity_id))
            Syncer.active.entities.Add(entity_id, newEntity);
        else Syncer.active.entities[entity_id] = newEntity;
    }
}
