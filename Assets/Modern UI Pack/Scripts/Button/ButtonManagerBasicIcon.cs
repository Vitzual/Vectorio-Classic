using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.UI.ModernUIPack
{
    public class ButtonManagerBasicIcon : MonoBehaviour, IPointerEnterHandler
    {
        // Content
        public Sprite buttonIcon;
        public UnityEvent clickEvent;
        public UnityEvent hoverEvent;
        public AudioClip hoverSound;
        public AudioClip clickSound;
        Button buttonVar;

        // Resources
        public Image normalIcon;
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
            normalIcon.sprite = buttonIcon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableButtonSounds == true && useHoverSound == true && buttonVar.interactable == true)
                soundSource.PlayOneShot(hoverSound);

            hoverEvent.Invoke();
        }
    }
}