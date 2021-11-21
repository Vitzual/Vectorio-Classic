using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;

public class StatsPanel : MonoBehaviour
{
    // Additional settings
    [System.Serializable]
    public class AdditionalSetting
    {
        public GameObject settingObj;
        public Entity entity;
        public HorizontalSelector[] selectors;
    }

    // Stat variables
    public Image buildingIcon;
    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDesc;
    public ProgressBar buildingHealth;
    public GameObject noSettings;
    public AdditionalSetting[] additionalSettings;

    // Runtime created
    public Dictionary<Entity, AdditionalSetting> settings;

    // Setup runtime 
    public void Start()
    {
        Events.active.onEntityClicked += SetPanel;
    }

    // Set panel
    public void SetPanel(BaseEntity entity)
    {

    }
}
