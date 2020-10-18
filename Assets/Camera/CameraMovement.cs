using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Declare variables
    protected float moveSpeed = 20f;
    protected new Rigidbody2D camera;

    protected Vector2 movement;
    protected Vector2 mousePos;

    private void Start()
    {
        camera = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get directional movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 50f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 30f;
        }
    }

    private void FixedUpdate()
    {
        // Move player
        camera.MovePosition(camera.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
