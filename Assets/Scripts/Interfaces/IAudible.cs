using UnityEngine;

public interface IAudible
{
    AudioClip sound { get; set; }
    void PlaySound();
}
