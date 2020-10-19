using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerTooltip : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image background;
        public TextMeshProUGUI text;

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
                UpdateTooltip();
            }
        }

        void LateUpdate()
        {
            if (UIManagerAsset != null)
            {
                if (Application.isEditor == true && UIManagerAsset != null)
                {
                    dynamicUpdateEnabled = true;
                    UpdateTooltip();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateTooltip()
        {
            try
            {
                background.color = UIManagerAsset.tooltipBackgroundColor;
                text.color = UIManagerAsset.tooltipTextColor;
                text.font = UIManagerAsset.tooltipFont;
                text.fontSize = UIManagerAsset.tooltipFontSize;
            }

            catch { }
        }
    }
}