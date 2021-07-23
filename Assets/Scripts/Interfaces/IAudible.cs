// IAudible Interface
// Apply to entities that should play a sound

using UnityEngine;

public interface IAudible
{
    AudioClip sound { get; set; }
    void PlaySound();
}
