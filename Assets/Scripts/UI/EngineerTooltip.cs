using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EngineerTooltip : MonoBehaviour
{
    // Blueprint variables
    public new TextMeshProUGUI name;
    public TextMeshProUGUI rarity;
    public Image backgroundColor;
    public TextMeshProUGUI description;
    public TextMeshProUGUI positiveEffect;
    public TextMeshProUGUI negativeEffect;

    // Start is called before the first frame update
    public void SetTooltip(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        name.text = blueprint.name.ToUpper();
        this.rarity.text = rarity.rarity.ToString().ToUpper();
        backgroundColor.color = rarity.color;
        description.text = blueprint.description;

        for(int i = 0; i < blueprint.effects.Count; i++)
        {
            if (blueprint.effects[i].negative)
                negativeEffect.text += Enum.GetName(typeof(Blueprint.Effect), blueprint.effects[i].effect).ToUpper() + " (" + rarity.modifier[i]*100 + "%)\n";
            else positiveEffect.text += Enum.GetName(typeof(Blueprint.Effect), blueprint.effects[i].effect).ToUpper() + " (" + rarity.modifier[i]*100 + "%)\n";
        }

        if (negativeEffect.text == "") negativeEffect.text = "None";
        else if (positiveEffect.text == "") positiveEffect.text = "None";
    }
}
