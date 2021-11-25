using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tech", menuName = "Building/Research")]
public class ResearchBoost : IdentifiableScriptableObject
{
    // Start is called before the first frame update
    public new string name;
    [TextArea] public string description;
    public int heatRequirement;
    public List<Cost> cost;
    public ResearchType type;
    public float amount;
    public Resource.CurrencyType currency;
}
