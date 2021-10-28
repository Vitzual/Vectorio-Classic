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
        // Parameterized Constructor
        public Currency(TextMeshProUGUI ui) { this.ui = ui; }

        // Currency variables
        public int amount;
        public int storage;
        public TextMeshProUGUI ui;
    }

    // Dictionary of all currencies
    public Dictionary<CurrencyType, Currency> currencies;
    public List<TextMeshProUGUI> amountElements; // TEMP

    // Get active instance
    public void Awake()
    {
        active = this;

        // I hate this and will change it :(
        int indexer = 0;
        foreach (CurrencyType currency in CurrencyType.GetValues(typeof(CurrencyType)))
        {
            currencies.Add(CurrencyType.Gold, new Currency(amountElements[indexer]));
            indexer += 1;
            if (indexer > amountElements.Count) break;
        }
    }

    // Add a resource
    public void Add(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type)) 
        {
            currencies[type].amount += amount;
            if (currencies[type].amount >= currencies[type].storage)
                currencies[type].amount = currencies[type].storage;
        }
    }

    // Remove a resource
    public void Remove(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type].amount -= amount;
            if (currencies[type].amount <= 0)
                currencies[type].amount = 0;
        }
    }

    // Add storage
    public void AddStorage(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
            currencies[type].storage += amount;
    }

    // Remove storage
    public void RemoveStorage(CurrencyType type, int amount)
    {
        if (currencies.ContainsKey(type))
        {
            currencies[type].storage -= amount;
            if (currencies[type].storage <= 0)
                currencies[type].storage = 0;
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
}
