using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    public static Sprites active;

    public Sprite gold;
    public Sprite essence;
    public Sprite iridium;
    public Sprite power;
    public Sprite heat;
    public Sprite health;
    public Sprite damage;
    public Sprite range;
    public Sprite rotationSpeed;
    public Sprite fireRate;
    public Sprite bulletPierces;
    public Sprite bulletAmount;
    public Sprite bulletSpeed;
    public Sprite bulletSpread;
    public Sprite blank;

    public void Start()
    {
        active = this;
    }

    public Sprite GetByEnum(Resource.Currency resource)
    {
        switch (resource)
        {
            case Resource.Currency.Gold:
                return gold;
            case Resource.Currency.Essence:
                return essence;
            case Resource.Currency.Iridium:
                return iridium;
            case Resource.Currency.Power:
                return power;
            case Resource.Currency.Heat:
                return heat;
            default:
                return blank;
        }
    }
}
