using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantPalette : MonoBehaviour
{
    // Start is called before the first frame update
    public List<VariantColor> _variantColors;
    public static Dictionary<Variant, VariantColor> variantColors;

    public void Awake()
    {
        variantColors = new Dictionary<Variant, VariantColor>();
        foreach (VariantColor color in _variantColors)
            variantColors.Add(color.type, color);
    }

    public static VariantColor GetVariantColor(Variant variant)
    {
        if (variantColors.ContainsKey(variant))
            return variantColors[variant];
        else return null;
    }
}
