using UnityEngine;
using UnityEditor;

public class bl_MiniMapHelperEditor : MonoBehaviour {

	[MenuItem("GameObject/UI/MiniMap/Item")]
    public static void AddItem()
    {
        GameObject go = Selection.activeGameObject;
        bl_MiniMapItem m = go.AddComponent<bl_MiniMapItem>();
        m.Target = go.transform;
    }
}