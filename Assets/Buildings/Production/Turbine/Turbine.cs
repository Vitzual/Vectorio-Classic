using UnityEngine;

public class Turbine : TileClass
{
    public Transform rotator; 

    public void Start()
    {
        GameObject.Find("Survival").GetComponent<Survival>().increaseAvailablePower(100);
    }

    // Update is called once per frame
    void Update()
    {
        rotator.Rotate(Vector3.forward, 150f * Time.deltaTime);
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().increaseAvailablePower(-100);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
