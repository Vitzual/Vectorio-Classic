using UnityEngine;

public class DefaultStorage : ResourceTile
{
    // Declare local object variables
    public GameObject icon;
    public int amountOverride = 0;

    // Awake method
    public void Awake()
    {
        Events.active.StoragePlaced(this);

        if (Resource.active != null) Resource.active.AddStorageObj(this);
        else Debug.Log("Storage could not successfully add to list");
    }

    // Add resource to storage
    public override int AddResources(int amount, bool showPopup)
    {
        // Add amount and grab storage amount
        this.amount += amount;
        int storage = Research.resource[type].storageAmount;

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
            if (showPopup) PopupHandler.active.CreatePopup(transform.position, type, "+" + (amount - amountToReturn));
            Resource.active.Apply(type, amount - amountToReturn, false);
            return amountToReturn;
        }
        else if (this.amount == storage)
        {
            // Set full variables to true
            isFull = true;
            if (icon != null) icon.SetActive(true);

            // Add proper amount and return overflow
            if (showPopup) PopupHandler.active.CreatePopup(transform.position, type, "+" + amount);
            Resource.active.Apply(type, amount, false);
            return 0;
        }
        else
        {
            // If does not exceed, add resources and return
            if (showPopup) PopupHandler.active.CreatePopup(transform.position, type, "+" + amount);
            Resource.active.Apply(type, amount, false);
            return 0;
        }
    }

    // Take resource
    public override int TakeResource()
    {
        // Calculate return amount
        int amountToReturn = amount;
        amount = 0;
        isFull = false;
        if (icon != null) icon.SetActive(false);

        // Update nearby resource ports
        DroneManager.overrideResourceCheck = true;

        // Return
        return amountToReturn;
    }

    // On destroy, override method and remove storage
    public override void DestroyEntity()
    {
        if (Resource.active != null)
        {
            Resource.active.Apply(type, -amount, false);
            if (Resource.storages.Contains(this))
                Resource.storages.Remove(this);
        }

        base.DestroyEntity();
    }
}
