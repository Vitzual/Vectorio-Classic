using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 

public class Syncer : NetworkBehaviour
{
    [Command]
    public void CmdSyncEntity(string id, Vector2 position, Quaternion rotation)
    {

    }

    public void RequestMatchInfo()
    {

    }
}
