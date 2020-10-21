using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    // Placement sprites
    public Sprite Turret;
    public Sprite Sniper;
    public Sprite Enemy;
    public Sprite Bolt;
    public Sprite Shotgun;
    public Sprite SMG;
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;
    private bool QuickPlace = true;

    // Object placements
    [SerializeField]
    private GameObject GridObj;
    [SerializeField]
    private GameObject SniperObj;
    [SerializeField]
    private GameObject TurretObj;
    [SerializeField]
    private GameObject EnemyObj;
    [SerializeField]
    private GameObject BoltObj;
    [SerializeField]
    private GameObject ShotgunObj;
    [SerializeField]
    private GameObject SMGObj;
    private GameObject SelectedObj;

    // UI Elements
    public Canvas Overlay;

    // Internal placement variables
    [SerializeField]
    private LayerMask TileLayer;
    private Vector2 MousePos;

    private void Start()
    {
        Selected = GetComponent<SpriteRenderer>();
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
            if (QuickPlace == true)
            {
                // If user left clicks, place object
                if (Input.GetButton("Fire1") && SelectedObj != null)
                {

                    Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider == null)
                    {
                        Instantiate(SelectedObj, transform.position, Quaternion.identity);
                    }
                }

                // If user right clicks, place object
                else if (Input.GetButton("Fire2") && SelectedObj != null)
                {

                    Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider != null && rayHit.collider.name != "Hub")
                    {
                        Destroy(rayHit.collider.gameObject);
                    }
                }
            }
            else if (QuickPlace == false)
            {
                // If user left clicks, place object
                if (Input.GetButtonDown("Fire1"))
                {

                    Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider == null)
                    {
                        Instantiate(SelectedObj, transform.position, Quaternion.identity);
                    }
                }

                // If user right clicks, place object
                else if (Input.GetButtonDown("Fire2"))
                {

                    Vector2 mouseRay = Camera.main.ScreenToWorldPoint(transform.position);
                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider != null && rayHit.collider.name != "Hub")
                    {
                        Destroy(rayHit.collider.gameObject);
                    }
                }
            }
        }

        // Change selected object
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTurret();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetTriangle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetShotgun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSniper();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSMG();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetBolt();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuickPlace = !QuickPlace;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableActiveInfo();
            Selected.sprite = null;
            SelectedObj = null;
        }

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

    public void SetTurret()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Turret Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Turret;
        SelectedObj = TurretObj;
    }

    public void SetShotgun()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Shotgun Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Shotgun;
        SelectedObj = ShotgunObj;
    }

    public void SetSniper()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Sniper Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Sniper;
        SelectedObj = SniperObj;
    }

    public void SetSMG()
    {
        DisableActiveInfo();
        Overlay.transform.Find("SMG Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = SMG;
        SelectedObj = SMGObj;
    }

    public void SetBolt()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Pulser Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Bolt;
        SelectedObj = BoltObj;
    }

    public void SetTriangle()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Enemy;
        SelectedObj = EnemyObj;
    }

    public void DisableActiveInfo()
    {
        Overlay.transform.Find("Turret Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Shotgun Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Sniper Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("SMG Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Pulser Info").GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }

}