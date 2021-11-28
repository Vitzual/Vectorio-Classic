using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    // Active instance
    public static Resource active;

    // Currency types
    public enum CurrencyType
    {
        Gold,
        Essence,
        Iridium,
        Power,
        Heat
    }

    // Resource currency class
    [System.Serializable]
    public class Currency
    {
        // Currency variables
        public CurrencyType type;
        public int amount;
        public int storage;
        public bool calcPerSecond;
        public string format;
        public bool allowOverflow;

        // UI elements
        public Image background;
        public TextMeshProUGUI resourceUI;
        public TextMeshProUGUI storageUI;
        public TextMeshProUGUI perSecond;

        // Resource bar
        public bool useResourceBar;
        public ProgressBar resourceBar;

        // Default variables
        public float collectionRate;
        public int collectionAmount;
        public int storageAmount;
    }

    // Dictionary of all currencies
    public Dictionary<CurrencyType, Currency> currencies;
    private Dictionary<Currency, int> lastCalculation;
    public Currency[] currencyElements;

    // List of all collectors and storages
    public static List<DefaultStorage> storages = new List<DefaultStorage>();

    // Get active instance
    public void Awake()
    {
        active = this;
        Setup();
    }

    // Generate resources
    public void Setup() 
    {
        // Setup currencies
        currencies = new Dictionary<CurrencyType, Currency>();
        lastCalculation = new Dictionary<Currency, int>();
        Research.resource = new Dictionary<CurrencyType, Research.ResourceBoost>();
        foreach (Currency currency in currencyElements)
        {
            currencies.Add(currency.type, currency);
            Research.GenerateBoost(currency.type, currency.collectionRate, currency.collectionAmount, currency.storageAmount);
            currency.background.color = new Color(1, 1, 1, 0.1f);
            if (currency.calcPerSecond)
                lastCalculation.Add(currency, 0);
        }

        // Setup events
        InvokeRepeating("UpdatePerSecond", 1, 1);
    }

    // Update resources
    public void UpdatePerSecond()
    {
        foreach(Currency currency in currencyElements)
        {
            if (currency.calcPerSecond)
            {
                int amount = currency.amount - lastCalculation[currency];
                if (amount == 0) currency.perSecond.text = "<color=white>" + amount + " / second";
                else if (amount > 0) currency.perSecond.text = "<color=green>+" + amount + " / second";
                else currency.perSecond.text = "<color=red>" + amount + " / second";
                lastCalculation[currency] = currency.amount;
            }
        }
    }

    // Update storages 
    public void UpdateStorages(CurrencyType type, int amount, bool add)
    {
        // Setup local loop variables
        int amountToAdd = amount;
        if (!add) amountToAdd = -amountToAdd;

        // Updates all storages
        for(int i = 0; i < storages.Count; i++)
        {
            // Check if storage is null
            if (storages[i] != null)
            {
                // Check if storage is same type
                if (storages[i].type == type)
                {
                    // If add, add resources
                    if (add)
                    {
                        if (!storages[i].isFull)
                        {
                            amountToAdd = storages[i].AddResources(amountToAdd);
                            if (amountToAdd <= 0) return;
                        }
                    }
                    else
                    {
                        // Update resources
                        int resourcesTaken = storages[i].TakeResource();
                        amountToAdd += resourcesTaken;
                        currencies[type].amount -= resourcesTaken;

                        if (amountToAdd > 0)
                        {
                            storages[i].AddResources(amountToAdd);
                            return;
                        }
                    }
                }
            }
            else if (!Gamemode.loadGame)
            {
                storages.RemoveAt(i);
                i--;
            }
        }
    }

    // Get currency class
    public Currency GetCurrency(CurrencyType type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type];
        else return null;
    }

    // Add a resource
    public void Add(CurrencyType type, int amount, bool useStorages)
    {
        // Heat and power update
        if (currencies[type].allowOverflow) currencies[type].amount += amount;
        else if (useStorages) UpdateStorages(type, amount, true);
        else currencies[type].amount += amount;

        // Display to UI
        UpdateUI(type);

        // Update unlockables
        Buildables.UpdateResourceUnlockables(type);

        // Update variant if heat passed
        if (type == CurrencyType.Heat) EnemyHandler.active.UpdateVariant();
    }

    // Remove a resource
    public void Remove(CurrencyType type, int amount, bool useStorages)
    {
        // Update storages
        if (currencies[type].allowOverflow) currencies[type].amount -= amount;
        else if (useStorages) UpdateStorages(type, amount, false);
        else currencies[type].amount -= amount;

        // Display to UI
        UpdateUI(type);

        // Update unlockables
        Buildables.UpdateResourceUnlockables(type);

        if (type == CurrencyType.Heat) EnemyHandler.active.UpdateVariant();
    }

    // Add storage
    public void AddStorage(CurrencyType type, int amount, DefaultStorage defaultStorage = null)
    {
        // Update active storages
        if (defaultStorage != null)
            storages.Add(defaultStorage);
        currencies[type].storage += amount;

        // Display to UI
        UpdateUI(type, true);
    }

    // Set storage
    public void SetStorage(CurrencyType type, int amount)
    {
        // Sets the storage
        currencies[type].storage = amount;

        // Display to UI
        UpdateUI(type, true);
    }

    // Remove storage
    public void RemoveStorage(CurrencyType type, int amount, DefaultStorage defaultStorage = null)
    {
        // Update active storages
        if (defaultStorage != null && storages.Contains(defaultStorage))
            storages.Remove(defaultStorage);

        // Revert the storage
        currencies[type].storage -= amount;
        if (currencies[type].storage <= 0)
            currencies[type].storage = 0;

        // Display to UI
        UpdateUI(type, true);
    }

    // Update UI
    public void UpdateUI(CurrencyType type, bool updateStorage = false)
    {
        // Display to UI
        if (currencies[type].resourceUI != null)
            currencies[type].resourceUI.text = FormatNumber(currencies[type].amount);

        if (updateStorage && currencies[type].storageUI != null)
        {
            // Change color based on storage value
            if (currencies[type].amount > currencies[type].storage)
                currencies[type].background.color = new Color(1, 0, 0, 0.3f);
            else currencies[type].background.color = new Color(1, 1, 1, 0.1f);

            currencies[type].storageUI.text = FormatNumber(currencies[type].storage) + " " + currencies[type].format;
        }

        if (currencies[type].useResourceBar && currencies[type].resourceBar != null)
        {
            currencies[type].resourceBar.currentPercent = (float)currencies[type].amount / (float)currencies[type].storage * 100;
            currencies[type].resourceBar.UpdateUI();
        }
    }

    // Check resources
    public bool CheckResources(Cost[] costs)
    {
        // Check if resource should be used
        foreach (Cost resource in costs)
        {
            // Use gamemode resource check
            if (!Gamemode.active.useResources && resource.resource != CurrencyType.Heat) continue;

            if (!resource.storage)
            {
                int amount = GetAmount(resource.resource);
                if (resource.add)
                {
                    Currency currency = GetCurrency(resource.resource);
                    if (currency.allowOverflow && amount > GetStorage(resource.resource)) return false;
                    else if (!currency.allowOverflow && amount + resource.amount > GetStorage(resource.resource)) return false;
                }
                else if (amount < resource.amount) return false;
            }
        }
        return true;
    }

    // Check freebie [NEEDS IMPROVEMENTS]
    public bool CheckFreebie(Buildable buildable)
    {
        // Check active instances
        if (buildable.isCollector) return CollectorHandler.active.collectors.Count < 1;
        else if (buildable.isStorage) return storages.Count < 1;
        else return false;
    }

    // Remove a resource based on building
    public void ApplyResources(Buildable buildable, bool isFree = false)
    {
        // Update resource values promptly
        foreach (Cost resource in buildable.resources)
        {
            // Check if gamemode is using resources
            if (!Gamemode.active.useResources)
            {
                // Validity check
                bool valid = false;

                // Check if resource adds storage
                if (resource.add && resource.storage) valid = true;
                else if (resource.resource == CurrencyType.Heat) valid = true;
                else if (resource.resource == CurrencyType.Power) valid = true;

                // Check if valid
                if (!valid) continue;
            }
            else if (isFree)
            {
                // Validity check
                bool valid = false;

                // Check if resources add storage
                if (resource.add) valid = true;
                else if (resource.resource == CurrencyType.Heat) valid = true;
                else if (resource.resource == CurrencyType.Power) valid = true;

                // Check if valid
                if (!valid) continue;
            }

            // Apply resources like usual
            if (resource.storage)
            {
                if (resource.add) active.AddStorage(resource.resource, resource.amount);
                else active.RemoveStorage(resource.resource, resource.amount);
            }
            else
            {
                if (resource.add) active.Add(resource.resource, resource.amount, true);
                else active.Remove(resource.resource, resource.amount, true);
            }
        }
    }

    // Remove a resource based on building
    public void RevertResources(Buildable buildable, bool refund = false)
    {
        // Update resource values promptly
        foreach (Cost resource in buildable.resources)
        {
            // Check if gamemode is using resources
            if (!Gamemode.active.useResources)
            {
                // Validity check
                bool valid = false;

                // Check if resource adds storage
                if (resource.add && resource.storage) valid = true;
                else if (resource.resource == CurrencyType.Heat) valid = true;
                else if (resource.resource == CurrencyType.Power) valid = true;

                // Check if valid
                if (!valid) continue;
            }

            if (resource.storage)
            {
                if (resource.add) active.RemoveStorage(resource.resource, resource.amount);
                else active.AddStorage(resource.resource, resource.amount);
            }
            else if (refund || currencies[resource.resource].allowOverflow)
            {
                if (resource.add) active.Remove(resource.resource, resource.amount, true);
                else active.Add(resource.resource, resource.amount, true);
            }
        }
    }

    // Check a resource amount
    public int GetAmount(CurrencyType type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type].amount;
        else return 0;
    }

    // Check a resource storage
    public int GetStorage(CurrencyType type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type].storage;
        else return 0;
    }

    // Get a resource name
    public string GetName(CurrencyType type)
    {
        switch(type)
        {
            case CurrencyType.Gold:
                return "Gold";
            case CurrencyType.Essence:
                return "Essence";
            case CurrencyType.Iridium:
                return "Iridium";
            case CurrencyType.Power:
                return "Power";
            case CurrencyType.Heat:
                return "Heat";
            default:
                return "Unknown";
        }
    }

    // Get a resource sprite
    public Sprite GetSprite(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Gold:
                return Sprites.GetSprite("Gold");
            case CurrencyType.Essence:
                return Sprites.GetSprite("Essence");
            case CurrencyType.Iridium:
                return Sprites.GetSprite("Iridium");
            case CurrencyType.Power:
                return Sprites.GetSprite("Power");
            case CurrencyType.Heat:
                return Sprites.GetSprite("Heat");
            default:
                return Sprites.GetSprite("Transparent");
        }
    }

    // Get heat or power
    public int GetHeat() { return currencies[CurrencyType.Heat].amount; }
    public int GetPower() { return currencies[CurrencyType.Power].amount; }

    // Number formatter
    public static string FormatNumber(int number)
    {
        if (number < 100000)
            return number.ToString();

        if (number < 1000000)
            return String.Format("{0:#,.}K", number - 500);

        if (number < 10000000)
            return String.Format("{0:#,,.##}M", number - 5000);

        if (number < 100000000)
            return String.Format("{0:#,,.#}M", number - 50000);

        if (number < 1000000000)
            return String.Format("{0:#,,.}M", number - 500000);

        return String.Format("{0:#,,,.##}B", number - 5000000);
    }
}
