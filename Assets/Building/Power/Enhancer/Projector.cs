using UnityEngine;

public class Projector : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Transform rotator;

    private void Start()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7,7), 1 << LayerMask.NameToLayer("Defense"));
        for (int i=0; i<colliders.Length; i++)
        {
            if (colliders[i].name.Contains("Collector"))
            {
                colliders[i].GetComponent<CollectorAI>().increaseAmount(4);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 50f * Time.deltaTime);
    }

    // Kill defense
    public override void DestroyTile()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(5, 5), 1 << LayerMask.NameToLayer("Defense"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name.Contains("Collector"))
            {
                colliders[i].GetComponent<CollectorAI>().decreaseAmount(4);
            }
        }

        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
