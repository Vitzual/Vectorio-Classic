using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class ClientLoader : NetworkBehaviour
{
    // Load data
    public class LoadData
    {
        public string matchInfo;
        public string[] internalID;
        public int[] runtimeID;
        public int[] metadataID;
        public float[] entityHealth;
        public float[] xCoord;
        public float[] yCoord;
        public string[] entityType;
        public int[] entityVar;

        public int totalIterations = 0;
        public int currentIteration = 0;
    }
    public LoadData loadData;
    public bool isLoading;

    // Start method
    public void Start()
    {
        if (hasAuthority && isClientOnly)
        {
            SetupClient();
        }
    }

    // Update method
    public void Update()
    {
        if (hasAuthority)
        {
            if (isLoading && loadData != null)
            {
                LoadServerData(loadData.currentIteration);
                loadData.currentIteration += 1;

                if (loadData.currentIteration >= loadData.totalIterations)
                {
                    TogglePlayerJoining(false);
                    loadData = null;
                }
                else CmdUpdateJoinProgress((float)loadData.currentIteration / (float)loadData.totalIterations);
            }
            //else if (isLoading) TogglePlayerJoining(false);
        }
    }

    // Enable load screen for all players
    [Command]
    public void CmdUpdateJoinProgress(float percentage)
    {
        RpcUpdateJoinProgress(percentage);
    }

    // Enable load screen for client
    [ClientRpc]
    public void RpcUpdateJoinProgress(float percentage)
    {
        UIEvents.active.UpdateJoinProgress(percentage);
    }

    // Toggle player joining screen
    public void TogglePlayerJoining(bool enabled)
    {
        if (hasAuthority)
        {
            isLoading = enabled;
            CmdToggleLoadScreen(enabled);
        }
    }

    // Enable load screen for all players
    [Command]
    public void CmdToggleLoadScreen(bool enabled)
    {
        RpcToggleLoadScreen(enabled);
    }

    // Enable load screen for client
    [ClientRpc]
    public void RpcToggleLoadScreen(bool enabled)
    {
        Settings.paused = enabled;
        UIEvents.active.PlayerJoin(enabled);
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

    // Loads in loadData from specific index
    public void LoadServerData(int i)
    {
        // Try catch to avoid errors
        try
        {
            // Check what type the entity is
            if (loadData.entityType[i] == "Building")
            {
                // Check building internal ID
                if (!ScriptableLoader.buildings.ContainsKey(loadData.internalID[i]))
                {
                    Debug.Log("[SERVER] Received invalid building key at " + i + "! Skipping");
                    return;
                }

                // Get building SO via ID request
                Building building = ScriptableLoader.buildings[loadData.internalID[i]];
                if (building == null) return;

                // Get buildable via building SO
                Buildable buildable = Buildables.RequestBuildable(building);
                if (buildable == null) return;

                // Create the new entity
                Vector2 position = new Vector2(loadData.xCoord[i], loadData.yCoord[i]);
                BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateBuilding(buildable, position,
                    Quaternion.identity, loadData.metadataID[i], loadData.entityHealth[i]);

                // Set entity runtime ID
                int entity_id = loadData.runtimeID[i];
                newEntity.runtimeID = entity_id;

                // Parse entity runtime ID to active dictionary
                if (Server.entities.ContainsKey(entity_id))
                    Server.entities[entity_id] = newEntity;
                else Server.entities.Add(entity_id, newEntity);

                // Check free variable for resources
                if (loadData.entityVar[i] != -1)
                {
                    ResourceTile tile = newEntity.GetComponent<ResourceTile>();
                    if (tile != null) tile.AddResources(loadData.entityVar[i], false);
                    else Debug.Log("[SERVER] A newly created entity has a resource variable attached to it, " +
                        "but the instantiated counterpart has no way to accept resources!");
                }
            }
            else if (loadData.entityType[i] == "Enemy")
            {
                // Get scriptable data
                Entity entity = ScriptableLoader.enemies[loadData.internalID[i]];
                Variant variant = ScriptableLoader.variants[Gamemode.stage.variant.InternalID];

                // Create entity
                Vector2 position = new Vector2(loadData.xCoord[i], loadData.yCoord[i]);
                BaseEntity newEntity = InstantiationHandler.active.RpcInstantiateEnemy(entity, variant, position,
                    Quaternion.identity, loadData.entityHealth[i], loadData.metadataID[i]);

                // Set entity runtime ID
                int entity_id = loadData.runtimeID[i];
                newEntity.runtimeID = entity_id;

                // Parse entity runtime ID to active dictionary
                if (Server.entities.ContainsKey(entity_id))
                    Server.entities[entity_id] = newEntity;
                else Server.entities.Add(entity_id, newEntity);
            }
            else Debug.Log("[SERVER] Invalid entity type " + loadData.entityType[i] + ", can not process!");
        }
        catch (Exception e)
        {
            Debug.Log("[SERVER] Invalid entity at index " + i + "! Caught error: " + e.StackTrace);
        }
    }

    // Grabs returned info from server
    [TargetRpc]
    public void RpcSetupClient(string matchInfo, string[] internalID, int[] runtimeID, int[] metadataID, float[] entityHealth,
        float[] xCoord, float[] yCoord, string[] entityType, int[] entityVar)
    {
        Debug.Log("[SERVER] Server returned match info request, creating new load data for client.");

        loadData = new LoadData();
        loadData.matchInfo = matchInfo;
        loadData.internalID = internalID;
        loadData.runtimeID = runtimeID;
        loadData.metadataID = metadataID;
        loadData.entityHealth = entityHealth;
        loadData.xCoord = xCoord;
        loadData.yCoord = yCoord;
        loadData.entityType = entityType;
        loadData.entityVar = entityVar;

        Debug.Log("[SERVER] Load data set. Pausing game to load client into match.");

        if (matchInfo != null && ScriptableLoader.stages.ContainsKey(matchInfo))
        {
            Gamemode.stage = ScriptableLoader.stages[matchInfo];
            Debug.Log("[SERVER] Stage info was processed successfully!");
        }
        else Debug.Log("[SERVER] Stage info could not be processed!");

        loadData.currentIteration = 0;
        loadData.totalIterations = loadData.internalID.Length;
        TogglePlayerJoining(true);
    }

    // Request match info
    [Command]
    public void CmdRequestMatchInfo()
    {
        Debug.Log("[SERVER] Match info request received, processing...");

        string stage_id = Gamemode.stage.InternalID;
        string[] internalID = new string[Server.entities.Count];
        int[] runtimeID = new int[Server.entities.Count];
        int[] metadataID = new int[Server.entities.Count];
        float[] entityHealth = new float[Server.entities.Count];
        float[] xCoord = new float[Server.entities.Count];
        float[] yCoord = new float[Server.entities.Count];
        string[] entityType = new string[Server.entities.Count];
        int[] entityVar = new int[Server.entities.Count];
        bool[] isGhost = new bool[Server.entities.Count];

        int index = 0;
        foreach (KeyValuePair<int, BaseEntity> entity in Server.entities)
        {
            if (entity.Value == null) continue;

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

                ResourceTile resourceTile = entity.Value.GetComponent<ResourceTile>();
                if (resourceTile != null) entityVar[index] = resourceTile.amount;
                else entityVar[index] = -1;

                GhostTile ghostTile = entity.Value.GetComponent<GhostTile>();
                if (ghostTile != null) isGhost[index] = true;
                else isGhost[index] = false;
            }
            else
            {
                metadataID[index] = (int)enemy.moveSpeed;
                entityType[index] = "Enemy";
            }

            index += 1;
        }
        Debug.Log("[SERVER] All match info formatted, sending back to client");
        RpcSetupClient(stage_id, internalID, runtimeID, metadataID, entityHealth, xCoord, yCoord, entityType, entityVar);
    }
}
