using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // Currency types
    public enum Currency
    {
        Gold,
        Essence,
        Iridium,
        Power,
        Heat
    }

    // Currency and storage variables
    public static Dictionary<Currency, int> currency = new Dictionary<Currency, int>();
    public static Dictionary<Currency, int> storage = new Dictionary<Currency, int>();

    // Add a resource
    public static void Add(Currency type, int amount)
    {
        if (currency.ContainsKey(type) && (!storage.ContainsKey(type) || currency[type] + amount < storage[type]))
        {
            currency[type] += amount;
        }
        else
        {
            currency.Add(type, amount);
        }
    }

    // Remove a resource
    public static void Remove(Currency type, int amount)
    {
        if (currency.ContainsKey(type))
        {
            currency[type] -= amount;
            if (currency[type] < 0)
                currency[type] = 0;
        }
    }

    // Check a resource
    public static int Get(Currency type)
    {
        if (currency.ContainsKey(type))
            return currency[type];
        else return 0;
    }

    // Add storage
    public static void AddStorage(Currency type, int amount)
    {
        if (storage.ContainsKey(type))
        {
            storage[type] += amount;
        }
        else
        {
            storage.Add(type, amount);
        }
    }

    // Remove storage
    public static void RemoveStorage(Currency type, int amount)
    {
        if (storage.ContainsKey(type))
        {
            storage[type] -= amount;
            if (storage[type] < 0)
                storage[type] = 0;
        }
    }

    // Check a storage
    public static int GetStorage(Currency type)
    {
        if (storage.ContainsKey(type))
            return storage[type];
        else return 0;
    }
}
