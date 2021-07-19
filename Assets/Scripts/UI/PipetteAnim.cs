using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PipetteAnim : MonoBehaviour
{
    public SpriteRenderer transparency;
    private float increaseAmount = 0.05f;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(transform.localScale.x + increaseAmount, transform.localScale.y + increaseAmount);
        transparency.color = new Color(1f, 1f, 1f, transparency.color.a - 0.012f);
        if (increaseAmount > 0f) increaseAmount -= 0.001f;
        if (transparency.color.a <= 0) Destroy(transform.parent.gameObject);
    }
}
