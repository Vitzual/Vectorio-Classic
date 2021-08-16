using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public CanvasGroup transparency;
    public TextMeshProUGUI amountText;
    public Image icon;
    public int moveUp;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.02f);
        moveUp += 1;
        if (moveUp > 100) transparency.alpha -= 0.02f;
        if (transparency.alpha <= 0) Destroy(gameObject);
    }

    public void SetPopup(string amount, string name)
    {
        amountText.text = amount;
        icon.sprite = Currencies.Load<Sprite>("Sprites/" + name);
    }
}
