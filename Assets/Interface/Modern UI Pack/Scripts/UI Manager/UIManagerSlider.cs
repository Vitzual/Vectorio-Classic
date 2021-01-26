using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerSlider : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;
        public bool hasLabel;
        public bool hasPopupLabel;

        [Header("RESOURCES")]
        public Image background;
        public Image bar;
        public Image handle;
        [HideInInspector] public TextMeshProUGUI label;
        [HideInInspector] public TextMeshProUGUI popupLabel;

        bool dynamicUpdateEnabled;

        void OnEnable()
        {
            if (UIManagerAsset == null)
            {
                try
                {
                    UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
                }

                catch
                {
                    Debug.LogWarning("No UI Manager found. Assign it manually, otherwise you'll get errors about it.", this);
                }
            }
        }

        void Awake()
        {
            if (dynamicUpdateEnabled == false)
            {
                this.enabled = true;
                UpdateSlider();
            }
        }

        void LateUpdate()
        {
            if (UIManagerAsset != null)
            {
                if (Application.isEditor == true && UIManagerAsset != null)
                {
                    dynamicUpdateEnabled = true;
                    UpdateSlider();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateSlider()
        {
            try
            {
                if (UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.BASIC)
                {
                    background.color = UIManagerAsset.sliderBackgroundColor;
                    bar.color = UIManagerAsset.sliderColor;
                    handle.color = UIManagerAsset.sliderColor;

                    if (hasLabel == true)
                    {
                        label.color = new Color(UIManagerAsset.sliderColor.r, UIManagerAsset.sliderColor.g, UIManagerAsset.sliderColor.b, label.color.a);
                        label.font = UIManagerAsset.sliderLabelFont;
                        label.fontSize = UIManagerAsset.sliderLabelFontSize;
                    }

                    if (hasPopupLabel == true)
                    {
                        popupLabel.color = new Color(UIManagerAsset.sliderPopupLabelColor.r, UIManagerAsset.sliderPopupLabelColor.g, UIManagerAsset.sliderPopupLabelColor.b, popupLabel.color.a);
                        popupLabel.font = UIManagerAsset.sliderLabelFont;
                    }
                }

                else if (UIManagerAsset.sliderThemeType == UIManager.SliderThemeType.CUSTOM)
                {
                    background.color = UIManagerAsset.sliderBackgroundColor;
                    bar.color = UIManagerAsset.sliderColor;
                    handle.color = UIManagerAsset.sliderHandleColor;

                    if (hasLabel == true)
                    {
                        label.color = new Color(UIManagerAsset.sliderLabelColor.r, UIManagerAsset.sliderLabelColor.g, UIManagerAsset.sliderLabelColor.b, label.color.a);
                        label.font = UIManagerAsset.sliderLabelFont;
                        label.font = UIManagerAsset.sliderLabelFont;
                    }

                    if (hasPopupLabel == true)
                    {
                        popupLabel.color = new Color(UIManagerAsset.sliderPopupLabelColor.r, UIManagerAsset.sliderPopupLabelColor.g, UIManagerAsset.sliderPopupLabelColor.b, popupLabel.color.a);
                        popupLabel.font = UIManagerAsset.sliderLabelFont;
                    }
                }
            }

            catch { }
        }
    }
}