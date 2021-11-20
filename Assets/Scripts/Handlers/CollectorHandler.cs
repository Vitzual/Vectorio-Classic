using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorHandler : MonoBehaviour
{
    // Active instance
    public static CollectorHandler active;

    // Class lists
    public List<DefaultCollector> collectors;
    public List<Storage> storages;

    public void Awake() { active = this; }

    public void Start()
    {
        Events.active.onCollectorPlaced += AddCollector;
        Events.active.onStoragePlaced += AddStorage;
    }

    public void Update()
    {
        for(int i = 0; i < collectors.Count; i++)
        {
            if (collectors[i] != null)
            {
                collectors[i].cooldown -= Time.deltaTime;
                if (collectors[i].cooldown < 0f)
                {
                    collectors[i].amount += Research.gold_yield;
                    collectors[i].cooldown = Research.gold_time;
                }
            }
            else
            {
                collectors.RemoveAt(i);
                i--;
            }
        }
    }

    public void TransferResources(int amount, Resource.CurrencyType type)
    {
        for(int i =0; i < storages.Count; i++)
        {
            if (storages[i] != null)
            {
                if (storages[i].type == type)
                    amount = storages[i].AddResources(amount);
                if (amount <= 0) return;
            }
            else
            {
                storages.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddCollector(DefaultCollector collector)
    {
        collectors.Add(collector);
    }

    public void AddStorage(Storage storage)
    {
        storages.Add(storage);
    }
}
