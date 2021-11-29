using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PipetteAnim : MonoBehaviour
{
    private AudioPlayer audioPlayer;
    public SpriteRenderer transparency;
    private float increaseAmount = 0.05f;
    public bool isAnimating = false;

    // Start anim
    public void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();
        Events.active.onPipette += PlayPipette;
    }

    // Play pipette 
    public void PlayPipette(BaseTile tile)
    {
        float size = tile.buildable.building.hologramSize * 2.5f;
        transform.localScale = new Vector2(size, size);
        transform.position = tile.transform.position;
        transparency.color = new Color(1f, 1f, 1f, 1f);
        increaseAmount = 0.05f;
        isAnimating = true;
        audioPlayer.PlayAudio();
    }

    // Update is called once per frame
    public void Update()
    {
        if (isAnimating) 
        {
            transform.localScale = new Vector2(transform.localScale.x + increaseAmount, transform.localScale.y + increaseAmount);
            transparency.color = new Color(1f, 1f, 1f, transparency.color.a - 0.012f);
            if (increaseAmount > 0f) increaseAmount -= 0.001f;
            if (transparency.color.a <= 0)
            {
                isAnimating = false;
                transparency.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
}
