using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notify : MonoBehaviour
{
    public bool isFlashing = false;
    public Image bg;

    public void startFlash()
    {
        if (!isFlashing) StartCoroutine(flash());
    }

    public IEnumerator flash()
    {
        isFlashing = true;
        float alphaHolder = bg.color.a;
        bg.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(1);
        bg.color = new Color(1, 1, 1, alphaHolder);
        isFlashing = false;
    }
}
