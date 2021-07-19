using UnityEngine;

public class ResearchLab : TileClass
{
    private void Start()
    {
        Research.LabsAvailable += 1;
        GameObject.Find("Research").GetComponent<Research>().UpdateAvailable();
    }

    // Kill defense
    public override void ModifyResearch()
    {
        Research.LabsAvailable -= 1;
    }
}
