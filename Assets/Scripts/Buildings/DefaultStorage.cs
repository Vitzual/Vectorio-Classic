using UnityEngine;

public class DefaultStorage: DefaultBuilding
{
    // Declare local object variables
    public int type;
    public int amount = 0;
    public bool isFull = false;
    public GameObject icon;

    // On start, invoke repeating SendGold() method
    public void Start()
    {
        // Default values
        Events.active.StoragePlaced(this);

        // Add the storage
        switch (type)
        {
            case 1:
                Resource.AddStorage(Resource.Currency.Gold, Research.research_gold_storage);
                return;
            case 2:
                Resource.AddStorage(Resource.Currency.Essence, Research.research_gold_storage);
                return;
            case 3:
                Resource.AddStorage(Resource.Currency.Iridium, Research.research_gold_storage);
                return;
        }
    }

    public void EnableIcon()
    {
        icon.SetActive(true);
    }

    public void DisableIcon()
    {
        icon.SetActive(false);
    }

    public int TakeResources(int input)
    {
        int leftOver;

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
        DisableIcon();
        return leftOver;
    }

    /*
    public void SendResources(int input)
    {
        switch(type)
        {
            case 1:
                //SRVSC.AddGold(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Gold", transform.position);
                return;
            case 2:
                //SRVSC.AddEssence(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Essence", transform.position);
                return;
            case 3:
                //SRVSC.AddIridium(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Iridium", transform.position);
                return;
            default:
                //SRVSC.AddGold(input);
                SRVSC.UI.CreateResourcePopup("+ " + input, "Gold", transform.position);
                return;
        }
    }
    */

    public int AddResources(int input, bool fromSave = false)
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
            //if (!fromSave) SendResources(storage - amount);
            EnableIcon();
            amount = storage;
            isFull = true;
            return holder - storage;
        }
        else if (holder == storage)
        {
            //if (!fromSave) SendResources(input);
            amount = holder;
            EnableIcon();
            isFull = true;
            return 0;
        }
        else
        {
            //if (!fromSave) SendResources(input);
            amount = holder;
            return 0;
        }
    }

    public Transform GetPosition()
    {
        return transform;
    }

}
