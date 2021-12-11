using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;

public class Nameplate : NetworkBehaviour
{
    // Nameplate variables
    [SyncVar(hook = nameof(SetPlateName))]
    public new string name;
    public TextMeshProUGUI displayName;

    [SyncVar(hook = nameof(SetPlateColor))]
    public int color;
    public Image displayIcon;

    public Color[] colors;

    // Change name
    public void Start()
    {
        if (hasAuthority)
        {
            UserData userData = User.Client.Id;
            CmdSetPlateName(userData.Name);
            CmdSetPlateColor(Random.Range(0, colors.Length));
        }
    }

    // Update name server-side
    [Command]
    public void CmdSetPlateName(string name)
    {
        this.name = name;
    }

    // Update name server-side
    [Command]
    public void CmdSetPlateColor(int color)
    {
        this.color = color;
    }

    // Set nameplate
    public void SetPlateName(string _Old, string _New)
    {
        displayName.text = _New;
    }

    // Set nameplate
    public void SetPlateColor(int _Old, int _New)
    {
        displayIcon.color = colors[_New];
    }
}
