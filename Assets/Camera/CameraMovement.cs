using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Declare variables
    protected float moveSpeed = 30f;
    protected new Rigidbody2D camera;
    protected Vector2 movement;
    protected Vector2 mousePos;

    // Booleans variables
    bool LegalMovement = true;

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
        LegalMovement = true;

        if (camera.position.x + movement.x > 250) { LegalMovement = false; movement.x = 0; }
        if (camera.position.x + movement.x < -245) { LegalMovement = false; movement.x = 0; }
        if (camera.position.y + movement.y > 245) { LegalMovement = false; movement.y = 0; }
        if (camera.position.y + movement.y < -245) { LegalMovement = false; movement.y = 0; }

        if (LegalMovement) camera.MovePosition(camera.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
