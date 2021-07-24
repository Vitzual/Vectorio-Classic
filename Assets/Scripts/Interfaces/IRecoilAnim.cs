using UnityEngine;

public interface IRecoilAnim
{
    public bool animationEnabled { get; set; }
    public bool animPlaying { get; set; }
    public bool animRebound { get; set; }
    public int animTracker { get; set; }
    public int animHolder { get; set; }
    public float animMovement { get; set; }
    public void PlayRecoilAnimation(Transform a);
}
