// IDDB (ID Database)
//
// Holds a list of all entity ID's throughout the game
//
// For an object to be usable, it needs to have a registered ID
// ID's get registered at runtime in the building registrar
//
// If an ID is changed, the system will default to null
// 
// For modding, it is important to make sure you choose a unique ID
// You can check the Vectorio IDDB on GitHub to see if an ID is taken

using System.Collections.Generic;
using UnityEngine;

public class IDDB : MonoBehaviour
{
    // Contains a list of all tile stats 
    [System.Serializable]
    public class IDs
    {
        public IDs(string name, int ID, Transform obj) 
        {
            this.name = name;
            this.ID = ID;
            this.obj = obj;
        }

        public string name;
        public int ID;
        public Transform obj;
    }
    public static List<IDs> ids;

    // Request an ID
    public static int RequestID(Transform obj)
    {
        foreach (IDs id in ids)
            if (id.obj == obj)
                return id.ID;
        return -1;
    }

    // Request an Obj
    public static Transform RequestObj(int ID)
    {
        foreach (IDs id in ids)
            if (id.ID == ID)
                return id.obj;
        return null;
    }

    // Create an ID with Obj
    public static void RegisterID(Transform obj, int ID)
    {
        // Remove any duplicate IDs
        foreach (IDs id in ids)
            if (id.ID == ID)
                ids.Remove(id);

        // Add the new ID
        ids.Add(new IDs(obj.name, ID, obj));
        Debug.Log("Registered ID " + ID + " for " + obj.name);
    }
}
