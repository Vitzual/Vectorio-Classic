using UnityEngine;

public class ConveyorAI : MonoBehaviour
{
    protected Animator Animsync;
    protected Animator Animation;

    private void Awake()
    {
        Animsync = GameObject.Find("Animsync").GetComponent<Animator>();
        Animation = GetComponent<Animator>();
    }

    private void Update()
    {
        Animation.Play(0, -1, Animsync.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}
