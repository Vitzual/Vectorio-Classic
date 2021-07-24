public interface IRecoilAnim
{
    bool animationEnabled { get; set; }
    bool animPlaying { get; set; }
    bool animRebound { get; set; }
    int animTracker { get; set; }
    int animHolder { get; set; }
    float animMovement { get; set; }
    public void playRecoilAnim();
}
