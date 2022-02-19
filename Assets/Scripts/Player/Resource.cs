using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;
using Mirror;
using Sirenix.OdinInspector;

public class Resource : NetworkBehaviour
{
    // Active instance
    public static Resource active;

    // Currency types
    public enum Type
    {
        Gold = 0,
        Essence = 1,
        Iridium = 2,
        Power = 3,
        Heat = 4,
        Crystalline = 5
    }

    // Resource currency class
    [Serializable]
    public class Currency
    {
        // Currency variables
        [BoxGroup("Currency Info")]
        public Type type;
        [BoxGroup("Currency Info")]
        public string format;
        [BoxGroup("Currency Info"), HideInInspector]
        public int amount, storage;
        [BoxGroup("Currency Info"), HideInInspector]
        public bool useAmountText, useStorageText, useIncomeText, useStorageBar;
        [BoxGroup("Currency Flags")]
        public bool isOutput, canOverflow;

        // Currency interface elements
        [BoxGroup("Currency Info")]
        public TextMeshProUGUI amountText, storageText, incomeText;
        [BoxGroup("Currency Elements")]
        public ProgressBar storageBar;
    }

    // Dictionary of all currencies
    public Dictionary<Type, Currency> currencies = new Dictionary<Type, Currency>();
    [HideInInspector] public Currency[] currencyElements;

    // List of all collectors and storages
    public static List<DefaultStorage> storages;
    public bool networkResources;
    public int currentPerSec = 0;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // Generate resources
    public void Setup(Currency[] currencyElements) 
    {
        // Reset currencies
        currencies = new Dictionary<Type, Currency>();
        storages = new List<DefaultStorage>();
        Research.ResetResearch();

        // Setup new currencies
        this.currencyElements = currencyElements;

        // Setup currency elements
        foreach (Currency currency in currencyElements)
        {
            // Generate runtime currency info
            currency.useAmountText = currency.amountText != null;
            currency.useIncomeText = currency.incomeText != null;
            currency.useStorageText = currency.storageText != null;
            currency.useStorageBar = currency.storageBar != null;

            // Add the currency to the currencies dict
            currencies.Add(currency.type, currency);
        }

        // Set game object to true for networking
        gameObject.SetActive(true);

        // If network resource, create network sync invoke
        if (networkResources)
            InvokeRepeating("UpdateNetworkResources", 0.5f, 1f);
    }

    // Apply a resource
    public void Apply(Type type, int amount, bool useStorages)
    {
        // Heat and power update
        if (currencies[type].canOverflow) currencies[type].amount += amount;
        else if (useStorages) UpdateStorages(type, amount);
        else currencies[type].amount += amount;

        // Display to UI
        UpdateUI(type);

        // Update unlockables
        Buildables.UpdateResourceUnlockables(type);
    }

    // Applies resources method
    public void ApplyResources(Cost[] costs)
    {
        foreach (Cost cost in costs) 
        {
            if (!cost.storage && currencies.ContainsKey(cost.type)) 
            {
                if (currencies[cost.type].isOutput) Apply(cost.type, cost.amount, true);
                else Apply(cost.type, -cost.amount, true);
            }
        }
    }

    // Apply outputs only
    public void ApplyOutputsOnly(Cost[] costs)
    {
        foreach (Cost cost in costs)
            if (!cost.storage && currencies[cost.type].isOutput)
                Apply(cost.type, cost.amount, true);
    }

    // Refund resources method
    public void RefundResources(Cost[] costs)
    {
        foreach (Cost cost in costs)
        {
            if (!cost.storage && currencies.ContainsKey(cost.type))
            {
                if (currencies[cost.type].isOutput) Apply(cost.type, -cost.amount, true);
                else Apply(cost.type, cost.amount, true);
            }
        }
    }

    // Apply outputs only
    public void RefundOutputsOnly(Cost[] costs)
    {
        foreach (Cost cost in costs)
            if (!cost.storage && currencies[cost.type].isOutput)
                Apply(cost.type, -cost.amount, true);
    }

    // Update storages 
    public void UpdateStorages(Type type, int amount)
    {
        // Setup local loop variables
        int amountToAdd = amount;
        bool add = amount >= 0;

        // Updates all storages
        for (int i = 0; i < storages.Count; i++)
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
                            amountToAdd = storages[i].AddResources(amountToAdd, false);
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
                            storages[i].AddResources(amountToAdd, false);
                            return;
                        }
                    }
                }
            }
            else if (!NewSaveSystem.loadGame)
            {
                storages.RemoveAt(i);
                i--;
            }
        }
    }

    // Add storage
    public void ApplyStorage(Type type, int amount)
    {
        // Update active storages
        currencies[type].storage += amount;
        UpdateUI(type, true);
    }

    // Set amount
    public void SetAmount(Type type, int amount)
    {
        // Sets the storage
        currencies[type].amount = amount;
        UpdateUI(type, true);
    }

    // Set storage
    public void SetStorage(Type type, int amount)
    {
        // Sets the storage
        currencies[type].storage = amount;
        UpdateUI(type, true);
    }

    // Add storage object
    public void AddStorageObj(DefaultStorage storage)
    {
        if (!storages.Contains(storage))
            storages.Add(storage);
    }

    // Check resources
    public bool CheckResources(Cost[] costs)
    {
        // Check if adequate resources 
        foreach(Cost cost in costs)
        {
            // If storage, ignore
            if (cost.storage || !currencies.ContainsKey(cost.type)) continue;

            // Get amount of resource type
            int amount = GetAmount(cost.type);

            // Check if overflow is allowed
            if (currencies[cost.type].canOverflow)
            {
                if (amount >= GetStorage(cost.type) || amount - cost.amount < 0)
                    return false;
            }
            else if (currencies[cost.type].isOutput)
            {
                if (amount + cost.amount > GetStorage(cost.type))
                    return false;
            }
            else
            {
                if (amount - cost.amount < 0)
                    return false;
            }
        }

        // If all checks passed, return true
        return true;
    }

    // Check resources
    public bool CheckOutputsOnly(Cost[] costs)
    {
        // Check if adequate resources 
        foreach (Cost cost in costs)
        {
            // If storage, ignore
            if (cost.storage || !currencies.ContainsKey(cost.type) || !currencies[cost.type].isOutput) continue;

            // Get amount of resource type
            int amount = GetAmount(cost.type);

            // Check if overflow is allowed
            if (currencies[cost.type].canOverflow)
            {
                if (amount >= GetStorage(cost.type))
                    return false;
            }
            else
            {
                if (amount + cost.amount > GetStorage(cost.type))
                    return false;
            }
        }

        // If all checks passed, return true
        return true;
    }

    // Update UI
    public void UpdateUI(Type type, bool updateStorage = false)
    {
        // Update variant if heat passed
        if (type == Type.Heat) EnemyHandler.active.UpdateVariant(currencies[Type.Heat].amount, currencies[Type.Heat].storage);

        // Display to UI
        if (currencies[type].amountText != null)
            currencies[type].amountText.text = FormatNumber(currencies[type].amount);

        // Change color based on storage value
        if (updateStorage && currencies[type].storageText != null)
            currencies[type].storageText.text = FormatNumber(currencies[type].storage) + " " + currencies[type].format;

        // Check resource bar
        if (currencies[type].useStorageBar)
        {
            currencies[type].storageBar.currentPercent = (float)currencies[type].amount / (float)currencies[type].storage * 100;
            currencies[type].storageBar.UpdateUI();
        }
    }

    // Check freebie
    public bool CheckFreebie(Buildable buildable)
    {
        // Check active instances
        if (buildable.isCollector) return CollectorHandler.active.collectors.Count < 1;
        else if (buildable.isStorage) return storages.Count < 1;
        else if (buildable.isDefense && Tutorial.tutorialBuilding != null) return Tutorial.tutorialBuilding == buildable.building;
        else if (buildable.isDroneport && Tutorial.tutorialBuilding != null) return Tutorial.tutorialBuilding == buildable.building;
        else return false;
    }

    // Check a resource amount
    public int GetAmount(Type type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type].amount;
        else return 0;
    }

    // Check a resource storage
    public int GetStorage(Type type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type].storage;
        else return 0;
    }

    // Get currency class
    public Currency GetCurrency(Type type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type];
        else return null;
    }

    // Get a resource name
    public string GetName(Type type)
    {
        switch(type)
        {
            case Type.Gold:
                return "Gold";
            case Type.Essence:
                return "Essence";
            case Type.Iridium:
                return "Iridium";
            case Type.Power:
                return "Power";
            case Type.Heat:
                return "Heat";
            default:
                return "Unknown";
        }
    }

    // Get a resource sprite
    public Sprite GetSprite(Type type)
    {
        switch (type)
        {
            case Type.Gold:
                return Sprites.GetSprite("Gold");
            case Type.Essence:
                return Sprites.GetSprite("Essence");
            case Type.Iridium:
                return Sprites.GetSprite("Iridium");
            case Type.Power:
                return Sprites.GetSprite("Power");
            case Type.Heat:
                return Sprites.GetSprite("Heat");
            default:
                return Sprites.GetSprite("Transparent");
        }
    }

    // Get heat or power
    public int GetHeat() { return currencies[Type.Heat].amount; }
    public int GetHeatStorage() { return currencies[Type.Heat].storage; }
    public int GetPower() { return currencies[Type.Power].amount; }
    public int GetAvailablePower() { return currencies[Type.Power].storage; }

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

    // Update resources
    public void UpdateNetworkResources() { foreach (KeyValuePair<Type, Currency> currency in currencies) UpdateUI(currency.Key); }

    /* Update resources
    public void UpdatePerSecond()
    {
        // Get current perSec and increment index
        PerSecond resource = perSeconds[currentPerSec];
        currentPerSec += 1;
        if (currentPerSec == perSeconds.Count)
            currentPerSec = 0;

        // Make new average
        int average = 0;

        // Shift all elements down one (yes this isn't in a for-loop, get over it lol
        resource.amount[resource.index] = currencies[resource.type].amount - resource.lastCalculation;

        // Iterate through and tally total average
        for (int i = 0; i < resource.amount.Length; i++)
            average += resource.amount[i];

        // Increment index
        resource.index += 1;
        if (resource.index == resource.amount.Length)
            resource.index = 0;

        // Divide average
        average = average / resource.amount.Length;
        resource.lastCalculation = currencies[resource.type].amount;

        if (average == 0) resource.perSecondUI.text = "<color=white>" + average + " / second";
        else if (average > 0) resource.perSecondUI.text = "<color=green>+" + average + " / second";
        else resource.perSecondUI.text = "<color=red>" + average + " / second";
    }
    */
}
