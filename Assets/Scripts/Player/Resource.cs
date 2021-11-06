using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        public string format;
        public TextMeshProUGUI resourceUI;
        public TextMeshProUGUI storageUI;
    }

    // Dictionary of all currencies
    public Dictionary<CurrencyType, Currency> currencies;
    public Currency[] currencyElements;

    public void Awake() { Setup(); }

    // Get active instance
    public void Setup()
    {
        active = this;

        currencies = new Dictionary<CurrencyType, Currency>();
        foreach (Currency currency in currencyElements)
            currencies.Add(currency.type, currency);
    }
    
    // Get currency class
    public Currency GetCurrency(CurrencyType type)
    {
        if (currencies.ContainsKey(type))
            return currencies[type];
        else return null;
    }

    // Add a resource
    public void Add(CurrencyType type, int amount, bool overrideStorage = false)
    {
        if (currencies.ContainsKey(type))
        {
            // Calculate amount
            currencies[type].amount += amount;
            if (!overrideStorage && currencies[type].amount >= currencies[type].storage)
                currencies[type].amount = currencies[type].storage;

            // Display to UI
            if (currencies[type].resourceUI != null)
                currencies[type].resourceUI.text = FormatNumber(currencies[type].amount);
        }
        else Debug.Log("Could not add " + type);

        if (type == CurrencyType.Heat) EnemyHandler.active.UpdateVariant();
    }

    // Remove a resource
    public void Remove(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            // Calculate amount
            currencies[type].amount -= amount;
            if (currencies[type].amount <= 0)
                currencies[type].amount = 0;

            // Display to UI
            if (currencies[type].resourceUI != null)
                currencies[type].resourceUI.text = FormatNumber(currencies[type].amount);
        }

        if (type == CurrencyType.Heat) EnemyHandler.active.UpdateVariant();
    }

    // Add storage
    public void AddStorage(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type].storage += amount;
            currencies[type].storageUI.text = FormatNumber(currencies[type].storage) + currencies[type].format;
        }
    }

    // Remove storage
    public void RemoveStorage(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type].storage -= amount;
            if (currencies[type].storage <= 0)
                currencies[type].storage = 0;
            currencies[type].storageUI.text = FormatNumber(currencies[type].storage) + currencies[type].format;
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
    static string FormatNumber(int number)
    {
        if (number < 1000)
            return number.ToString();

        if (number < 10000)
            return String.Format("{0:#,.##}K", number - 5);

        if (number < 100000)
            return String.Format("{0:#,.#}K", number - 50);

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
