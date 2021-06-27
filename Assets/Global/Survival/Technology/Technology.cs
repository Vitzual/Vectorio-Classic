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
    public int UnlockAmount = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockables
    {
        public string Name;
        public Transform Building;
        public int HeatNeeded;
        public bool Unlocked = false;
        public string Type;
        public Image InvIcon;
        public TextMeshProUGUI InvName;
    }
    public Unlockables[] UnlockTier;

    // Create unlocked list
    public List<Transform> unlocked = new List<Transform>();

    private void Start()
    {
        // Assign Survival script
        main = gameObject.GetComponent<Survival>();

        // Assign Interface script
        UI = gameObject.GetComponent<Interface>();
    }

    public Unlockables FindUnlockable(Transform building)
    {
        foreach (Unlockables unlockable in UnlockTier)
            if (unlockable.Building == building)
                return unlockable;
        return null;
    }

    // Sets the unlock tree back to the level that was saved
    public void loadSaveData(int unlockAmount)
    {
        loadingSave = true;
        UnlockAmount = unlockAmount;

        // Unlock set amount of tech https://www.youtube.com/watch?v=vVrjh-1CTtQ
        int unlockedCount = 0;
        while (unlockedCount < unlockAmount)
        {
            int lowestHeat = int.MaxValue;
            int indexOfLowestHeat = -1;
            for (int i = 0; i < UnlockTier.Length; i++)
            {
                if (!UnlockTier[i].Unlocked && UnlockTier[i].HeatNeeded < lowestHeat)
                {
                    lowestHeat = UnlockTier[i].HeatNeeded;
                    indexOfLowestHeat = i;
                }
            }

            if (indexOfLowestHeat != -1)
            {
                // Adds the unlock transform to the unlocked list
                addUnlocked(UnlockTier[indexOfLowestHeat].Building);

                // Updates the inventory button of the unlock
                UnlockTier[indexOfLowestHeat].InvIcon.sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[indexOfLowestHeat].Building.name);
                UnlockTier[indexOfLowestHeat].InvName.text = UnlockTier[indexOfLowestHeat].Building.name.ToUpper();

                // Increment unlock counter
                unlockedCount++;
            }
            else
            {
                closeUnlockTree();
                return;
            }
        }

        // Start next unlock
        loadingSave = false;
        StartNextUnlock();
    }

    // Set unlock level
    public void SetUnlock(int a)
    {
        UnlockAmount = a;
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
        for (int i = 0; i < UnlockTier.Length; i++)
            if (UnlockTier[i].Building.GetComponent<TileClass>().getID() == a)
                return UnlockTier[i].Building;

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
        for (int i = 0; i < UnlockTier.Length; i++)
            if (UnlockTier[i].Building.name == a)
                return UnlockTier[i].Building;

        // If both fail, return null
        return null;
    }

    // Gets called when checking if something should be unlocked
    public void UpdateUnlock(int a)
    {
        if (UnlocksLeft)
        {
            for (int i = 0; i < UnlockTier.Length; i++)
            {
                if (!UnlockTier[i].Unlocked && UnlockTier[i].HeatNeeded <= a)
                {
                    UnlockDefense(i);
                    StartNextUnlock();
                }
            }
        }
    }

    // Starts the next unlock 
    public void StartNextUnlock()
    {
        Transform c = UI.Overlay.transform.Find("Upgrade");

        int lowestHeat = int.MaxValue;
        int indexOfLowestHeat = -1;

        for (int i = 0; i < UnlockTier.Length; i++)
            if (!UnlockTier[i].Unlocked && UnlockTier[i].HeatNeeded < lowestHeat)
            {
                lowestHeat = UnlockTier[i].HeatNeeded;
                indexOfLowestHeat = i;
            }

        if (indexOfLowestHeat == -1)
            closeUnlockTree();
        else {
            c.Find("Amount").GetComponent<TextMeshProUGUI>().text = UnlockTier[indexOfLowestHeat].HeatNeeded.ToString();
            c.Find("Name").GetComponent<TextMeshProUGUI>().text = UnlockTier[indexOfLowestHeat].Building.name.ToString().ToUpper();
            c.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[indexOfLowestHeat].Building.name.ToString());
        }
    }

    // Unlocks a defense and displays to screen
    public void UnlockDefense(int index)
    {
        // Increment update count
        Unlockables unlock = UnlockTier[index];
        UnlockAmount++;

        // Add the building to the unlock list
        addUnlocked(unlock.Building);

        // Set button icon and text
        unlock.InvIcon.sprite = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
        unlock.InvName.text = unlock.Building.name.ToUpper();

        // Set the UOL
        UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + unlock.Building.name);
        UI.UOL.titleText = unlock.Building.name.ToUpper();
        UI.UOL.descriptionText = unlock.Building.GetComponent<TileClass>().description;
        UI.UOL.UpdateUI();

        // Check if building is RL or E
        if (unlock.Building.name == "Research Lab")
        {
            UI.showResearchUnlock();
            rButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
        }
        else if (unlock.Building.name == "Energizer") UI.showEnergizerUnlock();
        else
        {
            UI.UOL.OpenWindow();
            UI.UOLOpen = true;
        }

        // Set timescale
        Time.timeScale = 0f;
    }

    // Checks if a building is unlocked
    public bool checkIfUnlocked(Transform a)
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
    public void addUnlocked(Transform a)
    {
        unlocked.Add(a);

        // Find the object in the array list
        for (int i = 0; i < UnlockTier.Length; i++)
            if (UnlockTier[i].Building == a)
                UnlockTier[i].Unlocked = true;
    }

    public void closeUnlockTree()
    {
        UnlocksLeft = false;
        UI.Overlay.transform.Find("Upgrade").gameObject.SetActive(false);
        UnlockAmount = UnlockTier.Length;
        return;
    }
}
