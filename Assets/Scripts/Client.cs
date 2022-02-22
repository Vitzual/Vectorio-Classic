using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Client : NetworkBehaviour
{
    // Active instance
    public static Client active;

    // On start, set active
    public void Start()
    {
        if (hasAuthority)
        {
            Debug.Log("Settings active instance");
            active = this;
            Gamemode.active.Setup();
        }
    }

    // Sends outgoing request to server to validate a buildable can be placed
    [Command]
    public void CmdCreateBuildable(string buildable_ID, string cosmetic_ID, 
        Vector2 position, Quaternion rotation, bool runChecks, int metadata)
    {
        // Create a new building 
        Server.SrvCreateBuildable(buildable_ID, cosmetic_ID, position, rotation, runChecks, metadata);
    }

    // If server validates building request, calls this function on all clients
    [ClientRpc]
    public void RpcCreateBuildable(string buildable_ID, string cosmetic_ID, 
        Vector2 position, Quaternion rotation, int runtimeID, int metadata)
    {
        // Create the new building instance
        BaseEntity newBuilding = InstantiationHandler.active.InstantiateBuilding(buildable_ID, cosmetic_ID, position, rotation, metadata, -1);

        // Assign the passed down runtime ID
        Server.AssignRuntimeID(newBuilding, runtimeID);
    }
}
