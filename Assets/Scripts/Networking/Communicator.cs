using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Communicator : NetworkBehaviour
{
    // Active local instance
    public Communicator activePointer;
    public static Communicator active;
    public string permission = "FALSE";

    // Connect to syncer
    public void Start()
    {
        if (hasAuthority)
        {
            permission = "TRUE";
            active = this;
            activePointer = active;
            if (isClientOnly) SetupClient();
        }
    }

    // Setup a new client
    public void SetupClient()
    {
        if (hasAuthority)
        {
            Debug.Log("[SERVER] Beginning match request from server...");
            CmdRequestMatchInfo();
        }
    }

    // Show error to client
    [TargetRpc]
    public void RpcSetupError(string e)
    {
        Debug.Log("[SERVER] Server encountered error during match request. Error: " + e);
    }

    // Setup client
    [TargetRpc]
    public void RpcSetupClient(string matchInfo, string[] internalID, int[] runtimeID, int[] metadataID, float[] entityHealth,
        float[] xCoord, float[] yCoord, string[] entityType, int[] entityVar)
    {
        Debug.Log("[SERVER] Server returned match info request, processing...");

        if (matchInfo != null && ScriptableLoader.stages.ContainsKey(matchInfo))
        {
            Gamemode.stage = ScriptableLoader.stages[matchInfo];
            Debug.Log("[SERVER] Stage info was processed successfully!");
        }
        else Debug.Log("[SERVER] Stage info could not be processed!");

        // Iterate through match info and instantiate missing entities
        for (int i = 0; i < internalID.Length; i++)
        {
            // Check what type the entity is
            if (entityType[i] == "Building")
            {
                // Check building internal ID
                if (!ScriptableLoader.buildings.ContainsKey(internalID[i]))
                {
                    Debug.Log("[SERVER] Received invalid building key at " + i + "! Skipping");
                    continue;
                }

                // Get building SO via ID request
                Building building = ScriptableLoader.buildings[internalID[i]];
                if (building == null) return;

                // Get buildable via building SO
                Buildable buildable = Buildables.RequestBuildable(building);
                if (buildable == null) return;

                // Create the new entity
                Vector2 position = new Vector2(xCoord[i], yCoord[i]);
                BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateBuilding(buildable, position, 
                    Quaternion.identity, metadataID[i], entityHealth[i]);

                // Set entity runtime ID
                int entity_id = runtimeID[i];
                newEntity.runtimeID = entity_id;

                // Parse entity runtime ID to active dictionary
                if (Server.entities.ContainsKey(entity_id))
                    Server.entities[entity_id] = newEntity;
                else Server.entities.Add(entity_id, newEntity);

                // Check free variable for resources
                if (entityVar[i] != -1)
                {
                    ResourceTile tile = newEntity.GetComponent<ResourceTile>();
                    if (tile != null) tile.AddResources(entityVar[i], false);
                    else Debug.Log("[SERVER] A newly created entity has a resource variable attached to it, " +
                        "but the instantiated counterpart has no way to accept resources!");
                }
            }
            else if (entityType[i] == "Enemy")
            {
                // Get scriptable data
                Entity entity = ScriptableLoader.enemies[internalID[i]];
                Variant variant = ScriptableLoader.variants[Gamemode.stage.variant.InternalID];

                // Create entity
                Vector2 position = new Vector2(xCoord[i], yCoord[i]);
                BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position,
                    Quaternion.identity, entityHealth[i], metadataID[i]);

                // Set entity runtime ID
                int entity_id = runtimeID[i];
                newEntity.runtimeID = entity_id;

                // Parse entity runtime ID to active dictionary
                if (Server.entities.ContainsKey(entity_id))
                    Server.entities[entity_id] = newEntity;
                else Server.entities.Add(entity_id, newEntity);
            }
            else Debug.Log("[SERVER] Invalid entity type " + entityType[i] + ", can not process!");
        }
        Debug.Log("[SERVER] Entity info was processed successfully!");
        Debug.Log("[SERVER] Match info fully synced, client is ready.");
    }

    // Request match info
    [Command]
    public void CmdRequestMatchInfo()
    {
        Debug.Log("[SERVER] Match info request received, processing...");

        //try
        //{
        string stage_id = Gamemode.stage.InternalID;
        string[] internalID = new string[Server.entities.Count];
        int[] runtimeID = new int[Server.entities.Count];
        int[] metadataID = new int[Server.entities.Count];
        float[] entityHealth = new float[Server.entities.Count];
        float[] xCoord = new float[Server.entities.Count];
        float[] yCoord = new float[Server.entities.Count];
        string[] entityType = new string[Server.entities.Count];
        int[] entityVar = new int[Server.entities.Count];

        int index = 0;
        foreach (KeyValuePair<int, BaseEntity> entity in Server.entities)
        {
            internalID[index] = entity.Value.internalID;
            runtimeID[index] = entity.Key;
            entityHealth[index] = entity.Value.health;
            xCoord[index] = entity.Value.transform.position.x;
            yCoord[index] = entity.Value.transform.position.y;

            DefaultEnemy enemy = entity.Value.GetComponent<DefaultEnemy>();
            if (enemy == null)
            {
                metadataID[index] = entity.Value.metadata;
                entityType[index] = "Building";
            }
            else
            {
                metadataID[index] = (int)enemy.moveSpeed;
                entityType[index] = "Enemy";

                ResourceTile resourceTile = entity.Value.GetComponent<ResourceTile>();
                if (resourceTile != null) entityVar[index] = resourceTile.amount;
            }

            index += 1;
        }
        Debug.Log("[SERVER] Returning match info to client request");
        RpcSetupClient(stage_id, internalID, runtimeID, metadataID, entityHealth, xCoord, yCoord, entityType, entityVar);
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("[SERVER] Match request ran into error: " + e.Message);
        //    RpcSetupError(e.Message);
        //}
    }

    // Sync via match info
    public void SyncMatchInfo()
    {

    }

    // Internal call
    public void SyncGuardianBattle()
    {
        if (hasAuthority)
            CmdSyncGuardianBattle();
    }

    // Update collector grab for all players
    [Command]
    public void CmdSyncGuardianBattle()
    {
        RpcSyncGuardianBattle();
    }

    // Rpc metadata on all clients
    [ClientRpc]
    public void RpcSyncGuardianBattle()
    {
        // Attempt to start battle for all clients
        GuardianHandler.active.StartGuardianBattle();
    }


    // Internal call
    public void SyncMetadata(int id, int data)
    {
        if (hasAuthority)
            CmdSyncMetadata(id, data);
    }

    // Update collector grab for all players
    [Command]
    public void CmdSyncMetadata(int id, int data)
    {
        RpcSyncMetadata(id, data);
    }

    // Rpc metadata on all clients
    [ClientRpc]
    public void RpcSyncMetadata(int entity_id, int metadata)
    {
        // Attempt to destroy an active entity. If no entity found, attempt override on position
        if (Server.entities.ContainsKey(entity_id))
            Server.entities[entity_id].ApplyMetadata(metadata);
        else Debug.Log("[SERVER] Desync detected. An entity with ID " + entity_id + " received a " +
            "metadata change of " + metadata + " from the server, but the entities runtime ID does not exist on this client!");
    }

    // Internal call
    public void SyncCollector(int id, int amount)
    {
        if (hasAuthority)
            CmdCollector(id, amount);
    }

    // Update collector grab for all players
    [Command]
    public void CmdCollector(int id, int amount)
    {
        RpcCollect(id, amount);
    }

    // Rpc collector on all clients
    [ClientRpc]
    public void RpcCollect(int id, int amount)
    {
        // Reset the tile 
        if (Server.entities.ContainsKey(id))
            Server.entities[id].SyncEntity(amount);
        else Debug.Log("[SERVER] Client received a collector with a runtime ID that doesn't exist. " +
            "This will cause major issues with desyncing!");
    }
}
