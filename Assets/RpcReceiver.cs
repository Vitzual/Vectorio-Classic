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
    public Communicator communicator;
    public EnemySpawner spawner;
    public bool primaryReceiver = false;

    // Connect to syncer
    public void Start()
    {
        if (hasAuthority)
            CmdSetupReceiver(transform);
    }

    [Command]
    public void CmdSetupReceiver(Transform transform)
    {
        if (Server.primaryReceiver == null)
        {
            Server.primaryReceiver = transform.GetComponent<RpcReceiver>();
            RpcSetupPrimaryReceiver();
        }
        else RpcSetupClientReceiver(Gamemode.seed);
    }

    [TargetRpc]
    public void RpcSetupPrimaryReceiver()
    {
        spawner.enabled = true;
        Gamemode.active.Setup();
    }

    [TargetRpc]
    public void RpcSetupClientReceiver(string seed)
    {
        spawner.enabled = false;
        Gamemode.seed = seed;
        Gamemode.active.SyncSetup();
    }

    [ClientRpc]
    public void RpcSyncEnemy(int entity_id, string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Check if client
        if (isClientOnly && !Server.entities.ContainsKey(entity_id))
        {
            // Get scriptable data
            Entity entity = ScriptableLoader.enemies[enemy_id];
            Variant variant = ScriptableLoader.variants[variant_id];

            // Create entity
            BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position, rotation, health, speed);

            // Check if key already exists
            Server.entities.Add(entity_id, newEntity);
        }
    }

    [ClientRpc]
    public void RpcSyncBuildable(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata, bool useDrones)
    {
        // Check if client
        if (isClientOnly && !Server.entities.ContainsKey(entity_id))
        {
            // Create networked building
            CreateNetworkedBuilding(entity_id, id, position, rotation, metadata, useDrones);
        }
    }

    [ClientRpc]
    public void RpcSyncGhost(int old_id, int entity_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Check if client
        if (isClientOnly && !Server.entities.ContainsKey(entity_id))
        {
            // Remove ghost tile
            if (Server.entities.ContainsKey(old_id))
            {
                Server.entities[old_id].ResetTile();
                Server.entities.Remove(old_id);
            }
            else
            {
                Debug.Log("[SERVER] Desync detected. An entity with runtime ID " + old_id + " was destroyed on" +
                    " the server, but that runtime ID does not exist on this client!");
                InstantiationHandler.active.RpcDestroyBuilding(position);
            }

            // Sync buildable for all clients
            CreateNetworkedBuilding(entity_id, id, position, rotation, metadata, false);
        }
    }

    [ClientRpc]
    public void RpcSyncDestroy(int entity_id)
    {
        // Check client
        if (!isClientOnly) return;

        // Attempt to destroy an active entity. If no entity found, attempt override on position
        if (Server.entities.ContainsKey(entity_id))
        {
            Server.entities[entity_id].DestroyEntity();
            Server.entities.Remove(entity_id);
        }
        else Debug.Log("[SERVER] Desync detected. An entity with ID " + entity_id + " was removed " +
            "on the server, but that entities runtime ID does not exist on this client!");
    }

    // Create a networked building
    public void CreateNetworkedBuilding(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata, bool useDrone)
    {
        // Check entity ID
        if (entity_id == -1)
        {
            Debug.Log("[SERVER] Receiver desync detected. An entity was created on the server, but" +
                " the ID passed to this client is uninitialized. This will cause issues with syncing!");
        }

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

        // Set entity runtime ID
        newEntity.runtimeID = entity_id;

        // Parse entity runtime ID to active dictionary
        Server.entities.Add(entity_id, newEntity);
    }
}
