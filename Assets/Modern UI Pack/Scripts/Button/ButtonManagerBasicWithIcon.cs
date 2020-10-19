using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
    public class ButtonManagerBasicWithIcon : MonoBehaviour, IPointerEnterHandler
    {
        // Content
        public Sprite buttonIcon;
        public string buttonText = "Button";
        public UnityEvent clickEvent;
        public UnityEvent hoverEvent;
        public AudioClip hoverSound;
        public AudioClip clickSound;
        Button buttonVar;

        // Resources
        public Image normalImage;
        public TextMeshProUGUI normalText;
        public AudioSource soundSource;

        // Settings
        public bool useCustomContent = false;
        public bool enableButtonSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;

        void Start()
        {
            if (buttonVar == null)
                buttonVar = gameObject.GetComponent<Button>();

            buttonVar.onClick.AddListener(delegate
            {
                clickEvent.Invoke();
            });

            if (enableButtonSounds == true && useClickSound == true)
            {
                buttonVar.onClick.AddListener(delegate
                {
                    soundSource.PlayOneShot(clickSound);
                });
            }

            if (useCustomContent == false)
                UpdateUI();
        }

        public void UpdateUI()
        {
            normalImage.sprite = buttonIcon;
            normalText.text = buttonText;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useHoverSound == true && buttonVar.interactable == true)
                soundSource.PlayOneShot(hoverSound);

            hoverEvent.Invoke();
        }
    }
}