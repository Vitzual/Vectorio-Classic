using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class TriangleMk2AI : EnemyClass
{
    // Model components
    [SerializeField]
    private ParticleSystem ChargeEffect;
    private Rigidbody2D Triangle;

    // Triangle specific variables
    private bool InRange = false;
    private int ProcRange = 500;
    private int ChargeTime = 2;

    // On start, get rigidbody and assign death effect
    void Start()
    {
        Triangle = this.GetComponent<Rigidbody2D>();
    }

    // Targetting system
    void Update()
    {
        // Find closest enemy 
        if (target == null) {
            target = FindNearestDefence();
        }
        if (target != null)
        {
            float distance = (target.transform.position - this.transform.position).sqrMagnitude;
            // Rotate towards current target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - Triangle.position;

            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Triangle.rotation = angle;
            lookDirection.Normalize();
            Movement = lookDirection;

            // If close, charge up and launch at defense
            if (distance <= ProcRange && InRange == false && SceneManager.GetActiveScene().name != "Menu")
            {
                InRange = true;
                moveSpeed = 0;
                StartCoroutine(SetChargeup(ChargeTime));
            }
        } 
        else
        {
            Movement = new Vector2(0, 0);
        }
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Triangle.AddForce(Movement * moveSpeed);
    }

    // Wait x amount of time
    IEnumerator SetChargeup(int a)
    {
        yield return new WaitForSeconds(a);
        moveSpeed = 200f;
    }
}
