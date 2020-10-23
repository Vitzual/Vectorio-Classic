using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UGUIMiniMap;

public class bl_MiniMapItem : MonoBehaviour {

    [Separator("TARGET")]
    [Tooltip("Transform to UI Icon will be follow")]
    public Transform Target = null;
    [Tooltip("Custom Position from target position")]
    public Vector3 OffSet = Vector3.zero;

    [Separator("ICON")]
    public Sprite Icon = null;
    public Sprite DeathIcon = null;
    public Color IconColor = new Color(1, 1, 1, 0.9f);
    [Range(1,100)]public float Size = 20;

    [Separator("CIRCLE AREA")]
    public bool ShowCircleArea = false;
    [Range(1,100)]public float CircleAreaRadius = 10;
    public Color CircleAreaColor = new Color(1, 1, 1, 0.9f);

    [Separator("ICON BUTTON")]
    [Tooltip("UI can interact when press it?")]
    public bool isInteractable = true;
    public InteracableAction interacableAction = InteracableAction.OnHover;
    [TextArea(2,2)]public string InfoItem = "Info Icon here";
    public bool keepTextAlwaysFacingUp = false;

    [Separator("SETTINGS")]
    [Tooltip("Can Icon show when is off screen?")]
    public bool OffScreen = true;
    public bool DestroyWithObject = true;
    [Range(0,5)]public float BorderOffScreen = 0.01f;
    [Range(1,50)]public float OffScreenSize = 10;
    public bool isHoofdPunt = false;
    [Tooltip("Time before render/show item in minimap after instance")]
    [Range(0,3)]public float RenderDelay = 0.3f;
    public ItemEffect m_Effect = ItemEffect.None;
    public bool useCustomIconPrefab = false;
    public GameObject CustomIconPrefab;
    public bl_IconItem iconItem { get; private set; }

    //Privates
    private Image Graphic = null;
    private RectTransform GraphicRect;
    private RectTransform RectRoot;
    private GameObject cacheItem = null;
    private RectTransform CircleAreaRect = null;
    private Vector3 position;
    Vector3 screenPos = Vector3.zero;
    private bool clampedY, clampedX = false;

    /// <summary>
    /// Get all required component in start
    /// </summary>
    void Start()
    {
        if (MiniMapOwner != null)
        {
            CreateIcon();
            MiniMapOwner.RegisterItem(this);
        }
        else { Debug.Log("You need a MiniMap in scene for use MiniMap Items."); }
    }

    /// <summary>
    /// 
    /// </summary>
    void CreateIcon()
    {
        if (MiniMapOwner.hasError || !this.enabled) return;
        //Instantiate UI in canvas
        GameObject g = bl_MiniMapData.Instance.IconPrefab;
        if(useCustomIconPrefab && CustomIconPrefab != null) { g = CustomIconPrefab; }
        cacheItem = Instantiate(g) as GameObject;
        RectRoot = OffScreen ? MiniMapOwner.MiniMapUIRoot : MiniMapOwner.IconsParent;
        //SetUp Icon UI
        iconItem = cacheItem.GetComponent<bl_IconItem>();
        iconItem.SetUp(this);
        Graphic = iconItem.TargetGraphic;
        GraphicRect = Graphic.GetComponent<RectTransform>();
        if (Icon != null) { Graphic.sprite = Icon; Graphic.color = IconColor; }
        cacheItem.transform.SetParent(RectRoot.transform, false);
        GraphicRect.anchoredPosition = Vector2.zero;
        if (Target == null) { Target = transform; }
        StartEffect();
        iconItem.DelayStart(RenderDelay);
        iconItem.SetText(InfoItem);
        if (ShowCircleArea)
        {
           CircleAreaRect = iconItem.SetCircleArea(CircleAreaRadius, CircleAreaColor);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnUpdateItem()
    {    
        //If a component missing, return for avoid bugs.
        if (Target == null)
            return;
        if (Graphic == null)
            return;

        IconControl();
    }

    /// <summary>
    /// 
    /// </summary>
    void IconControl()
    {
        if (isHoofdPunt)
        {
            if (MiniMapOwner.Target != null)
            {
                transform.position = MiniMapOwner.Target.TransformPoint((MiniMapOwner.Target.forward) * 100);
            }
        }
        //Setting the modify position
        Vector3 CorrectPosition = TargetPosition + OffSet;
        Vector2 fullSize = RectRoot.rect.size;
        Vector2 size = RectRoot.rect.size * 0.5f;
        //Convert the position of target in ViewPortPoint
        Vector2 wvp = bl_MiniMap.MiniMapCamera.WorldToViewportPoint(CorrectPosition);
        //Calculate the position of target and convert into position of screen
        position = new Vector2((wvp.x * fullSize.x) - size.x, (wvp.y * fullSize.y) - size.y);

        Vector2 UnClampPosition = position;
        //if show off screen
        if (OffScreen)
        {
            //Calculate the max and min distance to move the UI
            //this clamp in the RectRoot sizeDela for border
            position.x = bl_MiniMapUtils.ClampBorders(position.x, -(size.x - BorderOffScreen), (size.x - BorderOffScreen), out clampedX);
            position.y = bl_MiniMapUtils.ClampBorders(position.y, -size.y - BorderOffScreen, (size.y - BorderOffScreen), out clampedY);
        }

        //calculate the position of UI again, determine if off screen
        //if off screen reduce the size
        float Iconsize = Size;
        //Use this (useCompassRotation when have a circle miniMap)
        if (MiniMapOwner.useCompassRotation)
        {
            //Calculate difference
            Vector3 forward = Target.position - MiniMapOwner.TargetPosition;
            //Position of target from camera
            Vector3 cameraRelativeDir = bl_MiniMap.MiniMapCamera.transform.InverseTransformDirection(forward);
            //normalize values for screen fix
            cameraRelativeDir.z = 0;
            cameraRelativeDir = cameraRelativeDir.normalized * 0.5f;
            //Convert values to positive for calculate area OnScreen and OffScreen.
            float posPositiveX = Mathf.Abs(position.x);
            float relativePositiveX = Mathf.Abs((0.5f + (cameraRelativeDir.x * MiniMapOwner.CompassSize)));
            //when target if offScreen clamp position in circle area.
            if (posPositiveX >= relativePositiveX)
            {
                screenPos.x = 0.5f + (cameraRelativeDir.x * MiniMapOwner.CompassSize)/*/ Camera.main.aspect*/;
                screenPos.y = 0.5f + (cameraRelativeDir.y * MiniMapOwner.CompassSize);
                position = screenPos;
                Iconsize = OffScreenSize;
            }
            else
            {
                Iconsize = Size;
            }
        }
        else
        {
            //check if the icon is out of the minimap bounds
            if (clampedX || clampedY)
            {
                Iconsize = OffScreenSize;
            }
            else
            {
                Iconsize = Size;
            }
        }
        //Apply position to the UI (for follow)
        GraphicRect.anchoredPosition = position;
        if (CircleAreaRect != null) { CircleAreaRect.anchoredPosition = UnClampPosition; }
        //Change size with smooth transition
        float CorrectSize = Iconsize * MiniMapOwner.IconMultiplier;
        GraphicRect.sizeDelta = Vector2.Lerp(GraphicRect.sizeDelta, new Vector2(CorrectSize, CorrectSize), Time.deltaTime * 8);

        if (MiniMapOwner.iconsAlwaysFacingUp)
        {
            //with this the icon rotation always will facing up
            if (MiniMapOwner.canvasRenderMode == bl_MiniMap.RenderMode.Mode2D) { GraphicRect.up = Vector3.up; }
            else
            {
                Quaternion r = Quaternion.identity;
                r.x = Target.rotation.x;
                GraphicRect.localRotation = r;
            }
        }
        else
        {
            //with this the rotation icon will depend of target
            Vector3 vre = MiniMapOwner.transform.eulerAngles;
            Vector3 re = Vector3.zero;
            //Fix player rotation for apply to el icon.
            re.z = ((-Target.rotation.eulerAngles.y) + vre.y);
            Quaternion q = Quaternion.Euler(re);
            GraphicRect.rotation = q;

            if (keepTextAlwaysFacingUp && iconItem.textAreaBackground != null)
            {
                iconItem.textRect.up = Vector3.up;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void StartEffect()
    {
        Animator a = Graphic.GetComponent<Animator>();
        if (m_Effect == ItemEffect.Pulsing)
        {
            a.SetInteger("Type", 2);
        }
        else if (m_Effect == ItemEffect.Fade)
        {
            a.SetInteger("Type", 1);
        }
    }

    /// <summary>
    /// When player or the target die, disable,remove,etc..
    /// call this for remove the item UI from Map
    /// for change to other icon and disable in certain time
    /// or destroy immediate
    /// </summary>
    /// <param name="inmediate"></param>
    public void DestroyItem(bool inmediate)
    {
        if (Graphic == null)
        {
            Debug.Log("Graphic Item of " + this.name + " not exist in scene");
            return;
        }

        if (DeathIcon == null || inmediate)
        {
            Graphic.GetComponent<bl_IconItem>().DestroyIcon(inmediate);
        }
        else
        {
            Graphic.GetComponent<bl_IconItem>().DestroyIcon(inmediate,DeathIcon);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ico"></param>
    public void SetIcon(Sprite ico)
    {
        if(cacheItem == null)
        {
            Debug.LogWarning("You can't set a icon before create the item.");
            return;
        }

        cacheItem.GetComponent<bl_IconItem>().SetIcon(ico);
    }

    /// <summary>
    /// Show a visible circle area in the minimap with this
    /// item as center
    /// </summary>
    public void SetCircleArea(float radius, Color AreaColor)
    {
        CircleAreaRect = cacheItem.GetComponent<bl_IconItem>().SetCircleArea(radius,AreaColor);
    }

    /// <summary>
    /// 
    /// </summary>
    public void HideCircleArea()
    {
        cacheItem.GetComponent<bl_IconItem>().HideCircleArea();
        CircleAreaRect = null;
    }

    /// <summary>
    /// Call this for hide item in miniMap
    /// For show again just call "ShowItem()"
    /// NOTE: For destroy item call "DestroyItem(bool immediate)" instant this.
    /// </summary>
    public void HideItem()
    {
        if (cacheItem != null)
        {
            cacheItem.SetActive(false);
        }
        else
        {
            Debug.Log("There is no item to disable.");
        }
    }

    /// <summary>
    /// Call this for show again the item in miniMap when is hide
    /// </summary>
    public void ShowItem()
    {
        if (cacheItem != null)
        {
            cacheItem.SetActive(true);
            cacheItem.GetComponent<bl_IconItem>().SetVisibleAlpha();
        }
        else
        {
            Debug.Log("There is no item to active.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetIconText(string text)
    {
        if (iconItem == null) return;
        iconItem.SetText(text);
    }

    /// <summary>
    /// If you need destroy icon when this gameObject is destroy.
    /// </summary>
    void OnDestroy()
    {
       if(MiniMapOwner != null) MiniMapOwner.RemoveItem(this);
        if (DestroyWithObject)
        {
            DestroyItem(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 TargetPosition
    {
        get
        {
            if (Target == null)
            {
                return Vector3.zero;
            }

            return new Vector3(Target.position.x, 0, Target.position.z);
        }
    }

    [System.Serializable]
    public enum InteracableAction
    {
        OnHover,
        OnTouch,
    }

    private bl_MiniMap _minimap = null;
    private bl_MiniMap MiniMapOwner
    {
        get
        {
            if (_minimap == null)
            {
                _minimap = bl_MiniMapUtils.GetMiniMap();
            }
            return _minimap;
        }
    }
}