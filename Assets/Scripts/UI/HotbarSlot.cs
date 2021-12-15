using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;

[System.Serializable]
public class HotbarSlot
{
    [HideInInspector]
    public Entity entity;
    public ButtonManagerBasicIcon button;
    public TextMeshProUGUI resourceUI;

    public void SetSlot(Entity entity, Sprite sprite)
    {
        this.entity = entity;

        if (sprite != null)
        {
            button.buttonIcon = sprite;
            button.UpdateUI();
        }
        else
            Debug.LogError("Sprite with name " + sprite.name + " could not be found!");

        if (resourceUI != null && Buildables.active.ContainsKey(entity))
            resourceUI.text = Resource.FormatNumber(Buildables.active[entity].GetResource(Resource.CurrencyType.Gold));
    }
}
