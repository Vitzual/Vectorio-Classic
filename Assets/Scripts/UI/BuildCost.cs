using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildCost : MonoBehaviour
{
    // Variables
    public Resource.CurrencyType resource;
    public TextMeshProUGUI element;
    public new string name;
    private int amount;

    // Update amount (Add)
    public void Add(int amount)
    {
        this.amount += amount;
        element.text = "<b>" + name.ToUpper() + ":</b> " + this.amount;
    }

    // Update amount (Remove)
    public void Remove(int amount)
    {
        this.amount -= amount;
        if (this.amount < 0) this.amount = 0;
        element.text = "<b>" + name.ToUpper() + ":</b> " + this.amount;
    }
}
