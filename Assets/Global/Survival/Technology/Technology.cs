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

    // Unlock variables 
    // 
    // x1 = UnlockValue1
    // x2 = UnlockValue2
    //
    // Heat = Reach x1 heat value
    // Power = Reach x1 power value
    // Place = Place x1 amount of building with ID x2
    // Research = Research x1 amount of techs 

    public int UnlockAmount = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockable
    {
        public string Name;
        public Transform Building;
        public string UnlockType;
        public int UnlockValue1;
        public int UnlockValue2;
        public int UnlockID;
        public bool Unlocked = false;
        public string InvType;
        public string InvUnlockDesc;
        public Image InvIcon;
        public TextMeshProUGUI InvName;
        public ProgressBar InvBar;
    }
    public Unlockable[] Unlocks;

    public List<int> HeatUnlocks;
    public List<int> PowerUnlocks;
    public List<int> PlaceUnlocks;
    public List<int> ResearchUnlocks;

    // Create unlocked list
    public List<Transform> unlocked = new List<Transform>();

    private void Start()
    {
        // Get scripts
        main = gameObject.GetComponent<Survival>();
        UI = gameObject.GetComponent<Interface>();

        // Go through and assign list types to lower calculation time on updates
        for(int i = 0; i < Unlocks.Length; i++)
        {
            if (Unlocks[i].UnlockType == "Heat") HeatUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Power") PowerUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Place") PlaceUnlocks.Add(i);
            else if (Unlocks[i].UnlockType == "Research") ResearchUnlocks.Add(i);
        }
    }

    public void UpdateUnlock(string unlockType)
    {
        switch(unlockType)
        {
            case "Heat":
                foreach (int index in HeatUnlocks)
                    if (main.Spawner.htrack >= Unlocks[index].UnlockValue1)
                        UnlockDefense(Unlocks[index]);
                return;
            case "Power":
                foreach (int index in PowerUnlocks)
                    if (main.PowerConsumption >= Unlocks[index].UnlockValue1)
                        UnlockDefense(Unlocks[index]);
                return;
            case "Place":
                foreach (int index in PlaceUnlocks)
                {
                    Transform building = FindTechBuilding(Unlocks[index].UnlockValue1);
                    if (building != null) {
                        if (BuildingHandler.buildingAmount.ContainsKey(building.name)
                            && BuildingHandler.buildingAmount[name] >= Unlocks[index].UnlockValue2)
                            UnlockDefense(Unlocks[index]);
                    }
                }
                return;
            case "Research":
                foreach (int index in ResearchUnlocks)
                    if (Unlocks[index].UnlockValue1 >= Research.amountResearched)
                        UnlockDefense(Unlocks[index]);
                return;
            default:
                Debug.LogError("Unlock type " + unlockType + " is not valid!");
                return;
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
        loadingSave = true;
        for (int i = 0; i < unlockList.Length; i++)
            UnlockDefense(GetUnlockableWithID(unlockList[i]));
        loadingSave = false;
    }

    // Iterates through all buildings in the tree, and returns the correct ID
    // This method will contain two for-loops until we find a solution to the UnlockLvl issue
    public Transform FindTechBuilding(int a)
    {
        // Checks the unlock list to see if the building exists
        for (int i = 0; i < unlocked.Count; i++)
            if (unlocked[i].GetComponent<TileClass>().getID() == a)
                return unlocked[i];

        // Backup check if the first iteration loop fails
        for (int i = 0; i < Unlocks.Length; i++)
            if (Unlocks[i].Building.GetComponent<TileClass>().getID() == a)
                return Unlocks[i].Building;

        // If both fail, return null
        return null;
    }

    public Transform FindTechBuildingWithName(string a)
    {
        // Checks the unlock list to see if the building exists
        for (int i = 0; i < unlocked.Count; i++)
            if (unlocked[i].name == a)
                return unlocked[i];

        // Backup check if the first iteration loop fails
        for (int i = 0; i < Unlocks.Length; i++)
            if (Unlocks[i].Building.name == a)
                return Unlocks[i].Building;

        // If both fail, return null
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
        UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
        UI.UOL.titleText = unlock.Building.name.ToUpper();
        UI.UOL.descriptionText = unlock.Building.GetComponent<TileClass>().description;
        UI.UOL.UpdateUI();

        // Add the building to the unlock list
        AddUnlocked(unlock.Building);

        // Set timescale
        Time.timeScale = 0f;
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
