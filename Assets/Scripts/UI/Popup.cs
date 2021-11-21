using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public CanvasGroup transparency;
    public TextMeshProUGUI amountText;
    public Image icon;
    public int moveUp;

    public bool MovePopup()
    {
        moveUp += 1;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.02f);
        if (moveUp > 100) transparency.alpha -= 0.02f;
        if (transparency.alpha <= 0) return true;
        return false;
    }

    public void SetPopup(string amount, Resource.CurrencyType type)
    {
        amountText.text = amount;
        icon.sprite = Resource.active.GetSprite(type);
    }
}
