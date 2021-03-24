using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePopup : MonoBehaviour
{
    public CanvasGroup transparency;
    public TextMeshProUGUI amountText;
    public int moveUp;
    public bool isAnim;
    public Vector3 originalPos;

    // Update is called once per frame
    void Update()
    {
        if (isAnim)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.02f);
            moveUp += 1;
            if (moveUp > 100) transparency.alpha -= 0.02f;
            if (transparency.alpha <= 0) isAnim = false;
        }
    }

    public void ResetPopup(string amount)
    {
        amountText.text = amount;
        isAnim = true;
        transform.position = originalPos;
        transparency.alpha = 1f;
        moveUp = 0;
    }

    public void SetPopup(Vector3 ogp)
    {
        isAnim = false;
        originalPos = ogp;
    }
}

