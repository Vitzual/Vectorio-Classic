using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject buildable;
    

    public void CreateBuildable()
    {
        // Create the new buildable object
        GameObject newBuildable = Instantiate(buildable, new Vector3(0, 0, 0), Quaternion.identity);

        // Set buildable values
    }
}
