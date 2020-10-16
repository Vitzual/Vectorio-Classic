using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Player
{

    // Declare variables
    public new Camera camera;
    protected float moveSpeed = 5f;

    protected Vector2 movement;
    protected Vector2 mousePos;

    void Update()
    {
        // Get directional movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Get mouse position
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // Move player
        player.AddForce(movement * moveSpeed);

        // Rotate player
        Vector2 lookDirection = mousePos - player.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        player.rotation = angle;
    }

}
