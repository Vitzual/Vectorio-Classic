using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Declare variables
    protected float moveSpeed = 30f;
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
            moveSpeed = 150f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 50f;
        }
    }

    private void FixedUpdate()
    {
        camera.MovePosition(camera.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
