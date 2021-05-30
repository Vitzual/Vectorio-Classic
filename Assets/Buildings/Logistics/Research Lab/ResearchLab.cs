using UnityEngine;

public class ResearchLab : TileClass
{
    private void Start()
    {
        Research.LabsAvailable += 1;
    }

    // Kill defense
    public override void DestroyTile()
    {
        Research.LabsAvailable -= 1;
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
