using UnityEngine;

public static class bl_MiniMapUtils  {


    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewPoint"></param>
    /// <param name="maxAnchor"></param>
    /// <returns></returns>
    public static Vector3 CalculateMiniMapPosition(Vector3 viewPoint,RectTransform maxAnchor)
    {
        viewPoint = new Vector2((viewPoint.x * maxAnchor.sizeDelta.x) - (maxAnchor.sizeDelta.x * 0.5f),
            (viewPoint.y * maxAnchor.sizeDelta.y) - (maxAnchor.sizeDelta.y * 0.5f));

        return viewPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bl_MiniMap GetMiniMap(int id = 0)
    {
        bl_MiniMap[] allmm = GameObject.FindObjectsOfType<bl_MiniMap>();
        if (id > allmm.Length - 1) return null;
        return allmm[id];
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static float ClampBorders(float value, float min, float max, out bool isClamped)
    {
        if (value < min)
        {
            value = min;
            isClamped = true;
        }
        else
        {
            if (value > max)
            {
                isClamped = true;
                value = max;
            }
            else
            {
                isClamped = false;
            }
        }
        return value;
    }

    private static Camera _renderCamera = null;
    public static Camera RenderCamera
    {
        get
        {
            if(_renderCamera == null)
            {
                _renderCamera = Camera.main;
                if(_renderCamera == null) { _renderCamera = Camera.current; }
            }
            return _renderCamera;
        }
    }
}