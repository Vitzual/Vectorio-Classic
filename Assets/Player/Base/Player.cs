using UnityEngine;

public class Player : MonoBehaviour
{

    static protected bool BuildMode = false;
    static protected Rigidbody2D player;

    void Start()
    {
        player = this.GetComponent<Rigidbody2D>();
    }
}
