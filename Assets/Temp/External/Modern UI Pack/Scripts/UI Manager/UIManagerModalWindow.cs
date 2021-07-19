using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerModalWindow : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image background;
        public Image contentBackground;
        public Image icon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

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
                UpdateModalWindow();
            }
        }

        void LateUpdate()
        {
            if (Application.isEditor == true && UIManagerAsset != null)
            {
                if (UIManagerAsset.enableDynamicUpdate == true)
                {
                    dynamicUpdateEnabled = true;
                    UpdateModalWindow();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateModalWindow()
        {
            try
            {
                background.color = UIManagerAsset.modalWindowBackgroundColor;
                contentBackground.color = UIManagerAsset.modalWindowContentPanelColor;
                icon.color = UIManagerAsset.modalWindowIconColor;
                title.color = UIManagerAsset.modalWindowTitleColor;
                description.color = UIManagerAsset.modalWindowDescriptionColor;
                title.font = UIManagerAsset.modalWindowTitleFont;
                description.font = UIManagerAsset.modalWindowContentFont;
            }

            catch { }
        }
    }
}