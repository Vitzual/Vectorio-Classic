using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UGUIMiniMap;

[CustomEditor(typeof(bl_MiniMap))]
public class bl_MiniMapEditor : Editor
{
    AnimBool GeneralAnim;
    protected static bool ShowGeneral = false;
    AnimBool ZoomAnim;
    protected static bool ShowZoom = false;
    AnimBool RotationAnim;
    protected static bool ShowRotation = false;
    AnimBool GripAnim;
    protected static bool ShowGrip = false;
    AnimBool PositionAnim;
    protected static bool ShowPosition = false;
    AnimBool AnimationsAnim;
    protected static bool ShowAnimation = false;
    AnimBool DragAnim;
    protected static bool ShowDrag = false;
    AnimBool RenderAnim;
    protected static bool ShowRender = false;
    AnimBool ReferencesAnim;
    protected static bool ShowReferences = false;
    AnimBool MarksAnim;
    protected static bool ShowMarks = false;

    private void OnEnable()
    {
        GeneralAnim = new AnimBool(ShowGeneral);
        GeneralAnim.valueChanged.AddListener(Repaint);
        ZoomAnim = new AnimBool(ShowZoom);
        ZoomAnim.valueChanged.AddListener(Repaint);
        RotationAnim = new AnimBool(ShowRotation);
        RotationAnim.valueChanged.AddListener(Repaint);
        GripAnim = new AnimBool(ShowGrip);
        GripAnim.valueChanged.AddListener(Repaint);
        PositionAnim = new AnimBool(ShowPosition);
        PositionAnim.valueChanged.AddListener(Repaint);
        AnimationsAnim = new AnimBool(ShowAnimation);
        AnimationsAnim.valueChanged.AddListener(Repaint);
        DragAnim = new AnimBool(ShowDrag);
        DragAnim.valueChanged.AddListener(Repaint);
        RenderAnim = new AnimBool(ShowRender);
        RenderAnim.valueChanged.AddListener(Repaint);
        ReferencesAnim = new AnimBool(ShowReferences);
        ReferencesAnim.valueChanged.AddListener(Repaint);
        MarksAnim = new AnimBool(ShowMarks);
        MarksAnim.valueChanged.AddListener(Repaint);
    }

    void CheckLayer(bl_MiniMap script)
    {
        string layer = LayerMask.LayerToName(script.MiniMapLayer);
        if (string.IsNullOrEmpty(layer))
        {
            CreateLayer("MiniMap");
            int layerID = LayerMask.NameToLayer("MiniMap");
            script.MiniMapLayer = layerID;
        }
    }

    public override void OnInspectorGUI()
    {
        bl_MiniMap script = (bl_MiniMap)target;
        bool allowSceneObjects = !EditorUtility.IsPersistent(target);
        serializedObject.Update();

        CheckLayer(script);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("window");

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("General Settings", EditorStyles.toolbarPopup)) { ShowGeneral = !ShowGeneral; GeneralAnim.target = ShowGeneral; }
        if (EditorGUILayout.BeginFadeGroup(GeneralAnim.faded))
        {
            script.m_Target = EditorGUILayout.ObjectField("Target", script.m_Target, typeof(GameObject), allowSceneObjects) as GameObject;
            script.LevelName = EditorGUILayout.TextField("Level Name", script.LevelName);
            script.MiniMapLayer = EditorGUILayout.LayerField("MiniMap Layer", script.MiniMapLayer);

            script.ToogleKey = (KeyCode)EditorGUILayout.EnumPopup("Mode Toggle Key", script.ToogleKey);
            script.m_Type = (bl_MiniMap.RenderType)EditorGUILayout.EnumPopup("Render Mode", script.m_Type);
            script.canvasRenderMode = (bl_MiniMap.RenderMode)EditorGUILayout.EnumPopup("Draw Mode", script.canvasRenderMode);
            if (script.canvasRenderMode == bl_MiniMap.RenderMode.Mode2D)
            {
                script.Ortographic2D = EditorGUILayout.ToggleLeft("Orthographic", script.Ortographic2D, EditorStyles.toolbarButton);
            }
            script.m_MapType = (bl_MiniMap.MapType)EditorGUILayout.EnumPopup("Map Mode", script.m_MapType);
            if (script.m_Type == bl_MiniMap.RenderType.Picture)
            {
                script.MapTexture = EditorGUILayout.ObjectField("Map Texture", script.MapTexture, typeof(Texture), allowSceneObjects) as Texture;
                if (script.WorldSpace != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Take ScreenShot", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    {
                        SetupScreenShot();
                    }
                    GUILayout.Space(5);
                    if (GUILayout.Button("Set Bounds", EditorStyles.toolbarButton, GUILayout.Width(75)))
                    {
                        Selection.activeTransform = script.WorldSpace;
                        EditorGUIUtility.PingObject(script.WorldSpace);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            script.isMobile = EditorGUILayout.ToggleLeft("isMobile", script.isMobile, EditorStyles.toolbarButton);
            script.UpdateRate = EditorGUILayout.IntSlider("Update Rate", script.UpdateRate, 1, 10);
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Zoom Settings", EditorStyles.toolbarPopup)) { ShowZoom = !ShowZoom; ZoomAnim.target = ShowZoom; }
        if (EditorGUILayout.BeginFadeGroup(ZoomAnim.faded))
        {
            script.DefaultHeight = EditorGUILayout.Slider("Default Zoom", script.DefaultHeight, script.MinZoom, script.MaxZoom);
            EditorGUILayout.LabelField("Zoom MinMax", EditorStyles.label);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(script.MinZoom.ToString("F2"), EditorStyles.toolbarButton);
            EditorGUILayout.MinMaxSlider(ref script.MinZoom, ref script.MaxZoom, 1, 100);
            GUILayout.Label(script.MaxZoom.ToString("F2"), EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            script.scrollSensitivity = EditorGUILayout.IntSlider("Zoom Scroll Sensitivity", script.scrollSensitivity, 1, 10);
            script.IconMultiplier = EditorGUILayout.Slider("Icon Size Multiplier", script.IconMultiplier, 0.05f, 2);
            script.LerpHeight = EditorGUILayout.Slider("Zoom Speed", script.LerpHeight, 1, 20);
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Position Settings", EditorStyles.toolbarPopup)) { ShowPosition = !ShowPosition; PositionAnim.target = ShowPosition; }
        if (EditorGUILayout.BeginFadeGroup(PositionAnim.faded))
        {
            script.FullMapPosition = EditorGUILayout.Vector3Field("FullScreen Map Position", script.FullMapPosition);
            if (script.canvasRenderMode == bl_MiniMap.RenderMode.Mode3D)
            {
                script.FullMapRotation = EditorGUILayout.Vector3Field("FullScreen Map Rotation", script.FullMapRotation);
            }
            script.FullMapSize = EditorGUILayout.Vector3Field("FullScreen Map Size", script.FullMapSize);
        }
        if (ShowPosition)
        {
            if (GUILayout.Button("Catch Position", EditorStyles.toolbarButton))
            {
                script.GetFullMapSize();
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Rotation Settings", EditorStyles.toolbarPopup)) { ShowRotation = !ShowRotation; RotationAnim.target = ShowRotation; }
        if (EditorGUILayout.BeginFadeGroup(RotationAnim.faded))
        {
            script.useCompassRotation = EditorGUILayout.ToggleLeft("Circle Mode", script.useCompassRotation, EditorStyles.toolbarButton);
            if (script.useCompassRotation)
            {
                script.CompassSize = EditorGUILayout.Slider("Circle Size", script.CompassSize, 25, 500);
            }
            script.DynamicRotation = EditorGUILayout.ToggleLeft("Rotate Map with player", script.DynamicRotation, EditorStyles.toolbarButton);
            if (script.canvasRenderMode == bl_MiniMap.RenderMode.Mode2D)
            {
                script.iconsAlwaysFacingUp = EditorGUILayout.ToggleLeft("Icons Always Facing Up", script.iconsAlwaysFacingUp, EditorStyles.toolbarButton);
            }
            else
            {
                script.iconsAlwaysFacingUp = EditorGUILayout.ToggleLeft("Icons Always Facing Up", script.iconsAlwaysFacingUp, EditorStyles.toolbarButton);
                // script.iconsAlwaysFacingUp = false;
            }
            script.SmoothRotation = EditorGUILayout.ToggleLeft("Smooth Rotation", script.SmoothRotation, EditorStyles.toolbarButton);
            if (script.SmoothRotation) { script.LerpRotation = EditorGUILayout.Slider("Rotation Lerp", script.LerpRotation, 1, 20); }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Grip Settings", EditorStyles.toolbarPopup)) { ShowGrip = !ShowGrip; GripAnim.target = ShowGrip; }
        if (EditorGUILayout.BeginFadeGroup(GripAnim.faded))
        {
            script.ShowAreaGrid = EditorGUILayout.ToggleLeft("Show Grip", script.ShowAreaGrid, EditorStyles.toolbarButton);
            if (script.ShowAreaGrid)
            {
                script.AreasSize = EditorGUILayout.Slider("Row Grip Size", script.AreasSize, 1, 25);
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Map Pointers Settings", EditorStyles.toolbarPopup)) { ShowMarks = !ShowMarks; MarksAnim.target = ShowMarks; }
        if (EditorGUILayout.BeginFadeGroup(MarksAnim.faded))
        {
            script.AllowMapMarks = EditorGUILayout.ToggleLeft("Allow Map Pointers Grip", script.AllowMapMarks, EditorStyles.toolbarButton);
            if (script.AllowMapMarks)
            {
                script.AllowMultipleMarks = EditorGUILayout.ToggleLeft("Allow multiples marks", script.AllowMultipleMarks, EditorStyles.toolbarButton);
                script.MapPointerPrefab = EditorGUILayout.ObjectField("Pointer Prefab", script.MapPointerPrefab, typeof(GameObject), allowSceneObjects) as GameObject;
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Drag Settings", EditorStyles.toolbarPopup)) { ShowDrag = !ShowDrag; DragAnim.target = ShowDrag; }
        if (EditorGUILayout.BeginFadeGroup(DragAnim.faded))
        {
            script.CanDragMiniMap = EditorGUILayout.ToggleLeft("Active Drag MiniMap", script.CanDragMiniMap, EditorStyles.toolbarButton);
            if (script.CanDragMiniMap)
            {
                script.DragOnlyOnFullScreen = EditorGUILayout.ToggleLeft("Only on full screen", script.DragOnlyOnFullScreen, EditorStyles.toolbarButton);
                script.ResetOffSetOnChange = EditorGUILayout.ToggleLeft("Auto reset position", script.ResetOffSetOnChange, EditorStyles.toolbarButton);
                EditorGUILayout.BeginHorizontal();
                Vector2 v = script.DragMovementSpeed;
                v.x = EditorGUILayout.Slider("Horizontal Speed", v.x, 0.01f, 30);
                v.y = EditorGUILayout.Slider("Vertical Speed", v.y, 0.01f, 30);
                script.DragMovementSpeed = v;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                Vector2 v2 = script.MaxOffSetPosition;
                v2.x = EditorGUILayout.Slider("MinMax Horizontal", v2.x, 1, 2000);
                v2.y = EditorGUILayout.Slider("MinMax Vertical", v2.y, 1, 2000);
                script.MaxOffSetPosition = v2;
                EditorGUILayout.EndHorizontal();
                script.DragCursorIcon = EditorGUILayout.ObjectField("Drag cursor image", script.DragCursorIcon, typeof(Texture2D), allowSceneObjects) as Texture2D;
                EditorGUILayout.BeginHorizontal();
                Vector2 v3 = script.HotSpot;
                v3.x = EditorGUILayout.Slider("Cursor X offset", v3.x, 0.01f, 10);
                v3.y = EditorGUILayout.Slider("Cursor Y offset", v3.y, 0.01f, 10);
                script.HotSpot = v3;
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Animations Settings", EditorStyles.toolbarPopup)) { ShowAnimation = !ShowAnimation; AnimationsAnim.target = ShowAnimation; }
        if (EditorGUILayout.BeginFadeGroup(AnimationsAnim.faded))
        {
            script.ShowLevelName = EditorGUILayout.ToggleLeft("Show Level Name", script.ShowLevelName, EditorStyles.toolbarButton);
            script.ShowPanelInfo = EditorGUILayout.ToggleLeft("Show Panel Info", script.ShowPanelInfo, EditorStyles.toolbarButton);
            script.FadeOnFullScreen = EditorGUILayout.ToggleLeft("Fade on full screen", script.FadeOnFullScreen, EditorStyles.toolbarButton);
            script.LerpTransition = EditorGUILayout.Slider("Full screen transition speed", script.LerpTransition, 1, 20);
            script.HitEffectSpeed = EditorGUILayout.Slider("Damage effect speed", script.HitEffectSpeed, 0.1f, 5);
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("Render Settings", EditorStyles.toolbarPopup)) { ShowRender = !ShowRender; RenderAnim.target = ShowRender; }
        if (EditorGUILayout.BeginFadeGroup(RenderAnim.faded))
        {
            script.PlayerIconSprite = EditorGUILayout.ObjectField("Player Icon", script.PlayerIconSprite, typeof(Sprite), false) as Sprite;
            script.playerColor = EditorGUILayout.ColorField("Player Color", script.playerColor);
            script.TintColor = EditorGUILayout.ColorField("Tint Color", script.TintColor);
            script.SpecularColor = EditorGUILayout.ColorField("Specular Color", script.SpecularColor);
            script.EmessiveColor = EditorGUILayout.ColorField("Emission Color", script.EmessiveColor);
            script.EmissionAmount = EditorGUILayout.Slider("Emission Amount", script.EmissionAmount, 0.1f, 5);
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        if (GUILayout.Button("References", EditorStyles.toolbarPopup)) { ShowReferences = !ShowReferences; ReferencesAnim.target = ShowReferences; }
        if (EditorGUILayout.BeginFadeGroup(ReferencesAnim.faded))
        {
            script.MMCamera = EditorGUILayout.ObjectField("Mini Map Camera", script.MMCamera, typeof(Camera), allowSceneObjects) as Camera;
            script.BottonAnimator = EditorGUILayout.ObjectField("Bottom Animator", script.BottonAnimator, typeof(Animator), allowSceneObjects) as Animator;
            script.PanelInfoAnimator = EditorGUILayout.ObjectField("Panel Animator", script.PanelInfoAnimator, typeof(Animator), allowSceneObjects) as Animator;
            script.HitEffectAnimator = EditorGUILayout.ObjectField("Damage Animator", script.HitEffectAnimator, typeof(Animator), allowSceneObjects) as Animator;
            script.MapPlane = EditorGUILayout.ObjectField("Map Plane", script.MapPlane, typeof(GameObject), allowSceneObjects) as GameObject;
            script.AreaPrefab = EditorGUILayout.ObjectField("Area Prefab", script.AreaPrefab, typeof(GameObject), allowSceneObjects) as GameObject;

            script.HoofdPuntPrefab = EditorGUILayout.ObjectField("Hoof Punt", script.HoofdPuntPrefab, typeof(GameObject), allowSceneObjects) as GameObject;
            script.ItemPrefabSimple = EditorGUILayout.ObjectField("Icon Simple Prefab", script.ItemPrefabSimple, typeof(GameObject), allowSceneObjects) as GameObject;
            script.RootAlpha = EditorGUILayout.ObjectField("Root Alpha", script.RootAlpha, typeof(CanvasGroup), allowSceneObjects) as CanvasGroup;

            script.WorldSpace = EditorGUILayout.ObjectField("Map Bounds Reference", script.WorldSpace, typeof(RectTransform), allowSceneObjects) as RectTransform;
            script.m_Canvas = EditorGUILayout.ObjectField("Canvas", script.m_Canvas, typeof(Canvas), allowSceneObjects) as Canvas;
            script.MiniMapUIRoot = EditorGUILayout.ObjectField("UI Root", script.MiniMapUIRoot, typeof(RectTransform), allowSceneObjects) as RectTransform;
            script.IconsParent = EditorGUILayout.ObjectField("Icons Parent", script.IconsParent, typeof(RectTransform), allowSceneObjects) as RectTransform;
            script.PlayerIcon = EditorGUILayout.ObjectField("Player Icon", script.PlayerIcon, typeof(Image), allowSceneObjects) as Image;
            script.ReferenceMat = EditorGUILayout.ObjectField("Map Material", script.ReferenceMat, typeof(Material), allowSceneObjects) as Material;
            script.AreaMaterial = EditorGUILayout.ObjectField("Grip Material", script.AreaMaterial, typeof(Material), allowSceneObjects) as Material;
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
        if (GUI.changed)
            EditorUtility.SetDirty(script);
    }

    public void CreateLayer(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new System.ArgumentNullException("name", "New layer name string is either null or empty.");

        var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        var layerProps = tagManager.FindProperty("layers");
        var propCount = layerProps.arraySize;

        SerializedProperty firstEmptyProp = null;

        for (var i = 0; i < propCount; i++)
        {
            var layerProp = layerProps.GetArrayElementAtIndex(i);

            var stringValue = layerProp.stringValue;

            if (stringValue == name) return;

            if (i < 8 || stringValue != string.Empty) continue;

            if (firstEmptyProp == null)
                firstEmptyProp = layerProp;
        }

        if (firstEmptyProp == null)
        {
            UnityEngine.Debug.LogError("Maximum limit of " + propCount + " layers exceeded. Layer \"" + name + "\" not created.");
            return;
        }

        firstEmptyProp.stringValue = name;
        tagManager.ApplyModifiedProperties();
    }

    void SetupScreenShot()
    {
        GameObject g = PrefabUtility.InstantiatePrefab(bl_MiniMapData.Instance.ScreenShotPrefab, EditorSceneManager.GetActiveScene()) as GameObject;
        g.GetComponent<bl_MiniMapScreenShot>().SetMiniMap((bl_MiniMap)target);
        Selection.activeGameObject = g;
        EditorGUIUtility.PingObject(g);
        g.transform.SetAsLastSibling();
    }
}