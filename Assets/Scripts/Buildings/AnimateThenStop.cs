using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateThenStop : MonoBehaviour
{
    // Enable / disable animation
    public bool animEnabled = true;
    public float animSize = 1.5f;
    protected float animTracker = 0.001f;
    public float animOriginal = 1f;

    // Check if loading
    public void Start()
    {
        enabled = !Gamemode.loadGame;
    }

    // Update is called once per frame
    public void Update()
    {
        if (animEnabled)
            DropInAnim();
        else
            enabled = false; // Disable script when finished
    }

    public void resetAnim()
    {
        animSize = 1.2f;
        animTracker = 0.001f;
        animOriginal = 1f;
    }

    public void DropInAnim()
    {
        transform.localScale = new Vector2(animSize, animSize);
        if (animSize <= animOriginal)
        {
            transform.localScale = new Vector2(animOriginal, animOriginal);
            animEnabled = false;
        }
        else
        {
            animSize -= animTracker;
            animTracker *= 1.2f;
        }
    }
}
