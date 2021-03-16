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
        UnlockAmount = unlockAmount - 1;

        // Iterate through the UnlockTier array and find each unlock in order
        for (int a = 0; a < UnlockTier.Length; a++)
        {
            int lowestHeat = int.MaxValue;
            int indexHolder = -1;
            for (int b = 0; b < UnlockTier.Length; b++)
            {
                if (!UnlockTier[b].Unlocked && UnlockTier[b].HeatNeeded < lowestHeat)
                {
                    lowestHeat = UnlockTier[b].HeatNeeded;
                    indexHolder = b;
                }
            }
            if (indexHolder != -1)
            {
                // Adds the unlock transform to the unlocked list
                addUnlocked(UnlockTier[indexHolder].Unlock);

                // Updates the inventory button of the unlock
                UnlockTier[indexHolder].InventoryButton.buttonIcon = Resources.Load<Sprite>("Sprites/" + UnlockTier[indexHolder].Unlock.name);
                UnlockTier[indexHolder].InventoryButton.UpdateUI();
            }
            else
            {
                closeUnlockTree();
                return;
            }
        }

        // Start next unlock
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
            if (UnlockTier[UnlockAmount].HeatNeeded <= a)
            {
                UnlockDefense(UnlockTier[UnlockAmount].Unlock, UnlockTier[UnlockAmount].InventoryButton, UnlockTier[UnlockAmount].Unlock.GetComponent<TileClass>().GetDescription());
                StartNextUnlock();
            }
        }
    }

    // Starts the next unlock 
    public void StartNextUnlock()
    {
        Transform c = UI.Overlay.transform.Find("Upgrade");

        int lowestHeat = int.MaxValue;
        bool anyLeft = false;

        for (int i = 0; i < UnlockTier.Length; i++)
            if (!UnlockTier[i].Unlocked && UnlockTier[i].HeatNeeded < lowestHeat)
            {
                lowestHeat = UnlockTier[i].HeatNeeded;
                UnlockAmount = i;
                anyLeft = true;
            }

        if (!anyLeft)
            closeUnlockTree();
        else {
            c.Find("Amount").GetComponent<TextMeshProUGUI>().text = UnlockTier[UnlockAmount].HeatNeeded.ToString();
            c.Find("Name").GetComponent<TextMeshProUGUI>().text = UnlockTier[UnlockAmount].Unlock.name.ToString().ToUpper();
            c.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[UnlockAmount].Unlock.name.ToString());
        }
    }

    // Unlocks a defense and displays to screen
    public void UnlockDefense(Transform a, ButtonManagerBasicIcon b, string c)
    {
        addUnlocked(a);
        b.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.titleText = a.transform.name.ToUpper();
        UI.UOL.descriptionText = c;
        UI.UOL.UpdateUI();
        UI.UOL.OpenWindow();
        UI.UOLOpen = true;
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
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
