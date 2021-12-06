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

    [Command]
    public void CmdSyncEnemy(string enemy_id, string variant_id, Vector2 position, Quaternion rotation, float health, float speed)
    {
        // Create new entity
        BaseEntity newEntity = InstantiationHandler.active.CreateEnemy(enemy_id, variant_id, position, rotation, health, speed);

        // If creation successful, sync with all clients
        if (newEntity != null)
        {
            bool validID = false;
            while (!validID) 
            {
                int genID = Random.Range(0, 99999999);
                if (!entities.ContainsKey(genID))
                {
                    validID = true;
                    entities.Add(genID, newEntity);
                    RpcSyncEnemy(genID, enemy_id, variant_id, position, rotation, health, speed);
                }
            }
        }
    }

    [Command]
    public void CmdSyncBuildable(string id, Vector2 position, Quaternion rotation)
    {

    }

    [Command]
    public void CmdSyncGhost(string id, Vector2 position, Quaternion rotation)
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
        // Get scriptable data
        Entity entity = ScriptableLoader.enemies[enemy_id];
        Variant variant = ScriptableLoader.variants[variant_id];

        // Create entity
        BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position, rotation, health, speed);
        entities.Add(entity_id, newEntity);
    }

    [ClientRpc]
    protected void RpcSyncBuildable(string id, Vector2 position, Quaternion rotation)
    {

    }

    [ClientRpc]
    protected void RpcSyncGhost(string id, Vector2 position, Quaternion rotation)
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
}
