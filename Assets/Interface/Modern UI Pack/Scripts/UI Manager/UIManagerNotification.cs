using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerNotification : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image background;
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
                UpdateNotification();
            }
        }

        void LateUpdate()
        {
            if (Application.isEditor == true && UIManagerAsset != null)
            {
                if (UIManagerAsset.enableDynamicUpdate == true)
                {
                    dynamicUpdateEnabled = true;
                    UpdateNotification();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateNotification()
        {
            try
            {
                background.color = UIManagerAsset.notificationBackgroundColor;
                icon.color = UIManagerAsset.notificationIconColor;
                title.color = UIManagerAsset.notificationTitleColor;
                description.color = UIManagerAsset.notificationDescriptionColor;
                title.font = UIManagerAsset.notificationTitleFont;
                title.fontSize = UIManagerAsset.notificationTitleFontSize;
                description.font = UIManagerAsset.notificationDescriptionFont;
                description.fontSize = UIManagerAsset.notificationDescriptionFontSize;
            }

            catch { }
        }
    }
}