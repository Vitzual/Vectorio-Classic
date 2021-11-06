using UnityEngine;

public class Storage: BaseTile
{
    // Declare local object variables
    public Resource.CurrencyType type;
    public int amount = 0;
    public bool isFull = false;
    public GameObject icon;

    // On start, invoke repeating SendGold() method
    public void Start()
    {
        Events.active.StoragePlaced(this);
        Resource.active.AddStorage(type, Research.GetStorageAmount(type));
    }

    // Add resource to storage
    public int AddResource(int amount)
    {
        // Add amount and grab storage amount
        this.amount += amount;
        int storage = Research.GetStorageAmount(type);

        // Determine if amount exceeds storage
        if (this.amount > storage)
        {
            // If exceeds, set to max amount
            int amountToReturn = this.amount - storage;
            this.amount = storage;

            // Set full variables to true
            isFull = true;
            if (icon != null) icon.SetActive(true);

            // Add proper amount and return overflow
            Resource.active.Add(type, amount - amountToReturn);
            return amountToReturn;
        }
        else
        {
            // If does not exceed, add resources and return
            Resource.active.Add(type, amount);
            return 0;
        }
    }

    // Take resource
    public int TakeResource()
    {
        int amountToReturn = amount;
        amount = 0;
        isFull = false;
        if (icon != null) icon.SetActive(false);
        return amountToReturn;
    }

    // On destroy, override method and remove storage
    public override void DestroyEntity()
    {
        Resource.active.RemoveStorage(type, Research.GetStorageAmount(type));
    }
}
