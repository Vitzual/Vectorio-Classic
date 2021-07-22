using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
    [ExecuteInEditMode]
    public class UIManagerProgressBarLoop : MonoBehaviour
    {
        [Header("SETTINGS")]
        public UIManager UIManagerAsset;
        public bool hasBackground;
        public bool useRegularBackground;

        [Header("RESOURCES")]
        public Image bar;
        [HideInInspector] public Image background;

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
                UpdateProgressBar();
            }
        }

        void LateUpdate()
        {
            if (UIManagerAsset != null)
            {
                if (Application.isEditor == true && UIManagerAsset != null)
                {
                    dynamicUpdateEnabled = true;
                    UpdateProgressBar();
                }

                else
                    dynamicUpdateEnabled = false;
            }
        }

        void UpdateProgressBar()
        {
            try
            {
                bar.color = UIManagerAsset.progressBarColor;

                if (hasBackground == true)
                {
                    if (useRegularBackground == true)
                        background.color = UIManagerAsset.progressBarBackgroundColor;
                    else
                        background.color = UIManagerAsset.progressBarLoopBackgroundColor;
                }
            }

            catch { }
        }
    }
}