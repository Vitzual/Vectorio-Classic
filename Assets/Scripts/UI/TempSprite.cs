using UnityEngine;

// Ported from private Automa repository

public class TempSprite
{
    /// <summary>
    /// Sets sprite with specified lifetime and fading
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="lifetime"></param>
    /// <param name="fadeSpeed"></param>
    /// <param name="startFade"></param>
    public TempSprite(SpriteRenderer spriteRenderer, Sprite sprite, Color color, float lifetime, float fadeSpeed, float startFade)
    {
        this.spriteRenderer = spriteRenderer;
        SetSprite(sprite, color, lifetime, fadeSpeed, startFade);
    }

    /// <summary>
    /// Sets sprite with specified lifetime and fading
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="lifetime"></param>
    /// <param name="fadeSpeed"></param>
    /// <param name="startFade"></param>
    public void SetSprite(Sprite sprite, Color color, float lifetime, float fadeSpeed, float startFade)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        this.lifetime = lifetime;

        useFading = true;
        this.fadeSpeed = fadeSpeed;
        this.startFade = startFade;
    }

    /// <summary>
    /// Sets sprite with specified lifetime, but no fading.
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="lifetime"></param>
    public TempSprite(SpriteRenderer spriteRenderer, Sprite sprite, Color color, float lifetime)
    {
        this.spriteRenderer = spriteRenderer;
        SetSprite(sprite, color, lifetime);
    }

    /// <summary>
    /// Sets sprite with specified lifetime, but no fading.
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="lifetime"></param>
    public void SetSprite(Sprite sprite, Color color, float lifetime)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        this.lifetime = lifetime;
        useFading = false;
    }

    /// <summary>
    /// Activates the sprite with the given position, size, and rotation
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <param name="rotation"></param>
    public void Activate(Vector2 position, Vector2 size, Quaternion rotation)
    {
        spriteRenderer.transform.position = position;
        spriteRenderer.transform.rotation = rotation;
        spriteRenderer.transform.localScale = size;
        spriteRenderer.gameObject.SetActive(true);
    }

    // Temp sprite variables
    public SpriteRenderer spriteRenderer;
    public float lifetime;
    private bool useFading;
    private float fadeSpeed;
    private float startFade;

    // Updates the sprite and returns true if lifetime over
    public bool Update()
    {
        lifetime -= Time.deltaTime;
        if (useFading)
        {
            if (lifetime <= startFade)
            {
                Color temp = spriteRenderer.color;
                temp.a -= Time.deltaTime * fadeSpeed;
                spriteRenderer.color = temp;
                return spriteRenderer.color.a <= 0f;
            }
            else return false;
        }
        else return lifetime <= 0f;
    }
}