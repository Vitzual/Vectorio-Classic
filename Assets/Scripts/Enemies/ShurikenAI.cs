using UnityEngine;
using System.Collections;

public class ShurikenAI : EnemyClass
{
    public Transform body;
    
    private void Update()
    {
        body.Rotate(0, 0, -300 * Time.deltaTime);
    }
}
