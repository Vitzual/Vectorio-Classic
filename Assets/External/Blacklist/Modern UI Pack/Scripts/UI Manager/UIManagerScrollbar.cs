using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerScrollbar : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;

        [Header("RESOURCES")]
        public Image background;
        public Image bar;

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
                UpdateScrollbar();
            }
        }

        void LateUpdate()
        {
            if (UIManagerAsset != null)
            {
                if (Application.isEditor == true && UIManagerAsset != null)
                {
                    dynamicUpdateEnabled = true;
                    UpdateScrollbar();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateScrollbar()
        {
            try
            {
                background.color = UIManagerAsset.scrollbarBackgroundColor;
                bar.color = UIManagerAsset.scrollbarColor;
            }

            catch { }
        }
    }
}