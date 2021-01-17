using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;

public class Technology : MonoBehaviour
{
    // Survival script
    public Survival main;

    // Interface script
    public Interface UI;

    // Unlock variables 
    public int UnlockLvl = 0;
    public bool UnlocksLeft = true;
    [System.Serializable]
    public class Unlockables
    {
        public Transform Unlock;
        public ButtonManagerBasicIcon InventoryButton;
        public Transform[] Enemy;
        public int[] AmountNeeded;
        public int[] AmountTracked;
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

    // Set unlock level
    public void SetUnlock(int a)
    {
        UnlockLvl = a;
    }

    // Set unlock progress 
    public void SetProgress(int[] a)
    {
        UnlockTier[UnlockLvl].AmountTracked = a;
    }

    // Force unlock progress to update
    // Usually called when loading a save
    public void ForceUpdateCheck()
    {
        StartNextUnlock();
        UpdateUnlockableGui();
    }

    // 
    public void UpdateUnlockableGui()
    {
        for (int i = 0; i < UnlockLvl; i++)
        {
            addUnlocked(UnlockTier[i].Unlock);
            Debug.Log(UnlockTier[i].Unlock.name);
            UnlockTier[i].InventoryButton.buttonIcon = Resources.Load<Sprite>("Sprites/" + UnlockTier[i].Unlock.name);
            UnlockTier[i].InventoryButton.UpdateUI();
        }
    }

    public void UpdateUnlock(Transform a)
    {
        if (UnlocksLeft)
        {
            // Itterate through list and update GUI accordingly
            for (int i = 0; i < UnlockTier[UnlockLvl].Enemy.Length; i++)
            {
                if (UnlockTier[UnlockLvl].Enemy[i].name == a.name)
                {
                    // Increment amount tracked and update GUI
                    UnlockTier[UnlockLvl].AmountTracked[i] += 1;
                    UpdateUnlockGui(i, ((double)UnlockTier[UnlockLvl].AmountTracked[i] / (double)UnlockTier[UnlockLvl].AmountNeeded[i]) * 100);
                }
            }

            // Check if requirements have been met
            bool RequirementsMetCheck = true;
            for (int i = 0; i < UnlockTier[UnlockLvl].Enemy.Length; i++)
            {
                if (UnlockTier[UnlockLvl].AmountTracked[i] < UnlockTier[UnlockLvl].AmountNeeded[i])
                {
                    RequirementsMetCheck = false;
                }
            }

            // If requirements met, unlock and start next unlock
            if (RequirementsMetCheck == true)
            {
                Transform newUnlock = UnlockTier[UnlockLvl].Unlock;

                UnlockDefense(newUnlock, UnlockTier[UnlockLvl].InventoryButton, newUnlock.GetComponent<TileClass>().GetDescription());
                StartNextUnlock();
            }
        }
    }

    public void UpdateUnlockGui(int a, double b)
    {
        UI.UpgradeProgressBars[a].currentPercent = (float)b;
    }

    public int[] GetAmountTracked()
    {
        try
        {
            return UnlockTier[UnlockLvl].AmountTracked;
        }
        catch
        {
            int[] result = new int[1];
            result[0] = 0;
            return result;
        }
    }

    public void StartNextUnlock()
    {
        UnlockLvl += 1;
        Transform c = UI.Overlay.transform.Find("Upgrade");

        try
        {
            int z = UnlockTier[UnlockLvl].Enemy.Length;
        }
        catch
        {
            UnlocksLeft = false;
            c.gameObject.SetActive(false);
        }
        finally
        {
            if (UnlocksLeft)
            {
                for (int i = 0; i <= 4; i++)
                {
                    UI.UpgradeProgressBars[i].currentPercent = 0;
                    try
                    {
                        UI.UpgradeProgressBars[i].transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + UnlockTier[UnlockLvl].Enemy[i].name);
                    }
                    catch
                    {
                        UI.UpgradeProgressBars[i].transform.Find("Type").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + "Undiscovered");
                    }
                }
                UI.UpgradeProgressName.text = UnlockTier[UnlockLvl].Unlock.transform.name;
            }
        }
    }

    public void UnlockDefense(Transform a, ButtonManagerBasicIcon b, string c)
    {
        addUnlocked(a);
        b.normalIcon.sprite = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.icon = Resources.Load<Sprite>("Sprites/" + a.transform.name);
        UI.UOL.titleText = a.transform.name.ToUpper();
        UI.UOL.descriptionText = c;
        UI.UOL.UpdateUI();
        UI.UOL.OpenWindow();
    }

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
        if (a == main.GetEssenceObj())
        {
            UI.ResearchButton.buttonIcon = Resources.Load<Sprite>("Sprites/Research");
            Research.research_unlocked = true;
        }
    }

    // Check if a building is unlocked
    public bool checkIfBuildingUnlocked(GameObject a)
    {
        for (int i = 0; i < unlocked.Count; i++)
        {
            if (a.name == unlocked[i].name)
            {
                return true;
            }
        }
        return false;
    }


}
