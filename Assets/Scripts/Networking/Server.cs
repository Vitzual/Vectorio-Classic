using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 
public class Server : NetworkBehaviour
{
    // All active entities in scene
    public static Dictionary<int, BaseEntity> entities = new Dictionary<int, BaseEntity>();
    public void Awake() { entities = new Dictionary<int, BaseEntity>(); }

    // Attempts to validate and create a building. 
    // If successful, forces all clients to do the same.
    [Server]
    public static void SrvCreateBuildable(string buildable_ID, string cosmetic_ID,
    Vector2 position, Quaternion rotation, bool runChecks, int metadata)
    {
        // Create a new building variable
        BaseEntity newBuilding;

        // Create building with or without checks
        if (runChecks) newBuilding = InstantiationHandler.active.CreateBuilding(buildable_ID, cosmetic_ID, position, rotation, metadata);
        else newBuilding = InstantiationHandler.active.InstantiateBuilding(buildable_ID, cosmetic_ID, position, rotation, metadata, -1);

        // If successful, assign runtime ID to building and sync clients
        if (newBuilding != null)
        {
            AssignRuntimeID(newBuilding);
            Client.active.RpcCreateBuildable(buildable_ID, cosmetic_ID, position, rotation, newBuilding.runtimeID, -1);
        }
    }

    // Assigns a unique runtime ID to an entity    
    public static void AssignRuntimeID(BaseEntity entity, int overrideID = -1)
    {
        // Generate new runtime ID if not specified
        if (overrideID == -1)
            overrideID = GenerateRuntimeID();

        // Add building and ID to server entity list
        entities.Add(overrideID, entity);
        entity.runtimeID = overrideID;
    }

    // Generates a runtime ID 
    public static int GenerateRuntimeID()
    {
        int maxLoop = 100;
        while (maxLoop != 0)
        {
            maxLoop -= 1;
            int genID = Random.Range(0, 99999999);
            if (!entities.ContainsKey(genID))
                return genID;
        }
        return -1;
    }
}
