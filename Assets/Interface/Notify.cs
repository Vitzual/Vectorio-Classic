using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Notify : MonoBehaviour
{
    public bool isFlashing = false;
    public Image bg;
    public AudioSource sound;
    public Settings volume;

    public void startFlash()
    {
        if (!isFlashing) StartCoroutine(flash());
    }

    public IEnumerator flash()
    {
        sound.volume = volume.GetSound();
        sound.Play();
        isFlashing = true;
        bg.color = new Color(1, 0, 0, bg.color.a);
        yield return new WaitForSeconds(1);
        bg.color = new Color(1, 1, 1, bg.color.a);
        isFlashing = false;
    }
}
