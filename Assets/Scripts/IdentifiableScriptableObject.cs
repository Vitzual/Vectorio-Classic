using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class IdentifiableScriptableObject : SerializedScriptableObject
{
    private static readonly Dictionary<IdentifiableScriptableObject, string> ObjectToString =
        new Dictionary<IdentifiableScriptableObject, string>();

    private static readonly Dictionary<string, IdentifiableScriptableObject> StringToObject =
        new Dictionary<string, IdentifiableScriptableObject>();

    [SerializeField, ReadOnly]
    private string internalId;

    [SerializeField, ReadOnly]
    private long createdAtTicks;

    [NonSerialized]
    private bool internalIdWasUpdated;

  
    public string InternalID => internalId;

#if UNITY_EDITOR
    private string CreatedAt => new DateTime(createdAtTicks).ToString(CultureInfo.CurrentCulture);
#endif

    protected void OnEnable()
    {
        ProcessRegistration(this);

        if (!internalIdWasUpdated)
            return;

        internalIdWasUpdated = false;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }

    protected void OnDestroy()
    {
        Debug.LogWarning($"Unexpected object destroyed. {internalId}");
        UnregisterObject(this);
        internalId = null;
    }

    public void OnAfterDeserialize()
    {
        base.OnAfterDeserialize();
        ProcessRegistration(this);
    }
    public void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();
        ProcessRegistration(this);
    }

    private static void ProcessRegistration(IdentifiableScriptableObject scriptableObject)
    {
       
        if (ObjectToString.TryGetValue(scriptableObject, out var existingId))
        {
            if (scriptableObject.internalId != existingId)
            {
               
                Debug.LogError($"Inconsistency: {scriptableObject.name} {scriptableObject.internalId} / {existingId}");
                scriptableObject.internalId = existingId;
            }

          
            if (StringToObject.ContainsKey(existingId))
                return;

           
            Debug.LogWarning("Inconsistent database tracking.");
            StringToObject.Add(existingId, scriptableObject);

            return;
        }

        if (string.IsNullOrEmpty(scriptableObject.internalId))
        {
            GenerateInternalId(scriptableObject);
            RegisterObject(scriptableObject);
            return;
        }

       
        if (!StringToObject.TryGetValue(scriptableObject.internalId, out var knownObject))
        {
           
            RegisterObject(scriptableObject);
            return;
        }

       
        if (knownObject == scriptableObject)
        {
         
            Debug.LogWarning("Inconsistent database tracking.");
            ObjectToString.Add(scriptableObject, scriptableObject.internalId);
            return;
        }

      
        if (knownObject == null)
        {

            Debug.LogWarning("Unexpected registration problem.");
            RegisterObject(scriptableObject, true);
            return;
        }

      
        GenerateInternalId(scriptableObject);

        RegisterObject(scriptableObject);
    }

    private static void RegisterObject(IdentifiableScriptableObject aID, bool replace = false)
    {
        if (replace)
        {
            StringToObject[aID.internalId] = aID;
        }
        else
        {
            StringToObject.Add(aID.internalId, aID);
        }

        ObjectToString.Add(aID, aID.internalId);
    }

    private static void UnregisterObject(IdentifiableScriptableObject aID)
    {
        StringToObject.Remove(aID.internalId);
        ObjectToString.Remove(aID);
    }

    private static void GenerateInternalId(IdentifiableScriptableObject obj)
    {
        obj.internalId = Guid.NewGuid().ToString();
        obj.createdAtTicks = DateTime.Now.Ticks;

        obj.internalIdWasUpdated = true;

    }
}
