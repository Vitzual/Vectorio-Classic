using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tech", menuName = "Vectorio/Building/Research")]
public class ResearchTech : IdentifiableScriptableObject
{
    // Start is called before the first frame update
    public Sprite icon;
    public new string name;
    [TextArea] public string description;
    public ResearchUI.IndexLists indexList;
    public int indexNumber;
    public int metadataID;
    public int heatUnlockCost;
    public List<Cost> cost = new List<Cost>();
    public ResearchTypeEnum type;
    public float amount;
    public Resource.CurrencyType currency;

    public int GetCost(Resource.CurrencyType cost)
    {
        foreach (Cost theCost in this.cost)
            if (theCost.type == cost)
                return theCost.amount;
        return 0;
    }
}
