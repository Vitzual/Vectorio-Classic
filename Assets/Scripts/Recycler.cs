using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class gradually recycles objects that are requesting to be destroyed.
// This is useful to optimize large amounts of objects being destroyed

public class Recycler : MonoBehaviour
{
    private static List<Transform> recyclables;
    private static bool acceptingRecyclables;

    public void Start()
    {
        recyclables = new List<Transform>();

        if (this != null)
            acceptingRecyclables = true;
        else
            acceptingRecyclables = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (recyclables.Count > 0)
        {
            if (recyclables[0] != null)
                Destroy(recyclables[0].gameObject);
            recyclables.RemoveAt(0);
        }
    }

    public static void AddRecyclable(Transform recyclable)
    {
        recyclable.gameObject.SetActive(false);

        if (acceptingRecyclables)
            recyclables.Add(recyclable);
        else
            Destroy(recyclable.gameObject);
    }
}