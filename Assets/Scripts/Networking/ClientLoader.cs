using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;

public class ClientLoader : NetworkBehaviour
{
    // Load data
    public class DataChunk
    {
        public string[] internalID;
        public string[] cosmeticID;
        public int[] runtimeID;
        public int[] metadataID;
        public float[] entityHealth;
        public float[] xCoord;
        public float[] yCoord;
        public string[] entityType;
        public int[] entityVar;
        public bool[] isGhost;
        public bool isAtEnd = false;
        public int currentIteration = 0;
    }
    public DataChunk loadData;
    public int arraySize = 250;
    public bool isLoading;

    public int totalProcessedAmount = 0;
    public int totalAmountToProcess = 0;

    // Start method
    public void Start()
    {
        if (hasAuthority && isClientOnly)
        {
            SetupClient();
        }
    }

    // Setup a new client
    public void SetupClient()
    {
        if (hasAuthority)
        {
            Debug.Log("[SERVER] Beginning match initial request from server...");
            RequestInitialSetup();
            Debug.Log("[SERVER] Beginning match info request from server...");
            CmdRequestMatchInfo(0);
        }
    }

    // Update method
    public void Update()
    {
        if (hasAuthority)
        {
            if (isLoading && loadData != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    LoadServerData(loadData.currentIteration);
                    loadData.currentIteration += 1;
                    totalProcessedAmount += 1;

                    if (loadData.currentIteration >= arraySize)
                    {
                        if (loadData.isAtEnd)
                        {
                            Debug.Log("[SERVER] All data chunks processed, resuming game.");
                            TogglePlayerJoining(false);
                            loadData = null;
                            RequestFinalSetup();
                        }
                        else
                        {
                            loadData = null;
                            CmdUpdateJoinProgress((float)totalProcessedAmount / (float)totalAmountToProcess);
                            Debug.Log("[SERVER] Data chunk " + (totalProcessedAmount / arraySize) + " processed, requesting next chunk...");
                            CmdRequestMatchInfo(totalProcessedAmount);
                        }
                        break;
                    }
                }
            }
        }
    }

    // Request initial match information from server
    public void RequestFinalSetup()
    {
        if (hasAuthority)
            CmdRequestFinalSetup();
    }

    // Request initial match information from server
    [Command]
    public void CmdRequestFinalSetup()
    {
        // Get final data for client
        List<string> unlockedIDs = new List<string>();
        foreach (KeyValuePair<Entity, Buildable> entity in Buildables.active)
            if (entity.Value.isUnlocked) unlockedIDs.Add(entity.Key.InternalID);

        // Get resources for client
        List<int> resourceID = new List<int>();
        List<int> resourceAmount = new List<int>();
        List<int> resourceStorages = new List<int>();

        // Iterate through resources
        foreach(KeyValuePair<Resource.Type, Resource.Currency> currency in Resource.active.currencies)
        {
            resourceID.Add((int)currency.Key);
            resourceAmount.Add(Resource.active.GetAmount(currency.Key));
            resourceStorages.Add(Resource.active.GetStorage(currency.Key));
        }

        // Rpc info back to requesting client
        RpcRequestFinalSetup(unlockedIDs.ToArray(), resourceID.ToArray(), resourceAmount.ToArray(), resourceStorages.ToArray());
    }

    // Request initial match information from server
    [TargetRpc]
    public void RpcRequestFinalSetup(string[] unlocked, int[] resources, int[] resourceAmounts, int[] resourceStorages)
    {
        // Iterate through unlockable ID's and unlock for client
        foreach (string unlock in unlocked) 
        {
            if (!ScriptableLoader.buildings.ContainsKey(unlock))
            {
                Debug.Log("[SERVER] Returned ID " + unlock + " to unlock that this client does not have reference" +
                    " to, check version and verify game files!");
                continue;
            }
            Buildable buildable = Buildables.RequestBuildable(ScriptableLoader.buildings[unlock]);
            if (buildable != null) Buildables.UnlockBuildable(buildable);
            else Debug.Log("[SERVER] Could not apply unlock for ID " + unlock + "!");
        }

        //Iterate through resources and sync with host
        for (int i = 0; i < resources.Length; i++)
        {
            try
            {
                Resource.Type type = (Resource.Type)resources[i];
                Resource.active.SetStorage(type, resourceStorages[i]);
                Resource.active.SetAmount(type, resourceAmounts[i]);
            }
            catch
            {
                Debug.Log("[SERVER] Resource type returned from host does not exist on this client, " +
                    "check version and verify game files!");
                continue;
            }
        }
    }

    // Request initial match information from server
    public void RequestInitialSetup()
    {
        if (hasAuthority)
            CmdRequestInitialSetup();
    }

    // Request initial match information from server
    [Command]
    public void CmdRequestInitialSetup()
    {
        RpcRequestInitialSetup(0, Server.entities.Count, Gamemode.stage.InternalID);
    }

    // Request initial match information from server
    [TargetRpc]
    public void RpcRequestInitialSetup(int start, int end, string stage_id)
    {
        totalProcessedAmount = start;
        totalAmountToProcess = end;

        if (ScriptableLoader.stages.ContainsKey(stage_id))
        {
            Stage stage = ScriptableLoader.stages[stage_id];
            Gamemode.stage = stage;
            Border.UpdateStage();
            Events.active.ChangeBorderColor(stage.borderOutline, stage.borderFill);
        }
        else Debug.Log("[SERVER] Stage ID returned from server does not exist on this client. Are you" +
            " sure you're up to date? (verify game files)");
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

    // Loads in loadData from specific index
    public void LoadServerData(int i)
    {
        // Try catch to avoid errors
        try
        {
            // Check if index is available
            if (loadData.entityType[i] == null) return;

            // Check what type the entity is
            else if (loadData.entityType[i] == "Building")
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

                // Get building position
                Vector2 position = new Vector2(loadData.xCoord[i], loadData.yCoord[i]);

                // Create either ghost or building
                BaseEntity newEntity;
                if (loadData.isGhost[i]) newEntity = InstantiationHandler.active.InstantiateGhost(buildable, loadData.cosmeticID[i],
                    position, Quaternion.identity, loadData.metadataID[i]);
                else newEntity = InstantiationHandler.active.InstantiateBuildings(buildable, loadData.cosmeticID[i],
                    position, Quaternion.identity, loadData.metadataID[i], loadData.entityHealth[i]);

                // Set entity runtime ID
                int runtime_id = loadData.runtimeID[i];
                newEntity.runtimeID = runtime_id;

                // Parse entity runtime ID to active dictionary
                if (Server.entities.ContainsKey(runtime_id))
                    Server.entities[runtime_id] = newEntity;
                else Server.entities.Add(runtime_id, newEntity);

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
                EnemyData enemy = ScriptableLoader.enemies[loadData.internalID[i]];

                // Create entity
                Vector2 position = new Vector2(loadData.xCoord[i], loadData.yCoord[i]);
                BaseEntity newEntity = EnemyHandler.active.CreateEnemy(enemy, Gamemode.stage.variant, position,
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
    public void RpcSetupClient(string[] internalID, string[] cosmeticID, int[] runtimeID, int[] metadataID, float[] entityHealth,
        float[] xCoord, float[] yCoord, string[] entityType, int[] entityVar, bool[] isGhost, bool isAtEnd)
    {
        Debug.Log("[SERVER] Server returned match info request, creating new data chunk...");

        loadData = new DataChunk();
        loadData.internalID = internalID;
        loadData.cosmeticID = cosmeticID;
        loadData.runtimeID = runtimeID;
        loadData.metadataID = metadataID;
        loadData.entityHealth = entityHealth;
        loadData.xCoord = xCoord;
        loadData.yCoord = yCoord;
        loadData.entityType = entityType;
        loadData.entityVar = entityVar;
        loadData.isGhost = isGhost;
        loadData.isAtEnd = isAtEnd;
        
        TogglePlayerJoining(true);
    }

    // Request match info
    [Command]
    public void CmdRequestMatchInfo(int startingIndex)
    {
        Debug.Log("[SERVER] Preparing match packet for client request...");

        int endIndex = arraySize + startingIndex;
        bool isAtEnd = false;

        string[] internalID = new string[arraySize];
        string[] cosmeticID = new string[arraySize];
        int[] runtimeID = new int[arraySize];
        int[] metadataID = new int[arraySize];
        float[] entityHealth = new float[arraySize];
        float[] xCoord = new float[arraySize];
        float[] yCoord = new float[arraySize];
        string[] entityType = new string[arraySize];
        int[] entityVar = new int[arraySize];
        bool[] isGhost = new bool[arraySize];

        int index = 0;
        for(int i = startingIndex; i < endIndex; i++)
        {
            // Check if still within range
            if (i >= Server.entities.Count)
            {
                isAtEnd = true;
                break;
            }

            KeyValuePair<int, BaseEntity> entity = Server.entities.ElementAt(i);
            if (entity.Value == null) continue;

            internalID[index] = entity.Value.internalID;
            if (entity.Value.cosmetic != null)
                cosmeticID[index] = entity.Value.cosmetic.InternalID;
            else cosmeticID[index] = "";
            runtimeID[index] = entity.Key;
            entityHealth[index] = entity.Value.health;
            xCoord[index] = entity.Value.transform.position.x;
            yCoord[index] = entity.Value.transform.position.y;

            Enemy enemy = entity.Value.GetComponent<Enemy>();
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

        Debug.Log("[SERVER] Match segment formatted, sending back to client.");
        RpcSetupClient(internalID, cosmeticID, runtimeID, metadataID, entityHealth, xCoord, yCoord, entityType, entityVar, isGhost, isAtEnd);
    }
}
