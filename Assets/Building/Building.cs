using UnityEngine;

public class Building : MonoBehaviour
{

    // Placement sprites
    public Sprite Turret;
    public Sprite Sniper;
    public Sprite Enemy;
    public Sprite Bolt;
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;
    private bool IsActive = false;
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
    private GameObject SelectedObj;

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

        
        if (QuickPlace == true) {
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
                if (rayHit.collider != null)
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
                if (rayHit.collider != null)
                {
                    Destroy(rayHit.collider.gameObject);
                }
            }
        }

        // Change selected object
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Adjustment = 1f;
            Selected.sprite = Turret;
            SelectedObj = TurretObj;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Adjustment = 1f;
            Selected.sprite = Enemy;
            SelectedObj = EnemyObj;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Adjustment = 1f;
            Selected.sprite = Sniper;
            SelectedObj = SniperObj;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Adjustment = 1f;
            Selected.sprite = Bolt;
            SelectedObj = BoltObj;
        }
        //else if (Input.GetKeyDown(KeyCode.G))
        //{
        //    IsActive = !IsActive;
        //    GridObj.gameObject.SetActive(IsActive);
        //}
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuickPlace = !QuickPlace;
        }

    }

    public void AdjustAlphaValue()
    {
        if (AdjustLimiter == 20)
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
        Selected.sprite = Turret;
        SelectedObj = TurretObj;
    }

    public void SetSniper()
    {
        Selected.sprite = Sniper;
        SelectedObj = SniperObj;
    }

    public void SetBolt()
    {
        Selected.sprite = Bolt;
        SelectedObj = BoltObj;
    }

    public void SetTriangle()
    {
        Selected.sprite = Enemy;
        SelectedObj = EnemyObj;
    }

    public void Quit()
    {
        Application.Quit();
    }

}