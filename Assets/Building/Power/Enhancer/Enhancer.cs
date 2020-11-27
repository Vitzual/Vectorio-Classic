using UnityEngine;

public class Enhancer : TileClass
{
    // Internal placement variables
    [SerializeField] private LayerMask TileLayer;
    public Transform rotator;
    public int IncreaseAmount;

    private void Start()
    {
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7,7), 1 << LayerMask.NameToLayer("Defense"));
        for (int i=0; i<colliders.Length; i++)
        {
            if (colliders[i].name.Contains("Collector"))
            {
                colliders[i].GetComponent<CollectorAI>().increaseAmount(IncreaseAmount);
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
                colliders[i].GetComponent<CollectorAI>().decreaseAmount(IncreaseAmount);
            }
        }

        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
