using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Declare variables
    public float maxRange = 750f;
    protected float moveSpeed = 50f;
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
            moveSpeed = 250f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed = 50f;
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            moveSpeed = 20f;
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            moveSpeed = 50f;
    }

    private void FixedUpdate()
    {
        LegalMovement = true;

        if (camera.position.x + movement.x > maxRange) { LegalMovement = false; movement.x = 0; }
        if (camera.position.x + movement.x < -maxRange) { LegalMovement = false; movement.x = 0; }
        if (camera.position.y + movement.y > maxRange) { LegalMovement = false; movement.y = 0; }
        if (camera.position.y + movement.y < -maxRange) { LegalMovement = false; movement.y = 0; }

        if (LegalMovement) camera.MovePosition(camera.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
