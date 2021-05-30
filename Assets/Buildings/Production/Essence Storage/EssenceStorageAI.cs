using UnityEngine;

public class EssenceStorageAI: TileClass
{
    // Declare local object variables
    public int amount;
    private Survival SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        BuildingHandler.buildings.Add(transform);
        SRVSC.essenceStorage += amount + Research.research_essence_storage;
        SRVSC.UI.EssenceStorage.text = SRVSC.essenceStorage + " MAX";
    }

    // Kill defense
    public override void DestroyTile()
    {
        if (Research.research_explosive_storages)
        {
            var colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 20f, 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Enemy Defense"));
            foreach (Collider2D collider in colliders)
            {
                if (collider.tag == "Enemy") collider.GetComponent<EnemyClass>().KillEntity();
                else if (collider.tag == "Enemy Defense") collider.GetComponent<TileClass>().DestroyTile();
            }
        }

        SRVSC.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        SRVSC.UpdateEssenceStorage(amount + Research.research_essence_storage);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
