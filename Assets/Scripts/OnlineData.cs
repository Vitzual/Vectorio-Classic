using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineData
{
    // Constructor
    public OnlineData()
    {
        privateSession = false;
        maxConnections = 10;
        listAsLobby = false;
    }

    // Online data
    public bool privateSession = false;
    public int maxConnections = 10;
    public bool listAsLobby = false;
}
