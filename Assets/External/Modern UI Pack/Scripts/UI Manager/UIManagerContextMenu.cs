using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerContextMenu : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image backgroundImage;

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
                    Debug.Log("No UI Manager found. Assign it manually, otherwise you'll get errors about it.", this);
                }
            }
        }

        void Awake()
        {
            if (dynamicUpdateEnabled == false)
            {
                this.enabled = true;
                UpdateContextMenu();
            }
        }

        void LateUpdate()
        {
            if (Application.isEditor == true && UIManagerAsset != null)
            {
                if (UIManagerAsset.enableDynamicUpdate == true)
                {
                    dynamicUpdateEnabled = true;
                    UpdateContextMenu();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateContextMenu()
        {
            backgroundImage.color = UIManagerAsset.contextBackgroundColor;
        }
    }
}