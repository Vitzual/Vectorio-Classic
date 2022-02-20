using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class DamageBar : MonoBehaviour
{
    // Object and thing
    [HideInInspector]
    public BaseEntity entity;
    [SerializeField]
    public ProgressBar bar;
    public RectTransform rect;
    public Image background;
    public Image foreground;

    // Create damaged bar
    public void UpdateDamage()
    {
        bar.currentPercent = entity.health / entity.maxHealth * 100;
    }

    // Set bar color
    public void SetBarColor(VariantStats variant)
    {
        VariantColor color = VariantPalette.GetVariantColor(variant.type);
        background.color = color.fillColor;
        foreground.material = color.borderMaterial;
    }
}
