using UnityEngine;

public class StorageAI: TileClass
{
    // Declare local object variables
    public int type;
    public int goldInside = 0;
    private Survival SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        BuildingHandler.buildings.Add(transform);
        switch (type)
        {
            case 1:
                SRVSC.goldStorage += Research.research_gold_storage;
                SRVSC.UI.GoldStorage.text = SRVSC.goldStorage + " MAX";
                return;
            case 2:
                SRVSC.essenceStorage += Research.research_essence_storage;
                SRVSC.UI.EssenceStorage.text = SRVSC.essenceStorage + " MAX";
                return;
            case 3:
                SRVSC.iridiumStorage += Research.research_iridium_storage;
                SRVSC.UI.IridiumStorage.text = SRVSC.iridiumStorage + " MAX";
                return;
        }

        GameObject.Find("Drone Handler").GetComponent<DroneManager>().updateResourceDrones(transform);
    }

    public int getStorageAmount()
    {
        switch (type)
        {
            case 1:
                return Research.research_gold_storage;
            case 2:
                return Research.research_essence_storage;
            case 3:
                return Research.research_iridium_storage;
            default:
                return Research.research_gold_storage;
        }
    }

    public Transform getPosition()
    {
        return transform;
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

        switch (type)
        {
            case 1:
                SRVSC.UpdateGoldStorage(Research.research_gold_storage);
                break;
            case 2:
                SRVSC.UpdateEssenceStorage(Research.research_gold_storage);
                break;
            case 3:
                SRVSC.UpdateIridiumStorage(Research.research_gold_storage);
                break;
        }

        SRVSC.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
