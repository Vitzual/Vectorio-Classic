using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public static Dictionary<string, Sprite> library = new Dictionary<string, Sprite>();
    public static Sprite emptySprite;
    public static bool generated = false;

    public void Start()
    {
        if (!generated)
            GenerateSprites(); 
    }

    public static void GenerateSprites()
    {
        generated = true;

        CreateDefaultSprite();

        List<Sprite> sprites = Resources.LoadAll("Sprites", typeof(Sprite)).Cast<Sprite>().ToList();

        foreach (Sprite sprite in sprites)
            library.Add(sprite.name, sprite);
    }

    public static Sprite GetSprite(string name)
    {
        if (!generated)
            GenerateSprites();

        library.TryGetValue(name, out Sprite sprite);
        if (sprite == null)
        {
            Debug.LogError("Failed to get sprite with name " + name);
            return emptySprite;
        }
        else return sprite;
    }

    public static void CreateDefaultSprite()
    {
        if (!generated)
            GenerateSprites();

        Texture2D tex = new Texture2D(256, 256);
        tex.alphaIsTransparency = true;

        Color c = Color.red;

        for (int x = 120; x <= 130; x++)
            for (int y = 120; y <= 130; y++)
                tex.SetPixel(x, y, c);

        emptySprite = Sprite.Create(tex, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f), 32f);
    }
}