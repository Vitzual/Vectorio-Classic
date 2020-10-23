using UnityEngine;
using UnityEngine.UI;

public class bl_MaskHelper : MonoBehaviour {

    [Header("Mask")]
    public Sprite MiniMapMask = null;
    public Sprite WorldMapMask = null;
    [Header("References")]
    [SerializeField]private Image Background = null;
    [SerializeField]private Sprite MiniMapBackGround = null;
    [SerializeField]private Sprite WorldMapBackGround = null;
    [SerializeField]private RectTransform MaskIconRoot = null;
    public GameObject[] OnFullScreenDisable;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        m_image.sprite = MiniMapMask;
    }


    private Image _image = null;
    private Image m_image
    {
        get
        {
            if (_image == null)
            {
                _image = this.GetComponent<Image>();
            }
            return _image;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="full"></param>
    public void OnChange(bool full = false)
    {
        if (full)
        {
            m_image.sprite = WorldMapMask;
            if (Background != null) { Background.sprite = WorldMapBackGround; }
        }
        else
        {
            m_image.sprite = MiniMapMask;
            if (Background != null) { Background.sprite = MiniMapBackGround; }
        }
        foreach (var item in OnFullScreenDisable)
        {
            item.SetActive(!full);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="trans"></param>
    public void SetMaskedIcon(RectTransform trans)
    {
        trans.SetParent(MaskIconRoot);
    }
}