using UnityEngine;

public class ShrinkGrow : MonoBehaviour
{
    public Transform body;
    public float grow = 1f;
    public bool growEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        if (body == null) body = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        body.localScale = new Vector2(grow, grow);
        if (growEnd)
        {
            grow += 0.004f;
            if (grow >= 1f)
                growEnd = false;
        }
        else
        {
            grow -= 0.004f;
            if (grow <= 0.8f)
                growEnd = true;
        }
    }
}
