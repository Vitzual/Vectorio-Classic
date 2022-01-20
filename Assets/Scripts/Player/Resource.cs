using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;
using Mirror;

public class Resource : NetworkBehaviour
{
    // Active instance
    public static Resource active;

    // Currency types
    public enum CurrencyType
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
        public CurrencyType type;
        public int amount;
        public int storage;
        public string format;
        public bool allowOverflow;
        public bool output;

        // UI elements
        public bool enabled;
        public GameObject element;
        public Image background;
        public TextMeshProUGUI resourceUI;
        public TextMeshProUGUI storageUI;

        // Resource bar
        public bool useResourceBar;
        public ProgressBar resourceBar;

        // Default variables
        public float collectionRate;
        public int collectionAmount;
        public int storageAmount;
    }

    [Serializable]
    public class PerSecond
    {
        public CurrencyType type;
        public int lastCalculation = 0;
        public int[] amount = new int[5];
        public int index = 0;
        public TextMeshProUGUI perSecondUI;
    }

    public List<PerSecond> perSeconds;
    public int currentPerSec = 0;

    // Dictionary of all currencies
    public Dictionary<CurrencyType, Currency> currencies = new Dictionary<CurrencyType, Currency>();
    public Currency[] currencyElements;

    // List of all collectors and storages
    public static List<DefaultStorage> storages;
    public bool networkResources;

    // Get active instance
    public void Awake()
    {
        active = this;
    }

    // Generate resources
    public void Setup(List<PerSecond> perSecondElements, Currency[] currencyElements) 
    {
        // Reset currencies
        currencies = new Dictionary<CurrencyType, Currency>();
        storages = new List<DefaultStorage>();
        Research.ResetResearch();

        // Setup new currencies
        this.currencyElements = currencyElements;

        // Setup currency elements
        foreach (Currency currency in currencyElements)
        {
            currencies.Add(currency.type, currency);

            if (currency.background != null)
                currency.background.color = new Color(1, 1, 1, 0.1f);

            if (ResearchUI.active != null)
                Research.GenerateBoost(currency.type, currency.collectionRate, currency.collectionAmount, currency.storageAmount);
        }

        // Setup per seconds
        perSeconds = perSecondElements;

        if (perSeconds.Count > 0)
        {
            // Setup PerSecond array
            foreach (PerSecond resource in perSeconds)
            {
                for (int i = 0; i < resource.amount.Length; i++)
                    resource.amount[i] = 0;
            }

            // Setup events
            if (perSeconds.Count > 0)
                InvokeRepeating("UpdatePerSecond", 0, 1f / (float)perSeconds.Count);
        }

        gameObject.SetActive(true);

        if (networkResources)
            InvokeRepeating("UpdateResources", 0.5f, 1f);
    }

    // Update resources
    public void UpdateResources() { foreach (KeyValuePair<CurrencyType, Currency> currency in currencies) UpdateUI(currency.Key); }

    // Apply a resource
    public void Apply(CurrencyType type, int amount, bool useStorages)
    {
        // Heat and power update
        if (currencies[type].allowOverflow) currencies[type].amount += amount;
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
                if (currencies[cost.type].output) Apply(cost.type, cost.amount, true);
                else Apply(cost.type, -cost.amount, true);
            }
        }
    }

    // Apply outputs only
    public void ApplyOutputsOnly(Cost[] costs)
    {
        foreach (Cost cost in costs)
            if (!cost.storage && currencies[cost.type].output)
                Apply(cost.type, cost.amount, true);
    }

    // Refund resources method
    public void RefundResources(Cost[] costs)
    {
        foreach (Cost cost in costs)
        {
            if (!cost.storage && currencies.ContainsKey(cost.type))
            {
                if (currencies[cost.type].output) Apply(cost.type, -cost.amount, true);
                else Apply(cost.type, cost.amount, true);
            }
        }
    }

    // Apply outputs only
    public void RefundOutputsOnly(Cost[] costs)
    {
        foreach (Cost cost in costs)
            if (!cost.storage && currencies[cost.type].output)
                Apply(cost.type, -cost.amount, true);
    }

    // Update storages 
    public void UpdateStorages(CurrencyType type, int amount)
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
    public void ApplyStorage(CurrencyType type, int amount)
    {
        // Update active storages
        currencies[type].storage += amount;
        UpdateUI(type, true);
    }

    // Set amount
    public void SetAmount(CurrencyType type, int amount)
    {
        // Sets the storage
        currencies[type].amount = amount;
        UpdateUI(type, true);
    }

    // Set storage
    public void SetStorage(CurrencyType type, int amount)
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
            if (currencies[cost.type].allowOverflow)
            {
                if (amount >= GetStorage(cost.type) || amount - cost.amount < 0)
                    return false;
            }
            else if (currencies[cost.type].output)
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
            if (cost.storage || !currencies.ContainsKey(cost.type) || !currencies[cost.type].output) continue;

            // Get amount of resource type
            int amount = GetAmount(cost.type);

            // Check if overflow is allowed
            if (currencies[cost.type].allowOverflow)
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
    public void UpdateUI(CurrencyType type, bool updateStorage = false)
    {
        // Update variant if heat passed
        if (type == CurrencyType.Heat) EnemyHandler.active.UpdateVariant(currencies[CurrencyType.Heat].amount, currencies[CurrencyType.Heat].storage);

        // Check if UI is enabled
        if (!currencies[type].enabled)
        {
            currencies[type].enabled = true;
            currencies[type].element.SetActive(true);
        }

        // Display to UI
        if (currencies[type].resourceUI != null)
            currencies[type].resourceUI.text = FormatNumber(currencies[type].amount);

        // Change color based on storage value
        if (updateStorage && currencies[type].storageUI != null)
        {
            if (currencies[type].background != null)
            {
                if (currencies[type].amount > currencies[type].storage)
                    currencies[type].background.color = new Color(1, 0, 0, 0.3f);
                else currencies[type].background.color = new Color(1, 1, 1, 0.1f);
            }

            currencies[type].storageUI.text = FormatNumber(currencies[type].storage) + " " + currencies[type].format;
        }

        // Check resource bar
        if (currencies[type].useResourceBar && currencies[type].resourceBar != null)
        {
            currencies[type].resourceBar.currentPercent = (float)currencies[type].amount / (float)currencies[type].storage * 100;
            currencies[type].resourceBar.UpdateUI();
        }
    }

    // Update resources
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

    // Get currency class
    public Currency GetCurrency(CurrencyType type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type];
        else return null;
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
    public int GetHeatStorage() { return currencies[CurrencyType.Heat].storage; }
    public int GetPower() { return currencies[CurrencyType.Power].amount; }
    public int GetAvailablePower() { return currencies[CurrencyType.Power].storage; }

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
