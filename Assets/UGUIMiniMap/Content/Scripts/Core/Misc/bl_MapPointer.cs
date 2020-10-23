using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bl_MapPointer : MonoBehaviour
{

    public string PlayerTag = "Player";
    public AudioClip SpawnSound;
    public MeshRenderer m_Render;
    private AudioSource ASource;
    
    private void OnEnable()
    {
        if(SpawnSound != null)
        {
            ASource = GetComponent<AudioSource>();
            ASource.clip = SpawnSound;
            ASource.Play();
        }
    }

    public void SetColor(Color c)
    {
        GetComponent<bl_MiniMapItem>().IconColor = c;
        c.a = 0.25f;
        m_Render.material.SetColor("_TintColor", c);
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == PlayerTag)
        {
            Destroy(gameObject);
        }
    }
}