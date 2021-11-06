using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorHandler : MonoBehaviour
{
    public List<Collector> collectors;

    public void Start()
    {
        Events.active.onCollectorPlaced += AddCollector;
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
                    collectors[i].collected += Research.research_gold_yield;
                    collectors[i].cooldown = Research.research_gold_time;
                }
            }
            else
            {
                collectors.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddCollector(Collector collector)
    {
        collectors.Add(collector);
    }
}
