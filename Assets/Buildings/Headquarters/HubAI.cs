using UnityEngine;

public class HubAI : TileClass
{
    protected Camera main;
    protected bool gameOver;
    [SerializeField] protected Canvas EndScreen;

    // On start, assign weapon variables
    void Start()
    {
        main = Camera.main;
        health = 30;
        maxhp = 30;
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = false;
        GameObject.Find("Camera").GetComponent<CameraScroll>().enabled = false;
        gameOver = true;
    }
}
