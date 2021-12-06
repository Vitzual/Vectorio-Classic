using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 

public class Syncer : NetworkBehaviour
{
    public Dictionary<int, BaseEntity> active = new Dictionary<int, BaseEntity>();

    // Setup new list
    public void Setup()
    {
        active = new Dictionary<int, BaseEntity>();
    }

    [Command]
    public void CmdSyncEntity(string id, Vector2 position, Quaternion rotation)
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
    public void RpcSyncEntity(string id, Vector2 position, Quaternion rotation)
    {

    }

    [ClientRpc]
    public void RpcSyncHealth(int id)
    {

    }

    [ClientRpc]
    public void RpcSyncDestroy(int id)
    {

    }

    [ClientRpc]
    public void RpcSyncMetadata(int id, int metadata)
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
