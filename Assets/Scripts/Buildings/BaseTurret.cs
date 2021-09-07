using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class BaseTurret : BaseBuilding, IAudible
{
    // Turret scriptable
    public Turret turret;

    // IAudible interface variables
    public AudioClip sound { get; set; }

    // IAudible sound method
    public void PlaySound()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.soundVolume - audioScale);
    }
}
