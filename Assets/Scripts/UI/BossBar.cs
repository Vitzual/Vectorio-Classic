using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;

public class BossBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Image bossIcon;
    public TextMeshProUGUI bossName;
    public ProgressBar bossBar;
    public Image bossBarBackground;
    public Image bossBarOverlap;
    public CanvasGroup canvasGroup;

    // Active instance
    [HideInInspector] public DefaultGuardian guardian;
    [HideInInspector] public bool activeGuardian = false;

    // On boss spawned
    public void Start()
    {
        Events.active.onGuardianSpawned += SetBar;
    }

    // Set boss bar
    public void SetBar(DefaultGuardian guardian)
    {
        // Set variables
        this.guardian = guardian;
        activeGuardian = true;
        canvasGroup.alpha = 1f;
        bossIcon.sprite = Sprites.GetSprite(guardian.name);
        bossName.text = guardian.name.ToUpper();

        // Set colors
        bossBarBackground.color = guardian.guardian.barBackgroundColor;
        bossBarOverlap.color = guardian.guardian.barForegroundColor;

        // Reset boss bar
        bossBar.currentPercent = 100f;
        bossBar.UpdateUI();
    }

    // Update bar
    public void Update()
    {
        if (activeGuardian)
        {
            if (guardian != null)
            {
                bossBar.currentPercent = guardian.health / guardian.maxHealth * 100;
                bossBar.UpdateUI();
            }
            else
            {
                activeGuardian = false;
                canvasGroup.alpha = 0f;
            }
        }
    }
}
