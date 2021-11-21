using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class DamageBar : MonoBehaviour
{
    // Object and thing
    [HideInInspector]
    public BaseEntity entity;
    [SerializeField]
    public ProgressBar bar;
    public RectTransform rect;

    // Create damaged bar
    public void UpdateDamage()
    {
        bar.currentPercent = entity.health / entity.maxHealth * 100;
    }
}
