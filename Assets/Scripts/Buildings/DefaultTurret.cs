using System.Collections.Generic;
using UnityEngine;

[HideInInspector]
public class DefaultTurret : DefaultBuilding, IAudible
{
    // IAudible interface variables
    public AudioClip sound { get; set; }

    // Barrel thing
    public Barrel barrel;

    // Sets stats and registers itself under the turret handler
    public void Start()
    {
        Events.active.TurretPlaced(barrel);
        SetStats();
    }

    // IAudible sound method
    public void PlaySound()
    {
        float audioScale = CameraScroll.getZoom() / 1400f;
        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, Settings.soundVolume - audioScale);
    }
}
