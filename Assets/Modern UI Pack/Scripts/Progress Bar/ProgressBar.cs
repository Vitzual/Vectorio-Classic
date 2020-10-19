using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ProgressBar : MonoBehaviour
    {
        // Content
        [Range(0, 100)] public float currentPercent;
        [Range(0, 100)] public int speed;

        // Resources
        public Image loadingBar;
        public TextMeshProUGUI textPercent;

        // Settings
        public bool isOn;
        public bool restart;
        public bool invert;

        void Start()
        {
            if (isOn == false)
            {
                loadingBar.fillAmount = currentPercent / 100;
                textPercent.text = ((int)currentPercent).ToString("F0") + "%";
            }
        }

        void Update()
        {
            if (isOn == true)
            {
                if (currentPercent <= 100 && invert == false)
                    currentPercent += speed * Time.deltaTime;

                else if (currentPercent >= 0 && invert == true)
                    currentPercent -= speed * Time.deltaTime;

                if (currentPercent >= 100 && speed != 0 && restart == true && invert == false)
                    currentPercent = 0;

                else if (currentPercent == 0 && speed != 0 && restart == true && invert == true)
                    currentPercent = 100;

                loadingBar.fillAmount = currentPercent / 100;
                textPercent.text = ((int)currentPercent).ToString("F0") + "%";
            }
        }

        public void UpdateUI()
        {
            loadingBar.fillAmount = currentPercent / 100;
            textPercent.text = ((int)currentPercent).ToString("F0") + "%";
        }
    }
}