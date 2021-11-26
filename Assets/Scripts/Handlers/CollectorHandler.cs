using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorHandler : MonoBehaviour
{
    // Active instance
    public static CollectorHandler active;

    // Class lists
    public List<DefaultCollector> collectors;

    public void Awake() { active = this; }

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
                if (collectors[i].cooldown < 0f) collectors[i].AddResources();
            }
            else
            {
                Resource.active.currencies[collectors[i].type].perSecond -=
                    (int)Research.resource[collectors[i].type].extractionRate /
                         Research.resource[collectors[i].type].extractionYield;

                collectors.RemoveAt(i);
                i--;
            }
        }
    }

    public void AddCollector(DefaultCollector collector)
    {
        Resource.active.currencies[collector.type].perSecond +=
            (int)Research.resource[collector.type].extractionRate / 
                 Research.resource[collector.type].extractionYield;

        collectors.Add(collector);
    }

    public void RecalculateAllCollectors(Resource.CurrencyType currency)
    {
        foreach(DefaultCollector collector in collectors)
            if (collector.type == currency)
                Resource.active.currencies[collector.type].perSecond +=
                    (int)Research.resource[collector.type].extractionRate /
                         Research.resource[collector.type].extractionYield;
    }
}
