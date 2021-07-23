using UnityEngine;

public class LayerManager : MonoBehaviour
{
    // List of layers to get on run
    public string turretLayerName;
    public static LayerMask turretLayer;

    private void Start()
    {
        turretLayer = 1 << LayerMask.NameToLayer(turretLayerName);
    }

    public static LayerMask getTurretLayer()
    {
        return turretLayer;
    }
}
