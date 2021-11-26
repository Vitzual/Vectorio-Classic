using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWHandler : MonoBehaviour
{
    // Hold super enumerator
    public enum Super
    {
        Trident
    }

    // Hold super
    public GameObject UI;
    public SuperWeapon superWeapon;
    public ProgressBar progressBar;

    // Set super weapon
    public void SetWeapon(SuperWeapon superWeapon)
    {
        this.superWeapon = superWeapon;
        progressBar.maxValue = superWeapon.turret.cooldown;
    }
}
