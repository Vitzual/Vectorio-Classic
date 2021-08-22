using UnityEngine;

public class ResearchLab : BaseBuilding
{
    private void Start()
    {
        Research.LabsAvailable += 1;
        GameObject.Find("Research").GetComponent<Research>().UpdateAvailable();
    }
}
