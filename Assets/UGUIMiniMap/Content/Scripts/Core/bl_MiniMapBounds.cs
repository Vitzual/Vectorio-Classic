using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class bl_MiniMapBounds : MonoBehaviour {

    [Header("Use UI editor Tool for scale the wordSpace")]
    public Color GizmoColor = new Color(1, 1, 1, 0.75f);

    /// <summary>
    /// Debuging world space of map
    /// </summary>
    void OnDrawGizmosSelected()
    {
        RectTransform r = this.GetComponent<RectTransform>();
        Vector3 v = r.sizeDelta;
        Vector3 pivot = r.position;

        Gizmos.color = GizmoColor;
        Gizmos.DrawCube(pivot, new Vector3(v.x, 2, v.y));
        Gizmos.DrawWireCube(pivot, new Vector3(v.x, 2, v.y));
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(bl_MiniMapBounds))]
public class bl_MiniMapBoundsEditor : Editor
{
    private Tool beforeTool = Tool.Move;

    private void OnEnable()
    {
        beforeTool = Tools.current;
    }

    private void OnDisable()
    {
        Tools.current = beforeTool;
        Tools.current = Tool.Rect;
    }

    void OnSceneGUI()
    {
        // get the chosen game object
        bl_MiniMapBounds t = target as bl_MiniMapBounds;
        if (t == null)
            return;

        Tools.current = Tool.Rect;
        t.transform.position = Handles.DoPositionHandle(t.transform.position, t.transform.rotation);
    }
}
#endif
