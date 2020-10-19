using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
    public class ContextMenuManager : MonoBehaviour
    {
        // Resources
        [SerializeField]
        public Canvas mainCanvas;
        public GameObject contextButton;
        public GameObject contextContent;
        public Animator contextAnimator;

        // Settings
        [HideInInspector] public bool isContextMenuOn;

        // Bounds
        [Range(-50, 50)] public int vBorderTop = -10;
        [Range(-50, 50)] public int vBorderBottom = 10;
        [Range(-50, 50)] public int hBorderLeft = 15;
        [Range(-50, 50)] public int hBorderRight = -15;

        Vector2 uiPos;
        Vector3 cursorPos;
        Vector3 contentPos = new Vector3(0, 0, 0);
        Vector3 contextVelocity = Vector3.zero;
        RectTransform contextRect;

        void Start()
        {
            if (mainCanvas == null)
                mainCanvas = gameObject.GetComponentInParent<Canvas>();

            if (contextAnimator == null)
                contextAnimator = gameObject.GetComponent<Animator>();

            contextRect = gameObject.GetComponent<RectTransform>();
            contentPos = new Vector3(vBorderTop, hBorderLeft, 0);
            gameObject.transform.SetAsLastSibling();
        }

        public void CheckForBounds()
        {
            if (uiPos.x <= -100)
            {
                contentPos = new Vector3(hBorderLeft, contentPos.y, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(0f, contextContent.GetComponent<RectTransform>().pivot.y);
            }

            if (uiPos.x >= 100)
            {
                contentPos = new Vector3(hBorderRight, contentPos.y, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(1f, contextContent.GetComponent<RectTransform>().pivot.y);
            }

            if (uiPos.y <= -75)
            {
                contentPos = new Vector3(contentPos.x, vBorderBottom, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(contextContent.GetComponent<RectTransform>().pivot.x, 0f);
            }

            if (uiPos.y >= 75)
            {
                contentPos = new Vector3(contentPos.x, vBorderTop, 0);
                contextContent.GetComponent<RectTransform>().pivot = new Vector2(contextContent.GetComponent<RectTransform>().pivot.x, 1f);
            }
        }

        public void SetContextMenuPosition()
        {
            cursorPos = Input.mousePosition;
            uiPos = contextRect.anchoredPosition;
            CheckForBounds();

            if (mainCanvas.renderMode == RenderMode.ScreenSpaceCamera || mainCanvas.renderMode == RenderMode.WorldSpace)
            {
                cursorPos.z = gameObject.transform.position.z;
                contextRect.position = Camera.main.ScreenToWorldPoint(cursorPos);
                contextContent.transform.localPosition = Vector3.SmoothDamp(contextContent.transform.localPosition, contentPos, ref contextVelocity, 0);
            }

            else if (mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                contextRect.position = cursorPos;
                contextContent.transform.position = new Vector3(cursorPos.x + contentPos.x, cursorPos.y + contentPos.y, 0);
            }
        }

        public void CloseOnClick()
        {
            contextAnimator.Play("Menu Out");
            isContextMenuOn = false;
        }
    }
}