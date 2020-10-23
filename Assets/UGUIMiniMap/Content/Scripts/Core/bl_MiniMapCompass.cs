using UnityEngine;

public class bl_MiniMapCompass : MonoBehaviour {

    private Transform Target;
    public RectTransform CompassRoot;
    
    public RectTransform North;
    public RectTransform South;
    public RectTransform East;
    public RectTransform West;
    [HideInInspector] public int Grade;

    private int Rotation;
    private bl_MiniMap MiniMap;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        MiniMap = GetComponent<bl_MiniMap>();
        if (Target == null)
        {
            Target = MiniMap.Target;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        //return always positive
        if (Target != null)
        {
            Rotation = (int)Mathf.Abs(Target.eulerAngles.y);
        }
        else
        {
            Target = MiniMap.Target;
            Rotation = (int)Mathf.Abs(m_Transform.eulerAngles.y);
        }
        Rotation = Rotation % 360;//return to 0 


        Grade = Rotation;
        //opposite angle
        if (Grade > 180)
        {
            Grade = Grade - 360;
        }
        float cm = CompassRoot.sizeDelta.x * 0.5f;
        if (MiniMap.useCompassRotation)
        {
            if (bl_MiniMapUtils.RenderCamera == null) return;

            Vector3 north = Vector3.forward * 1000;
            Vector3 tar = bl_MiniMapUtils.RenderCamera.transform.forward;
            tar.y = 0;

            float n = angle3602(north, tar,Target.right);
            Vector3 rot = CompassRoot.eulerAngles;
            rot.z = n;
            CompassRoot.eulerAngles = rot;
        }
        else
        {
            North.anchoredPosition = new Vector2((cm - (Grade * 2) - cm), 0);
            South.anchoredPosition = new Vector2((cm - Rotation * 2 + 360) - cm, 0);
            East.anchoredPosition = new Vector2((cm - Grade * 2 + 180) - cm, 0);
            West.anchoredPosition = new Vector2((cm - Rotation * 2 + 540) - cm, 0);
        }

    }

    float angle3602(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        Vector3 cross = Vector3.Cross(from, to);
        if (cross.y < 0) { angle = -angle;}
        return angle;
    }

    public float Angle360(Vector2 p1, Vector2 p2, Vector2 o = default(Vector2))
    {
        Vector2 v1, v2;
        if (o == default(Vector2))
        {
            v1 = p1.normalized;
            v2 = p2.normalized;
        }
        else
        {
            v1 = (p1 - o).normalized;
            v2 = (p2 - o).normalized;
        }
        float angle = Vector2.Angle(v1, v2);
        return Mathf.Sign(Vector3.Cross(v1, v2).z) < 0 ? (360 - angle) % 360 : angle;
    }

    private Transform t;
    private Transform m_Transform
    {
        get
        {
            if (t == null)
            {
                t = this.GetComponent<Transform>();
            }
            return t;
        }
    }
}