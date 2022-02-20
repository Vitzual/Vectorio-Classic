using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Syncs all instantions 
public class Server : NetworkBehaviour
{
    // All active entities in scene
    public static Dictionary<int, BaseEntity> entities = new Dictionary<int, BaseEntity>();

    // Awake
    public void Awake()
    {
        entities = new Dictionary<int, BaseEntity>();
    }

    // Generates a runtime ID 
    public static int GenerateRuntimeID()
    {
        int maxLoop = 100;
        while (maxLoop != 0)
        {
            maxLoop -= 1;
            int genID = Random.Range(0, 99999999);
            if (!entities.ContainsKey(genID))
                return genID;
        }
        return -1;
    }

    // Assigns a unique runtime ID to an entity    
    public static void AssignRuntimeID(BaseEntity entity)
    {
        int genID = GenerateRuntimeID();
        entities.Add(genID, entity);
        entity.runtimeID = genID;
    }
}
