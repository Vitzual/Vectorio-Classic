using UnityEngine;

public class DefaultStorage : ResourceTile
{
    // Declare local object variables
    public GameObject icon;
    public int amountOverride = 0;

    // Start method
    public void Start()
    {
        Events.active.StoragePlaced(this);

        if (Resource.active != null)
            Resource.active.AddStorage(type, Research.GetStorageAmount(type));
    }

    // Add resource to storage
    public override int AddResources(int amount)
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
            PopupHandler.active.CreatePopup(transform.position, type, "+" + (amount - amountToReturn));
            Resource.active.Add(type, amount - amountToReturn, false);
            return amountToReturn;
        }
        else
        {
            // If does not exceed, add resources and return
            PopupHandler.active.CreatePopup(transform.position, type, "+" + amount);
            Resource.active.Add(type, amount, false);
            return 0;
        }
    }

    // Take resource
    public override int TakeResource()
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
        if (Resource.active != null)
        {
            Resource.active.Remove(type, amount, false);
            Resource.active.RemoveStorage(type, Research.GetStorageAmount(type));
        }

        base.DestroyEntity();
    }
}
