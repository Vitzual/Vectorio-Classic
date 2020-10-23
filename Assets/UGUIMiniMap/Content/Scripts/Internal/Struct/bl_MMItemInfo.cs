using UnityEngine;

public class bl_MMItemInfo
{
    //Requiered
    //Position where new item was instantiate
    public Vector3 Position;

    //Optionals
    //Leave null if not have target to follow(is static in instantiate position).
    public Transform Target;
    public float Size = 12;
    public Color Color = new Color(1, 1, 1, 0.95f);
    public bool Interactable = false;
    public Sprite Sprite;
    public ItemEffect Effect = ItemEffect.Fade;
    
    public bl_MMItemInfo(Vector3 position)
    {
        Position = position;
    }

    public bl_MMItemInfo(Transform target)
    {
        Target = target;
    }
}