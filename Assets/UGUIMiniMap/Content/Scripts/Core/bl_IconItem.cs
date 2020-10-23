using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UGUIMiniMap;
using UnityEngine.EventSystems;

public class bl_IconItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {

    [Separator("SETTINGS")]
    public float DestroyIn = 5f;
    [Separator("REFERENCES")]
    public Image TargetGraphic;
    [SerializeField]private RectTransform CircleAreaRect = null;
    public Sprite DeathIcon = null;
    public GameObject textAreaBackground;
    [SerializeField]private Text InfoText = null;
    public CanvasGroup m_CanvasGroup;
    public CanvasGroup infoAlpha;

    private Animator Anim;
    private float delay = 0.1f;
    private bl_MaskHelper MaskHelper = null;
    private bl_MiniMapItem miniMapItem;
    private bool isTextOpen = false;
    public RectTransform textRect { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        //Get the canvas group or add one if nt have.
        if(m_CanvasGroup == null)
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }
        if(GetComponent<Animator>() != null)
        {
            Anim = GetComponent<Animator>();
        }
        if(Anim != null) { Anim.enabled = false; }
        if (textAreaBackground != null) { textRect = textAreaBackground.GetComponent<RectTransform>(); textAreaBackground.SetActive(false); }
        m_CanvasGroup.alpha = 0;
        if(CircleAreaRect != null) { CircleAreaRect.gameObject.SetActive(false); }
    }

    public void SetUp(bl_MiniMapItem item)
    {
        miniMapItem = item;
        m_CanvasGroup.interactable = miniMapItem.isInteractable;
    }

    /// <summary>
    /// When player or the target die,desactive,remove,etc..
    /// call this for remove the item UI from Map
    /// for change to other icon and desactive in certain time
    /// or destroy immediate
    /// </summary>
    /// <param name="inmediate"></param>
    public void DestroyIcon(bool inmediate)
    {
        if (inmediate)
        {
            Destroy(gameObject);
        }
        else
        {
            //Change the sprite to icon death
            TargetGraphic.sprite = DeathIcon;
            //destroy in 5 seconds
            Destroy(gameObject, DestroyIn);
        }
    }
    /// <summary>
    /// When player or the target die,desactive,remove,etc..
    /// call this for remove the item UI from Map
    /// for change to other icon and desactive in certain time
    /// or destroy immediate
    /// </summary>
    /// <param name="inmediate"></param>
    /// <param name="death"></param>
    public void DestroyIcon(bool inmediate,Sprite death)
    {
        if (inmediate)
        {
            Destroy(gameObject);
        }
        else
        {
            //Change the sprite to icon death
            TargetGraphic.sprite = death;
            //destroy in 5 seconds
            Destroy(gameObject, DestroyIn);
        }
    }
    /// <summary>
    /// Get info to display
    /// </summary>
    /// <param name="info"></param>
    public void SetText(string info)
    {
        if (InfoText == null)
            return;

        InfoText.text = info;
        if(textAreaBackground != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(textRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(InfoText.GetComponent<RectTransform>());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ico"></param>
    public void SetIcon(Sprite ico)
    {
        TargetGraphic.sprite = ico;
    }

    /// <summary>
    /// Show a visible circle area in the minimap with this
    /// item as center
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="AreaColor"></param>
    public RectTransform SetCircleArea(float radius,Color AreaColor)
    {
        if(CircleAreaRect == null) { return null; }

        MaskHelper = transform.root.GetComponentInChildren<bl_MaskHelper>();
        MaskHelper.SetMaskedIcon(CircleAreaRect);
        radius = radius * 10;
        radius = radius * bl_MiniMapUtils.GetMiniMap().IconMultiplier;
        Vector2 r = new Vector2(radius, radius);
        CircleAreaRect.sizeDelta = r;
        CircleAreaRect.GetComponent<Image>().CrossFadeColor(AreaColor, 1, true, true);
        CircleAreaRect.gameObject.SetActive(true);

        return CircleAreaRect;
    }

    /// <summary>
    /// 
    /// </summary>
    public void HideCircleArea()
    {
        CircleAreaRect.SetParent(transform);
        CircleAreaRect.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIcon()
    {
        yield return new WaitForSeconds(delay);
        while(m_CanvasGroup.alpha < 1)
        {
            m_CanvasGroup.alpha += Time.deltaTime * 2;
            yield return null;
        }
        if (Anim != null) { Anim.enabled = true; }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetVisibleAlpha()
    {
        m_CanvasGroup.alpha = 1;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (miniMapItem == null) return;
        if (!miniMapItem.isInteractable || miniMapItem.interacableAction != bl_MiniMapItem.InteracableAction.OnHover) return;
        OnInteract(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (miniMapItem == null) return;
        if (!miniMapItem.isInteractable || miniMapItem.interacableAction != bl_MiniMapItem.InteracableAction.OnTouch) return;
        OnInteract();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (miniMapItem == null) return;
        if (!miniMapItem.isInteractable || miniMapItem.interacableAction != bl_MiniMapItem.InteracableAction.OnHover) return;
        OnInteract(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnInteract()
    {
        isTextOpen = !isTextOpen;
        OnInteract(isTextOpen);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnInteract(bool open)
    {
        StopCoroutine("FadeInfo");
        StartCoroutine("FadeInfo", !open);
    }

    /// <summary>
    /// 
    /// </summary>
    IEnumerator FadeInfo(bool fadeOut)
    {
        if (!fadeOut) { textAreaBackground.SetActive(true); }
        float d = 0;
        while (d < 1)
        {
            d += Time.deltaTime * 4;
            if (fadeOut)
            {
                infoAlpha.alpha = Mathf.Lerp(1, 0, d);
            }
            else
            {
                infoAlpha.alpha = Mathf.Lerp(0, 1, d);
            }
            yield return null;
        }
        if (fadeOut) { textAreaBackground.SetActive(false); }
    }

    public void DelayStart(float v) { delay = v; StartCoroutine(FadeIcon()); }
}