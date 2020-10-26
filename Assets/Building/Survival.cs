using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Survival : MonoBehaviour
{

    // Player stats
    public int gold = 100;
    protected int essence = 0;
    protected int iridium = 0;

    // Placement sprites
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;

    // Object placements
    [SerializeField]
    private GameObject SniperObj;
    [SerializeField]
    private GameObject TurretObj;
    [SerializeField]
    private GameObject BoltObj;
    [SerializeField]
    private GameObject ShotgunObj;
    [SerializeField]
    private GameObject WallObj;
    [SerializeField]
    private GameObject SMGObj;
    [SerializeField]
    private GameObject CollectorObj;
    [SerializeField]
    private GameObject ConveyorObj;
    private GameObject SelectedObj;
    private GameObject LastObj;
    private float rotation = 0f;

    // UI Elements
    public Canvas Overlay;
    private bool MenuOpen;
    private bool BuildingOpen;
    readonly int CacheAmount = 10;
    public TextMeshProUGUI GoldAmount;
    int CurrentCache = 1;

    // Internal placement variables
    [SerializeField]
    private LayerMask TileLayer;
    private Vector2 MousePos;
    delegate void HotbarItem();
    List<HotbarItem> hotbar = new List<HotbarItem>();

    private void Start()
    {
        Selected = GetComponent<SpriteRenderer>();
        MenuOpen = false;
        BuildingOpen = false;

        // Temporary
        hotbar.Add(SetTurret);
        hotbar.Add(SetCollector);
        hotbar.Add(SetConveyor);
        hotbar.Add(SetWall);

        InvokeRepeating("UpdateGui", 0f, 1f);
    }

    private void Update()
    {
        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5*Mathf.Round(MousePos.x/5), 5*Mathf.Round(MousePos.y/5));

        // Make color flash
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        tmp.a = Adjustment;
        this.GetComponent<SpriteRenderer>().color = tmp;
        AdjustAlphaValue();

        if (Input.mousePosition.y > 170f)
        {
            if (SelectedObj == null)
            {
                if (CurrentCache >= CacheAmount)
                {
                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);
                    CurrentCache = 0;
                    if (rayHit.collider != null)
                    {
                        ShowTileInfo(rayHit.collider);
                    } 
                    else
                    {
                        Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
                    }
                }
                CurrentCache += 1;
            }
            // If user left clicks, place object
            else if (Input.GetButton("Fire1") && !BuildingOpen)
            {
                Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
                RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                // Raycast tile to see if there is already a tile placed
                if (rayHit.collider == null)
                {
                    LastObj = Instantiate(SelectedObj, transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    LastObj.name = SelectedObj.name;
                    if (SelectedObj == WallObj)
                    {
                        CalculateWallPlacement();
                    }
                }
            }
            // If user right clicks, place object
            else if (Input.GetButton("Fire2") && !BuildingOpen)
            {
                Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 0;
                RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                // Raycast tile to see if there is already a tile placed
                if (rayHit.collider != null && rayHit.collider.name != "Hub")
                {
                    Destroy(rayHit.collider.gameObject);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectHotbar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectHotbar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectHotbar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectHotbar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectHotbar(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectHotbar(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectHotbar(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectHotbar(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SelectHotbar(8);
        }
        else if (Input.GetKeyDown(KeyCode.R) && BuildingOpen == false && MenuOpen == false && SelectedObj != null)
        {
            rotation = rotation -= 90f;
            if (rotation == -360f)
            {
                rotation = 0;
            }
            Selected.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        }
        if (Input.GetKeyDown(KeyCode.E) && BuildingOpen == false)
        {
            BuildingOpen = true;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().interactable = true;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && BuildingOpen == true)
        {
            BuildingOpen = false;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().interactable = false;
            Overlay.transform.Find("Survival Menu").GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            DisableActiveInfo();
            Overlay.transform.Find("Selected Info").GetComponent<CanvasGroup>().alpha = 0;
            Selected.sprite = null;
            SelectedObj = null;
            rotation = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuOpen == false)
        {
            MenuOpen = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = false;
        }
    }

    void ShowTileInfo(Collider2D a)
    {
        Overlay.transform.Find("Hovering Stats").GetComponent<CanvasGroup>().alpha = 1;
        Transform b = Overlay.transform.Find("Hovering Stats");
        b.transform.Find("Health").GetComponent<ProgressBar>().currentPercent = a.GetComponent<TileClass>().GetPercentage();
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name + " (Level " + a.GetComponent<TileClass>().GetLevel() + ")";
        b.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + a.name);
    }

    void ShowSelectedInfo(GameObject a)
    {
        Overlay.transform.Find("Selected Info").GetComponent<CanvasGroup>().alpha = 1;
        Transform b = Overlay.transform.Find("Selected Info");
        b.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = a.name + " (Level " + a.GetComponent<TileClass>().GetLevel() + ")";
        b.transform.Find("Cost").GetComponent<TextMeshProUGUI>().text = "Cost:      " + a.GetComponent<TileClass>().GetCost();
        b.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + a.name);
    }

    void CalculateWallPlacement()
    {
        RaycastHit2D a = Physics2D.Raycast(new Vector2(MousePos.x+5f,MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(MousePos.x-5f,MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(MousePos.x,MousePos.y+5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(MousePos.x,MousePos.y-5f), Vector2.zero, Mathf.Infinity, TileLayer);
        if (a.collider != null && a.collider.name == "Wall")
        {
            a.collider.GetComponent<WallAI>().UpdateSprite(1);
            LastObj.GetComponent<WallAI>().UpdateSprite(3);
        }
        if (b.collider != null && b.collider.name == "Wall")
        {
            b.collider.GetComponent<WallAI>().UpdateSprite(3);
            LastObj.GetComponent<WallAI>().UpdateSprite(1);
        }
        if (c.collider != null && c.collider.name == "Wall")
        {
            c.collider.GetComponent<WallAI>().UpdateSprite(2);
            LastObj.GetComponent<WallAI>().UpdateSprite(4);
        }
        if (d.collider != null && d.collider.name == "Wall")
        {
            d.collider.GetComponent<WallAI>().UpdateSprite(4);
            LastObj.GetComponent<WallAI>().UpdateSprite(2);
        }
    }

    private void UpdateGui()
    {
        GoldAmount.text = ""+gold;
    }

    public void AddGold(int a)
    {
        gold += a;
    }

    public void AdjustAlphaValue()
    {
        if (AdjustLimiter == 10)
        {
            if (AdjustSwitch == false)
            {
                Adjustment -= 0.1f;
            }
            else if (AdjustSwitch == true)
            {
                Adjustment += 0.1f;
            }
            if (AdjustSwitch == false && Adjustment <= 0f)
            {
                AdjustSwitch = true;
            }
            else if (AdjustSwitch == true && Adjustment >= 1f)
            {
                AdjustSwitch = false;
            }
            AdjustLimiter = 0;
        }
        else
        {
            AdjustLimiter += 1;
        }
    }

    public void SelectHotbar(int index)
    {
        try
        {
            hotbar[index]();
        } catch { return; }
        if (index == 0)
        {
            Overlay.transform.Find("One").GetComponent<Button>().interactable = false;
        }
        else if (index == 1)
        {
            Overlay.transform.Find("Two").GetComponent<Button>().interactable = false;
        }
        else if (index == 2)
        {
            Overlay.transform.Find("Three").GetComponent<Button>().interactable = false;
        }
        else if (index == 3)
        {
            Overlay.transform.Find("Four").GetComponent<Button>().interactable = false;
        }
        else if (index == 4)
        {
            Overlay.transform.Find("Five").GetComponent<Button>().interactable = false;
        }
        else if (index == 5)
        {
            Overlay.transform.Find("Six").GetComponent<Button>().interactable = false;
        }
        else if (index == 6)
        {
            Overlay.transform.Find("Seven").GetComponent<Button>().interactable = false;
        }
        else if (index == 7)
        {
            Overlay.transform.Find("Eight").GetComponent<Button>().interactable = false;
        }
        else
        {
            Overlay.transform.Find("Nine").GetComponent<Button>().interactable = false;
        }
    }

    public void SetTurret()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = TurretObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetShotgun()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = ShotgunObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetSniper()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = SniperObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetSMG()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = SMGObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetBolt()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = BoltObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetWall()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = WallObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetCollector()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = CollectorObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void SetConveyor()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        SelectedObj = ConveyorObj;
        Selected.sprite = Resources.Load<Sprite>("Sprites/" + SelectedObj.name);
        ShowSelectedInfo(SelectedObj);
    }

    public void DisableActiveInfo()
    {
        Overlay.transform.Find("One").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Two").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Three").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Four").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Five").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Six").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Seven").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Eight").GetComponent<Button>().interactable = true;
        Overlay.transform.Find("Nine").GetComponent<Button>().interactable = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

}