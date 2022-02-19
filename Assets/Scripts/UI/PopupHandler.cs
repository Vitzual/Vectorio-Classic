using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    public static PopupHandler active;

    public Popup popup;
    public List<Popup> activePopups = new List<Popup>();

    public void Awake()
    {
        active = this;
        activePopups = new List<Popup>();
    }

    public void CreatePopup(Vector2 position, Resource.Type type, string amount)
    {
        if (!Settings.resourcePopups || NewSaveSystem.loadGame) return;

        Popup newPopup = Instantiate(popup, position, Quaternion.identity);
        newPopup.SetPopup(amount, type);
        activePopups.Add(newPopup);
    }

    public void Update()
    {
        for(int i = 0; i < activePopups.Count; i++)
        {
            if(activePopups[i].MovePopup())
            {
                Destroy(activePopups[i].gameObject);
                activePopups.RemoveAt(i);
                i--;
            }
        }
    }
}
