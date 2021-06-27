using UnityEngine;

public class StorageAI: TileClass
{
    // Declare local object variables
    public int type;
    public int amount = 0;
    public GameObject icon;
    public bool isFull = false;
    private Survival SRVSC;
    private DroneManager droneManager;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        // Default values
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        BuildingHandler.buildings.Add(transform);
        BuildingHandler.storages.Add(this);
        droneManager = GameObject.Find("Drone Handler").GetComponent<DroneManager>();
        droneManager.updateResourceDrones(transform);

        // Add the storage
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
    }

    public void enableIcon()
    {
        icon.SetActive(true);
    }

    public void disableIcon()
    {
        icon.SetActive(false);
    }

    public int takeResources(int input)
    {
        int leftOver = 0;
        droneManager.forceUpdateResourceDrones();

        if (amount >= input)
        {
            amount -= input;
            leftOver = 0;
        }
        else
        {
            leftOver = input - amount;
            amount = 0;
        }     
        
        isFull = false;
        disableIcon();
        return leftOver;
    }

    public void sendResources(int input)
    {
        switch(type)
        {
            case 1:
                SRVSC.AddGold(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Gold", transform.position);
                return;
            case 2:
                SRVSC.AddEssence(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Essence", transform.position);
                return;
            case 3:
                SRVSC.AddIridium(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Iridium", transform.position);
                return;
            default:
                SRVSC.AddGold(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Gold", transform.position);
                return;
        }
    }

    public int addResources(int input)
    {
        // Get the correct storage value
        int storage = Research.research_gold_storage;
        if (type == 2) storage = Research.research_essence_storage;
        else if (type == 3) storage = Research.research_iridium_storage;

        // Determine if icon should be enabled
        // Return the amount not put in storage
        int holder = amount + input;
        if (holder > storage)
        {
            sendResources(storage - amount);
            enableIcon();
            amount = storage;
            isFull = true;
            return holder - storage;
        }
        else if (holder == storage)
        {
            sendResources(input);
            amount = holder;
            enableIcon();
            isFull = true;
            return 0;
        }
        else
        {
            sendResources(input);
            amount = holder;
            return 0;
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
                SRVSC.UpdateGoldStorage(amount);
                break;
            case 2:
                SRVSC.UpdateEssenceStorage(amount);
                break;
            case 3:
                SRVSC.UpdateIridiumStorage(amount);
                break;
        }

        SRVSC.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
