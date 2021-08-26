using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationHandler : MonoBehaviour
{
    public static bool isEnabled = true;

    // Contains all active enemies in the scene
    [System.Serializable]
    public class ActiveRotators
    {
        // Constructor
        public ActiveRotators(Transform rotator, float speed)
        {
            this.rotator = rotator;
            this.speed = speed;
        }

        // Class variables
        public Transform rotator { get; set; }
        public float speed { get; set; }

    }

    [HideInInspector]
    public List<ActiveRotators> Rotators;

    // Update is called once per frame
    public void Update()
    {
        if (!isEnabled) return;

        for(int i=0; i<Rotators.Count; i++)
        {
            if (Rotators[i].rotator != null)
            {
                Rotators[i].rotator.Rotate(Vector3.forward, Rotators[i].speed * Time.deltaTime);
            }
            else
            {
                Rotators.RemoveAt(i);
                i--;
            }
        }
    }

    public void registerRotator(Transform a, float b)
    {
        Rotators.Add(new ActiveRotators(a, b));
    }
}
