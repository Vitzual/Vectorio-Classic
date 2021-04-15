using UnityEngine;

public class ResearchLab : TileClass
{
    private void Start()
    {

    }

    // Kill defense
    public override void DestroyTile()
    {
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.Euler(90f, 0f, 0f));
        Destroy(gameObject);
    }
}
