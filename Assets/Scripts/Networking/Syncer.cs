using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 

public class Syncer : NetworkBehaviour
{
    public static Syncer active;
    public Dictionary<int, BaseEntity> entities = new Dictionary<int, BaseEntity>();

    // Setup new list
    public void Setup()
    {
        entities = new Dictionary<int, BaseEntity>();
    }

    [ServerCallback]
    public void CmdSyncEnemy(string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Check authority
        if (!hasAuthority) return;

        // Create new entity
        BaseEntity newEntity = InstantiationHandler.active.CreateEnemy(enemy_id, variant_id, position, rotation, health, speed);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            int genID = AssignRuntimeID(newEntity);
            RpcSyncEnemy(genID, enemy_id, variant_id, position, rotation, health, speed);
        }
    }

    [Command]
    public void CmdSyncBuildable(string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Create new buildable
        BaseEntity newEntity = InstantiationHandler.active.CreateBuilding(id, position, rotation, metadata);

        // Check if entity created successfully 
        if (newEntity != null)
        {
            int genID = AssignRuntimeID(newEntity);
            RpcSyncBuildable(genID, id, position, rotation, metadata);
        }
    }

    [Command]
    public void CmdSyncGhost(string id, Vector2 position, Quaternion rotation, int metadata)
    {
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
            RpcSyncGhost(genID, id, position, rotation, metadata);
        }
    }

    [Command]
    public void CmdSyncBullet(int id)
    {

    }

    [Command]
    public void CmdSyncHealth(int id)
    {

    }

    [Command]
    public void CmdSyncDestroy(int id)
    {

    }

    [Command]
    public void CmdSyncMetadata(int id, int metadata)
    {

    }

    [ClientRpc]
    protected void RpcSyncEnemy(int entity_id, string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Check if client
        if (isClientOnly)
        {
            // Get scriptable data
            Entity entity = ScriptableLoader.enemies[enemy_id];
            Variant variant = ScriptableLoader.variants[variant_id];

            // Create entity
            BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position, rotation, health, speed);
            entities.Add(entity_id, newEntity);
        }
    }

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
    protected void RpcSyncGhost(int entity_id, string id, Vector2 position, Quaternion rotation, int metadata)
    {
        // Check if client
        if (isClientOnly)
        {
            // Remove ghost tile
            InstantiationHandler.active.RpcDestroyBuilding(position);

            // Sync buildable for all clients
            RpcSyncBuildable(entity_id, id, position, rotation, metadata);
        }
    }

    [ClientRpc]
    protected void RpcSyncBullet(int id)
    {

    }

    [ClientRpc]
    protected void RpcSyncHealth(int id)
    {

    }

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
