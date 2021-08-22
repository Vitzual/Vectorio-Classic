using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;

public class Technology : MonoBehaviour
{
    // Survival script
    public Survival main;

    // Interface script
    public Interface UI;
    public ButtonManagerBasicIcon rButton;
    public bool loadingSave = false;
    public GameObject InventoryItem;
    public bool TreeGenerated = false;

    // Unlock variables 
    // 
    // x1 = UnlockValue1
    // x2 = UnlockValue2
    //
    // Heat = Reach x1 heat value
    // Power = Reach x1 power value
    // Place = Place x1 amount of building with ID x2
    // Research = Research x1 amount of techs 
    // Destroy = Destroy x1 amount of enemies with ID x2

    public int UnlockAmount = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockable
    {
        public string Name;
        public Transform Building;
        public string InvType;
        public string UnlockType;
        public string UnlockDesc;
        public Sprite UnlockIcon;
        public int UnlockID;
        public int AmountRequirement; // Amount needed
        public int AmountOptionalID; // ID of entity 
        public int AmountTracked; // Holder value

        public Transform ParentObj;
        public Transform ChildObj;
        public Image InvIcon;
        public TextMeshProUGUI InvName;
        public Button InvButton;
        public Transform Progress;
        public ProgressBar ProgressBar;
        public TextMeshProUGUI ProgressDesc;
        public TextMeshProUGUI ProgressText;
        public Image ProgressIcon;

        public bool Unlocked = false;
    }
    public Unlockable[] Unlocks;

    public List<int> HeatUnlocks;
    public List<int> PowerUnlocks;
    public List<int> PlaceUnlocks;
    public List<int> ResearchUnlocks;
    public List<int> DestroyUnlocks;
    public List<int> BossUnlocks;

    // Create unlocked list
    public List<Transform> unlocked = new List<Transform>();

    private void Start()
    {
        // Get scripts
        main = gameObject.GetComponent<Survival>();
        UI = gameObject.GetComponent<Interface>();

        GenerateTree();
    }

    public void GenerateTree()
    {
        // Check if the tree has already been generated
        if (TreeGenerated) return;
        else TreeGenerated = true;

        // Go through and assign list types to lower calculation time on updates
        for (int i = 0; i < Unlocks.Length; i++)
        {
            // Grab all button variables
            try
            {
                GameObject newItem = Instantiate(InventoryItem, new Vector3(0, 0, 0), Quaternion.identity);
                newItem.transform.SetParent(Unlocks[i].ParentObj);
                newItem.transform.localScale = new Vector3(1, 1, 1);
                newItem.transform.name = Unlocks[i].Building.name;
                Unlocks[i].ChildObj = newItem.transform;
                Unlocks[i].InvIcon = Unlocks[i].ChildObj.Find("Image").GetComponent<Image>();
                Unlocks[i].InvName = Unlocks[i].ChildObj.Find("Name").GetComponent<TextMeshProUGUI>();
                Unlocks[i].InvButton = Unlocks[i].ChildObj.Find("Button").GetComponent<Button>();
                Unlocks[i].Progress = Unlocks[i].ChildObj.Find("Progress");
                Unlocks[i].ProgressBar = Unlocks[i].Progress.GetComponent<ProgressBar>();
                Unlocks[i].ProgressDesc = Unlocks[i].Progress.Find("Goal").GetComponent<TextMeshProUGUI>();
                Unlocks[i].ProgressText = Unlocks[i].Progress.Find("Progress").GetComponent<TextMeshProUGUI>();
                Unlocks[i].ProgressIcon = Unlocks[i].Progress.Find("Icon").GetComponent<Image>();
            }
            catch (Exception e)
            {
                Debug.Log("Error while adding button " + Unlocks[i].Building.name + "...\nStacktrace: " + e);
                return;
            }

            // Set all variables grabbed 
            Transform building = Unlocks[i].Building;
            Unlocks[i].InvButton.onClick.AddListener(delegate { main.SetChosenObj(building); });
            Unlocks[i].InvName.text = Unlocks[i].Building.name.ToUpper();
            Unlocks[i].ProgressDesc.text = Unlocks[i].UnlockDesc;
            Unlocks[i].ProgressText.text = "0/" + Unlocks[i].AmountRequirement;
            Unlocks[i].ProgressIcon.sprite = Unlocks[i].UnlockIcon;

            // Add unlock to proper unlock list
            if (Unlocks[i].UnlockType == "Heat") HeatUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Power") PowerUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Place") PlaceUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Research") ResearchUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Destroy") DestroyUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Boss") BossUnlocks.Add(i);
        }
    }

    public void UpdateUnlock(string unlockType, int arg = -1)
    {
        switch(unlockType)
        {
            case "Heat":
                foreach (int index in HeatUnlocks)
                {
                    if (!Unlocks[index].Unlocked && main.Spawner.htrack >= Unlocks[index].AmountRequirement) UnlockDefense(Unlocks[index]);
                    UpdateUnlockUI(Unlocks[index], main.Spawner.htrack);
                }
                return;
            case "Power":
                foreach (int index in PowerUnlocks)
                {
                    if (!Unlocks[index].Unlocked && main.PowerConsumption >= Unlocks[index].AmountRequirement) UnlockDefense(Unlocks[index]);
                    UpdateUnlockUI(Unlocks[index], main.PowerConsumption);
                }
                return;
            case "Place":
                /*
                foreach (int index in PlaceUnlocks)
                {
                    if (Unlocks[index].Unlocked) continue;
                    Transform building = FindTechBuilding(Unlocks[index].AmountOptionalID);
                    if (building != null) {
                        if (BuildingSystem.buildingAmount.ContainsKey(building.name))
                        {
                            if (BuildingSystem.buildingAmount[building.name] >= Unlocks[index].AmountRequirement) UnlockDefense(Unlocks[index]);
                            UpdateUnlockUI(Unlocks[index], BuildingSystem.buildingAmount[building.name]);
                        }
                    }
                }
                */
                return;
            case "Research":
                foreach (int index in ResearchUnlocks)
                {
                    if (!Unlocks[index].Unlocked && Research.amountResearched >= Unlocks[index].AmountRequirement ) UnlockDefense(Unlocks[index]);
                    UpdateUnlockUI(Unlocks[index], Research.amountResearched);
                }
                return;
            case "Destroy":
                foreach (int index in DestroyUnlocks) 
                {
                    if (!Unlocks[index].Unlocked && Unlocks[index].AmountOptionalID == arg)
                    {
                        Unlocks[index].AmountTracked += 1;
                        if (Unlocks[index].AmountTracked >= Unlocks[index].AmountRequirement)
                            UnlockDefense(Unlocks[index]);
                        UpdateUnlockUI(Unlocks[index], Unlocks[index].AmountTracked);
                    }
                }
                return;
            case "Boss":
                foreach(int index in BossUnlocks)
                {
                    if (!Unlocks[index].Unlocked && Unlocks[index].AmountOptionalID == arg)
                    {
                        Unlocks[index].AmountTracked = 1;
                        UnlockDefense(Unlocks[index]);
                        UpdateUnlockUI(Unlocks[index], Unlocks[index].AmountTracked);
                    }
                }
                return;
            default:
                Debug.LogError("Unlock type " + unlockType + " is not valid!");
                return;
        }
    }

    public void UpdateUnlockUI(Unlockable unlock, int amount, bool loading = false)
    {
        if (loading || unlock.AmountRequirement == amount)
        {
            unlock.Progress.gameObject.SetActive(false);
            unlock.InvName.fontSize = 30;
            unlock.InvName.text = unlock.Building.name.ToUpper() + " <size=18>LEVEL 1";
            unlock.InvIcon.sprite = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
        }
        else
        {
            unlock.ProgressBar.currentPercent = (float)amount / (float)unlock.AmountRequirement * 100;
            unlock.ProgressText.text = amount + "/" + unlock.AmountRequirement;
        }
    }

    public Unlockable GetUnlockableWithBuilding(Transform building)
    {
        foreach (Unlockable unlock in Unlocks)
            if (unlock.Building == building)
                return unlock;
        return null;
    }

    public Unlockable GetUnlockableWithID(int ID)
    {
        foreach (Unlockable unlock in Unlocks)
            if (unlock.UnlockID == ID)
                return unlock;
        return null;
    }

    public int[] GetSaveData()
    {
        int[] unlockList = new int[Unlocks.Length];
        for (int i = 0; i < unlockList.Length; i++)
            if (Unlocks[i].Unlocked) unlockList[i] = Unlocks[i].UnlockID;
        return unlockList;
    }

    public void LoadSaveData(int[] unlockList)
    {
        GenerateTree();
        loadingSave = true;
        for (int i = 0; i < unlockList.Length; i++)
        { 
            Unlockable unlock = GetUnlockableWithID(unlockList[i]);
            if (unlock == null) continue;
            UnlockDefense(unlock);
            UpdateUnlockUI(unlock, -1, true);
        }
        loadingSave = false;
    }

    public Transform FindTechBuilding(int a)
    {
        // OUTDATED - MARKED FOR REFACTOR
        return null;
    }

    public Transform FindTechBuildingWithName(string a)
    {
        // OUTDATED - MARKED FOR REFACTOR
        return null;
    }

    // Unlocks a defense and displays to screen
    public void UnlockDefense(Unlockable unlock)
    {
        // Increment update count
        if (unlock == null) return;

        // Set button icon and text
        unlock.InvIcon.sprite = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
        unlock.InvName.text = unlock.Building.name.ToUpper();

        // Set the UOL
        if (!loadingSave)
        {
            UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
            UI.UOL.titleText = unlock.Building.name.ToUpper();
            UI.UOL.descriptionText = unlock.Building.GetComponent<BaseBuilding>().GetDescription();
            UI.UOL.UpdateUI();
        }

        // Add the building to the unlock list
        AddUnlocked(unlock.Building);

        // Set timescale
        if (!loadingSave) Time.timeScale = 0f;
    }

    // Checks if a building is unlocked
    public bool CheckIfUnlocked(Transform a)
    {
        for (int i = 0; i < unlocked.Count; i++)
        {
            if (a.name == unlocked[i].name)
            {
                return true;
            }
        }
        UI.DisableActiveInfo();
        return false;
    }

    // Add a new object to unlock list
    public void AddUnlocked(Transform a)
    {
        // Check if building is RL or E
        if (a.name == "Research Lab")
        {
            if (!loadingSave) UI.showResearchUnlock();
            rButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
            Research.ResearchUnlocked = true;
        }
        else if (!loadingSave && a.name == "Energizer")
        {
            UI.showEnergizerUnlock();
            UI.UOLOpen = true;
        }
        else if (!loadingSave)
        {
            UI.UOL.OpenWindow();
            UI.UOLOpen = true;
        }

        // Add the unlock to the list
        unlocked.Add(a);

        // Find the object in the array list
        for (int i = 0; i < Unlocks.Length; i++)
            if (Unlocks[i].Building == a)
                Unlocks[i].Unlocked = true;
    }

    public void CloseUnlockTree()
    {
        UnlocksLeft = false;
        UI.Overlay.transform.Find("Upgrade").gameObject.SetActive(false);
        UnlockAmount = Unlocks.Length;
        return;
    }
}
