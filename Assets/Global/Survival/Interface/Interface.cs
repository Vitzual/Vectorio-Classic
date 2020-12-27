using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Interface : MonoBehaviour
{
    // Survival script
    private Survival main;

    // Interace Elements
    public GameObject HotbarUI;
    public Canvas Overlay;
    public bool MenuOpen;
    public bool BuildingOpen;
    public bool ResearchOpen;
    public bool ShowingInfo;
    public TextMeshProUGUI GoldAmount;
    public TextMeshProUGUI EssenceAmount;
    public TextMeshProUGUI IridiumAmount;
    public ModalWindowManager UOL;
    public ProgressBar PowerUsageBar;
    public ProgressBar[] UpgradeProgressBars;
    public TextMeshProUGUI UpgradeProgressName;
    public ButtonManagerBasic SaveButton;
    public ButtonManagerBasicIcon[] hotbarButtons;

    // Start is called before the first frame update
    private void Start()
    {
        // Assign Survival script
        main = gameObject.GetComponent<Survival>();

        MenuOpen = false;
        ResearchOpen = false;
        BuildingOpen = false;
    }

    // Set the status of an overlay. 
    // a = name of the overlay
    // b = activate or deactive overlay
    public void SetOverlayStatus(string a, bool b)
    {
        if (b)
        {
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().interactable = true;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().interactable = false;
            Overlay.transform.Find(a).GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void ShowTileInfo(Collider2D a)
    {
        Transform b = Overlay.transform.Find("Prompt");
        b.transform.Find("Health").GetComponent<ProgressBar>().currentPercent = a.GetComponent<TileClass>().GetPercentage();
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
    }

    public void ShowSelectedInfo(Transform a)
    {
        Overlay.transform.Find("Selected").GetComponent<CanvasGroup>().alpha = 1;
        Transform b = Overlay.transform.Find("Selected");
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name;
        b.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = a.GetComponent<TileClass>().GetCost().ToString();
        b.transform.Find("Heat").GetComponent<TextMeshProUGUI>().text = a.GetComponent<TileClass>().GetHeat().ToString();
        b.transform.Find("Power").GetComponent<TextMeshProUGUI>().text = a.GetComponent<TileClass>().getConsumption().ToString();
        b.transform.Find("Building").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + a.name);
    }

    public void SetSelectedHotbar(int index)
    {
        if (index == 0)
        {
            HotbarUI.transform.Find("One").GetComponent<Button>().interactable = false;
        }
        else if (index == 1)
        {
            HotbarUI.transform.Find("Two").GetComponent<Button>().interactable = false;
        }
        else if (index == 2)
        {
            HotbarUI.transform.Find("Three").GetComponent<Button>().interactable = false;
        }
        else if (index == 3)
        {
            HotbarUI.transform.Find("Four").GetComponent<Button>().interactable = false;
        }
        else if (index == 4)
        {
            HotbarUI.transform.Find("Five").GetComponent<Button>().interactable = false;
        }
        else if (index == 5)
        {
            HotbarUI.transform.Find("Six").GetComponent<Button>().interactable = false;
        }
        else if (index == 6)
        {
            HotbarUI.transform.Find("Seven").GetComponent<Button>().interactable = false;
        }
        else if (index == 7)
        {
            HotbarUI.transform.Find("Eight").GetComponent<Button>().interactable = false;
        }
        else
        {
            HotbarUI.transform.Find("Nine").GetComponent<Button>().interactable = false;
        }
    }

    public void UpdateHotbar()
    {
        for (int i = 0; i < main.hotbar.Length; i++)
        {
            if (main.hotbar[i] != null)
                hotbarButtons[i].buttonIcon = Resources.Load<Sprite>("Sprites/" + main.hotbar[i].name);
            else
                hotbarButtons[i].buttonIcon = Resources.Load<Sprite>("Sprites/Undiscovered");
            hotbarButtons[i].UpdateUI();
        }
    }

    public void DisableActiveInfo()
    {
        HotbarUI.transform.Find("One").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Two").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Three").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Four").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Five").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Six").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Seven").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Eight").GetComponent<Button>().interactable = true;
        HotbarUI.transform.Find("Nine").GetComponent<Button>().interactable = true;
    }
}
