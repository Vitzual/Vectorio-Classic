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

    public bool loadingSave = false;

    // Unlock variables 
    public int UnlockAmount = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockables
    {
        public Transform Unlock;
        public ButtonManagerBasicIcon InventoryButton;
        public int HeatNeeded;
        public bool Unlocked = false;
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
                addUnlocked(UnlockTier[indexOfLowestHeat].Unlock);

                // Updates the inventory button of the unlock
                UnlockTier[indexOfLowestHeat].InventoryButton.buttonIcon = Resources.Load<Sprite>("Sprites/" + UnlockTier[indexOfLowestHeat].Unlock.name);
                UnlockTier[indexOfLowestHeat].InventoryButton.UpdateUI();

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
            if (UnlockTier[i].Unlock.GetComponent<TileClass>().getID() == a)
                return UnlockTier[i].Unlock;

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
                    UnlockDefense(UnlockTier[i].Unlock, UnlockTier[i].InventoryButton, UnlockTier[i].Unlock.GetComponent<TileClass>().GetDescription());
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
            c.Find("Name").GetComponent<TextMeshProUGUI>().text = UnlockTier[indexOfLowestHeat].Unlock.name.ToString().ToUpper();
            c.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[indexOfLowestHeat].Unlock.name.ToString());
        }
    }

    // Unlocks a defense and displays to screen
    public void UnlockDefense(Transform a, ButtonManagerBasicIcon b, string c)
    {
        addUnlocked(a);
        UnlockAmount++;
        b.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.titleText = a.transform.name.ToUpper();
        UI.UOL.descriptionText = c;
        UI.UOL.UpdateUI();
        if (a.name == "Essence Drill") UI.showResearchUnlock();
        else
        {
            UI.UOL.OpenWindow();
            UI.UOLOpen = true;
        }
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
            if (UnlockTier[i].Unlock == a)
                UnlockTier[i].Unlocked = true;

        // If essence object, unlock research
        if (a == main.GetEssenceObj())
        {
            UI.ResearchButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
            UI.ResearchButton.UpdateUI();
            Research.research_unlocked = true;
        }
    }

    public void closeUnlockTree()
    {
        UnlocksLeft = false;
        UI.Overlay.transform.Find("Upgrade").gameObject.SetActive(false);
        UnlockAmount = UnlockTier.Length;
        return;
    }
}
