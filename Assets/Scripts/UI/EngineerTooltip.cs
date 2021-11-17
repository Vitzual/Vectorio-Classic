using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EngineerTooltip : MonoBehaviour
{
    // Blueprint variables
    public new TextMeshProUGUI name;
    public TextMeshProUGUI rarity;
    public SpriteRenderer background;
    public TextMeshProUGUI description;
    public TextMeshProUGUI effect;
    public Transform postiveList;
    public Transform negativeList;

    // Start is called before the first frame update
    public void SetTooltip(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        name.text = blueprint.name.ToUpper();
        this.rarity.text = rarity.rarity.ToString().ToUpper();
    }
}
