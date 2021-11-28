using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReceiver : MonoBehaviour
{
    // Sound
    private AudioSource audioSource;

    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Events.active.onRefreshSound += UpdateSound;
    }

    // Update is called once per frame
    void UpdateSound()
    {
        audioSource.volume = Settings.music;
    }
}
