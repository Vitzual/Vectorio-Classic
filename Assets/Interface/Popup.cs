using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    public int moveUp;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.001f);
        moveUp += 1;
        if (moveUp > 1000) Destroy(this);
    }

    public void SetPopup(string amount)
    {
        amountText.text = amount;
    }
}
