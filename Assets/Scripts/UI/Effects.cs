using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Ported from private Automa repository

public class Effects : MonoBehaviour
{
    // Audio source prefab
    public SpriteRenderer _spritePrefab;
    public static SpriteRenderer spritePrefab;
    [SerializeField]
    public TextMeshPro _textPrefab;
    public static TextMeshPro textPrefab;

    // List of sprites
    public static List<TempSprite> activeSprites;
    public static List<TempSprite> inactiveSprites;

    // List of text
    public static List<TempText> activeTexts;
    public static List<TempText> inactiveTexts;

    // On awake prefabs
    public void Start()
    {
        activeSprites = new List<TempSprite>();
        inactiveSprites = new List<TempSprite>();
        activeTexts = new List<TempText>();
        inactiveTexts = new List<TempText>();
        spritePrefab = _spritePrefab;
        textPrefab = _textPrefab;
    }

    // Update effects
    public void Update()
    {
        // Update active temp sprites
        for (int i = 0; i < activeSprites.Count; i++)
        {
            if (activeSprites[i].Update())
            {
                inactiveSprites.Add(activeSprites[i]);
                activeSprites.RemoveAt(i);
                i--;
            }
        }

        // Update active temp texts
        for (int i = 0; i < activeTexts.Count; i++)
        {
            if (activeTexts[i].Update())
            {
                inactiveTexts.Add(activeTexts[i]);
                activeTexts.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Creates a temporary sprite at the specified location
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="lifetime"></param>
    /// <param name="fadeSpeed"></param>
    /// <param name="startFade"></param>
    public static void CreateTempSprite(Sprite sprite, Color color, Vector2 size, Vector2 position, Quaternion rotation,
        float lifetime, float fadeSpeed = -1, float startFade = 1)
    {
        // Check for pooled sprites
        if (inactiveSprites.Count > 0)
        {
            // Update the sprite
            TempSprite newSprite = inactiveSprites[0];
            if (fadeSpeed > 0) newSprite.SetSprite(sprite, color, lifetime, fadeSpeed, startFade);
            else newSprite.SetSprite(sprite, color, lifetime);
            newSprite.Activate(position, size, rotation);
            activeSprites.Add(newSprite);
            inactiveSprites.RemoveAt(0);
        }
        else
        {
            // Create new temp sprite instance
            SpriteRenderer newRenderer = Instantiate(spritePrefab, position, rotation);
            TempSprite newSprite = new TempSprite(newRenderer, sprite, color, lifetime, fadeSpeed, startFade);
            newSprite.Activate(position, size, rotation);
            activeSprites.Add(newSprite);
        }
    }

    /// <summary>
    /// Creates a temporary text mesh at the specified location
    /// </summary>
    /// <param name="text"></param>
    /// <param name="position"></param>
    /// <param name="color"></param>
    /// <param name="alphaDecrease"></param>
    /// <param name="alphaTime"></param>
    /// <param name="speedDecrease"></param>
    public static void CreateTempText(string text, Vector2 position, Color color, float alphaDecrease = 2f,
        float alphaTime = 0.5f, float speedDecrease = 2f)
    {
        // Check for pooled texts
        if (inactiveTexts.Count > 0)
        {
            // Update the text
            TempText newText = inactiveTexts[0];
            newText.Set(text, position, color, alphaDecrease, alphaTime, speedDecrease);
            activeTexts.Add(newText);
            inactiveTexts.RemoveAt(0);
        }
        else
        {
            // Create new temp text instance
            TextMeshPro newTextMesh = Instantiate(textPrefab, position, Quaternion.identity);
            activeTexts.Add(new TempText(newTextMesh, position, text, color, alphaDecrease, alphaTime, speedDecrease));
        }
    }
}