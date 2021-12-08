using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 

public class Syncer : NetworkBehaviour
{
    // Active instance
    public static Syncer active;

    // Add self to syncer list
    public static RpcReceiver receiver;

    // All active entities in scene
    public Dictionary<int, BaseEntity> entities = new Dictionary<int, BaseEntity>();

    // Awake
    public void Awake()
    {
        entities = new Dictionary<int, BaseEntity>();

        active = this;
        Setup();
    }

    // Add receiver
    public static bool ConnectReceiver(RpcReceiver rpcReceiver)
    {
        if (receiver == null)
        {
            receiver = rpcReceiver;
            return true;
        }
        return false;
    }

    // Setup new list
    public void Setup()
    {
        entities = new Dictionary<int, BaseEntity>();
    }

    [Server]
    public void SrvSyncEnemy(string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Create new entity
        BaseEntity newEntity = InstantiationHandler.active.CreateEnemy(enemy_id, variant_id, position, rotation, health, speed);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            int genID = AssignRuntimeID(newEntity);
            newEntity.runtimeID = genID;
            receiver.RpcSyncEnemy(genID, enemy_id, variant_id, position, rotation, health, speed);
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
            int genID = AssignRuntimeID(newEntity);
            newEntity.runtimeID = genID;
            receiver.RpcSyncBuildable(genID, id, position, rotation, metadata, Gamemode.active.useDroneConstruction);
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
            int genID = AssignRuntimeID(newEntity);
            newEntity.runtimeID = genID;
            receiver.RpcSyncGhost(old_id, genID, id, position, rotation, metadata);
        }
    }

    [Command]
    public void CmdSyncDestroy(int id)
    {

    }

    [Command]
    public void CmdSyncMetadata(int id, int metadata)
    {

    }

    /*
    [ClientRpc]
    protected void RpcSyncBuildable(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Check if client
        if (isClientOnly)
        {
            // Get building SO via ID request
            Building building = ScriptableLoader.buildings[id];
            if (building == null) return;

            // Get buildable via building SO
            Buildable buildable = Buildables.RequestBuildable(building);
            if (buildable == null) return;

            // Create the new entity
            BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateBuilding(buildable, position, rotation, metadata, -1);
            entities.Add(entity_id, newEntity);
        }
    }

    [ClientRpc]
    protected void RpcSyncGhost(int old_id, int entity_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Check if client
        if (isClientOnly)
        {
            // Remove ghost tile
            if (entities.ContainsKey(old_id))
            {
                entities[old_id].ResetTile();
                entities.Remove(old_id);
            }
            else InstantiationHandler.active.RpcDestroyBuilding(position);

            // Sync buildable for all clients
            RpcSyncBuildable(entity_id, id, position, rotation, metadata);
        }
    }
    */

    [ClientRpc]
    protected void RpcSyncDestroy(int id)
    {

    }

    [ClientRpc]
    protected void RpcSyncMetadata(int id, int metadata)
    {

    }

    [ClientCallback]
    public void CmdRequestMatchInfo()
    {

    }

    [TargetRpc]
    public void RpcRequestMatchInfo()
    {

    }

    
    public int AssignRuntimeID(BaseEntity entity)
    {
        int maxLoop = 100;
        while (maxLoop != 0)
        {
            maxLoop -= 1;
            int genID = Random.Range(0, 99999999);
            if (!entities.ContainsKey(genID))
            {
                entities.Add(genID, entity);
                return genID;
            }
        }
        Debug.LogError("A UUID could not be created for entity!");
        return -1;
    }
}
