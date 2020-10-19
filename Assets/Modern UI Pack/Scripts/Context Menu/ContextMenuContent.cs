using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ContextMenuContent : MonoBehaviour, IPointerClickHandler
    {
        [Header("RESOURCES")]
        public ContextMenuManager contextManager;
        public Transform itemParent;

        [Header("ITEMS")]
        public List<ContextItem> contexItems = new List<ContextItem>();

        Animator contextAnimator;
        GameObject selectedItem;
        Image setItemImage;
        TextMeshProUGUI setItemText;
        Sprite imageHelper;
        string textHelper;

        [System.Serializable]
        public class ContextItem
        {
            public string itemText;
            public Sprite itemIcon;
            public ContextItemType contextItemType;
            public UnityEvent onClickEvents;
        }

        public enum ContextItemType
        {
            BUTTON
        }

        void Start()
        {
            if (contextManager == null)
            {
                try
                {
                    contextManager = GameObject.Find("Context Menu").GetComponent<ContextMenuManager>();
                    contextAnimator = contextManager.contextAnimator;
                    itemParent = contextManager.transform.Find("Content/Item List").transform;
                }

                catch
                {
                    Debug.Log("Context Menu - No variable attached to Context Manager.", this);
                }
            } 

            foreach (Transform child in itemParent)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (contextManager.isContextMenuOn == true)
            {
                contextAnimator.Play("Menu Out");
                contextManager.isContextMenuOn = false;
            }

            else if (eventData.button == PointerEventData.InputButton.Right && contextManager.isContextMenuOn == false)
            {
                foreach (Transform child in itemParent)
                {
                    GameObject.Destroy(child.gameObject);
                }

                for (int i = 0; i < contexItems.Count; ++i)
                {
                    if (contexItems[i].contextItemType == ContextItemType.BUTTON)
                        selectedItem = contextManager.contextButton;

                    GameObject go = Instantiate(selectedItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(itemParent, false);

                    setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                    textHelper = contexItems[i].itemText;
                    setItemText.text = textHelper;

                    Transform goImage;
                    goImage = go.gameObject.transform.Find("Icon");
                    setItemImage = goImage.GetComponent<Image>();
                    imageHelper = contexItems[i].itemIcon;
                    setItemImage.sprite = imageHelper;

                    Button itemButton;
                    itemButton = go.GetComponent<Button>();
                    itemButton.onClick.AddListener(contexItems[i].onClickEvents.Invoke);
                    itemButton.onClick.AddListener(CloseOnClick);
                    StartCoroutine(ExecuteAfterTime(0.01f));
                }

                contextManager.SetContextMenuPosition();
                contextAnimator.Play("Menu In");
                contextManager.isContextMenuOn = true;
                contextManager.SetContextMenuPosition();
            }
        }

        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            itemParent.gameObject.SetActive(false);
            itemParent.gameObject.SetActive(true);
            StopCoroutine(ExecuteAfterTime(0.01f));
            StopCoroutine("ExecuteAfterTime");
        }

        public void CloseOnClick()
        {
            contextAnimator.Play("Menu Out");
            contextManager.isContextMenuOn = false;
        }
    }
}