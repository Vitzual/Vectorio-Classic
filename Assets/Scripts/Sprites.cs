using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public Sprite _emptySprite;
    public static Dictionary<string, Sprite> library = new Dictionary<string, Sprite>();
    public static Sprite emptySprite;
    public static bool generated = false;

    public void Start()
    {
        emptySprite = _emptySprite;

        if (!generated)
            GenerateSprites(); 
    }

    public static void GenerateSprites()
    {
        generated = true;

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
}