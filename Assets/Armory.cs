using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Armory : MonoBehaviour
{
    // Armory variables
    public CosmeticButton button;
    public Buildable buildable;
    public List<CosmeticButton> cosmeticButtons;
    public GameObject listObj;
    public TextMeshProUGUI empty;
    public Transform list;

    // Start is called before the first frame update
    public void SetupArmory(Buildable buildable)
    {
        // Clear previous cosmetic buttons
        if (cosmeticButtons != null)
            foreach (CosmeticButton button in cosmeticButtons)
                Recycler.AddRecyclable(button.transform);
        cosmeticButtons = new List<CosmeticButton>();

        // Check if buildable is null
        if (buildable == null)
        {
            listObj.SetActive(false);
            empty.gameObject.SetActive(true);
            empty.text = "NO SKINS AVAILABLE!";
            return;
        }

        // Create new list of cosmetics
        this.buildable = buildable;
        if (buildable.availableCosmetics != null && buildable.availableCosmetics.Count > 0)
        {
            listObj.SetActive(true);
            empty.gameObject.SetActive(false);

            foreach(Cosmetic cosmetic in buildable.availableCosmetics)
            {
                CosmeticButton newButton = Instantiate(button, Vector2.zero, Quaternion.identity).GetComponent<CosmeticButton>();
                newButton.transform.SetParent(list);
                newButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newButton.Setup(cosmetic, buildable);
                newButton.locked.SetActive(!cosmetic.validateLocalApplication());
                newButton.lockedText.text = cosmetic.requirement;
                cosmeticButtons.Add(newButton);
            }
        }
        else
        {
            listObj.SetActive(false);
            empty.gameObject.SetActive(true);
            empty.text = "NO SKINS UNLOCKED FOR\n<SIZE = 30><b>" + buildable.building.name;
        }
    }
}
