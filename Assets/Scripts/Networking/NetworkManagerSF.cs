using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
//using HeathenEngineering.SteamAPI;

public class NetworkManagerSF : NetworkManager
{

    public static NetworkManagerSF active;

    private bool isServer = true;

    public override void Start()
    {
        active = this;
        base.Start();
    }

    public void Join(string id)
    {
        // Set address to steamid64
        isServer = false;
        networkAddress = id;

        Debug.Log("[SERVER] Attempting connection to " + id);

        // Connect
        StartClient();
    }

    public override void OnStartServer()
    {
        print("Start server");
        if (!isServer) return;
        //SteamSettings.Client.SetRichPresence("clientOf", SteamSettings.Client.user.id.ToString());
    }

    public override void OnStopServer()
    {
        if (!isServer) return;
        //SteamSettings.Client.ClearRichPresence();
    }

    public override void OnStartClient()
    {
        if (!isServer)
        {
            print("Started client");
            //SteamSettings.Client.SetRichPresence("clientOf", networkAddress);
        }
    }

    public override void OnStopClient()
    {
        //SteamSettings.Client.ClearRichPresence();
    }

    public override void OnClientDisconnect(NetworkConnection networkConnection)
    {
        if (Communicator.active != null)
            Communicator.active.SyncClientDisconnect();
    }
}