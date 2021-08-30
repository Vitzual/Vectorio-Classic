using UnityEngine;

public class Lab : BaseBuilding
{
    private void Start()
    {
        Research.LabsAvailable += 1;
        GameObject.Find("Research").GetComponent<Research>().UpdateAvailable();
    }
}
