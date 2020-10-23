using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Tutorial.Wizard.MiniMap
{
    public class MiniMapDocumentation : TutorialWizard
    {
        //required//////////////////////////////////////////////////////
        private const string ImagesFolder = "ugui-minimap/editor/";
        private NetworkImages[] ServerImages = new NetworkImages[]
        {
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-1.jpg", Image = null},
        new NetworkImages{Name = "img-2.jpg", Image = null},
        new NetworkImages{Name = "img-3.jpg", Image = null},
        new NetworkImages{Name = "img-4.jpg", Image = null},
        new NetworkImages{Name = "img-5.jpg", Image = null},
        new NetworkImages{Name = "img-6.jpg", Image = null},
        new NetworkImages{Name = "img-7.jpg", Image = null},
        new NetworkImages{Name = "img-8.jpg", Image = null},
        new NetworkImages{Name = "img-9.jpg", Image = null},
        new NetworkImages{Name = "img-10.jpg", Image = null},
        new NetworkImages{Name = "img-11.jpg", Image = null},
        new NetworkImages{Name = "img-12.jpg", Image = null},
        new NetworkImages{Name = "img-13.jpg", Image = null},
        new NetworkImages{Name = "img-14.jpg", Image = null},
        };
        private Steps[] AllSteps = new Steps[] {
     new Steps { Name = "Get Started", StepsLenght = 1 },
     new Steps { Name = "Add MiniMap", StepsLenght = 1 },
     new Steps { Name = "SetUp MiniMap", StepsLenght = 3 },
     new Steps { Name = "Picture Mode", StepsLenght = 4 },
     new Steps { Name = "Add Icon", StepsLenght = 1 },
     new Steps { Name = "Player Icon", StepsLenght = 1 },
     new Steps { Name = "Icon Text", StepsLenght = 1 },
    };

        [MenuItem("Window/MiniMap/Documentation")]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MiniMapDocumentation));
        }
        //final required////////////////////////////////////////////////

        public override void OnEnable()
        {
            base.OnEnable();
            base.Initizalized(ServerImages, AllSteps, ImagesFolder);
            SetHolderImage(Resources.Load<Texture2D>("place-holder-editor"));
        }

        public override void WindowArea(int window)
        {
            GUI.skin.textArea.richText = true;
            if (window == 0)
            {
                GetStarted();
            }
            else if (window == 1) { AddMiniMap(); }
            else if (window == 2) { SetUpMiniMap(); }
            else if (window == 3) { PictureMode(); }
            else if (window == 4) { AddIcon(); }
            else if (window == 5) { PlayerIcon(); }
            else if (window == 6) { DrawIconText(); }
        }

        bool LayerApply = false;
        void GetStarted()
        {
            DrawText("After import the package in your Unity project you have to add a new <b>Layer</b>, for it if you have not do it yet, simply click in the button bellow.");
            DownArrow();
            if (!LayerExist("MiniMap"))
            {
                if (DrawButton("Add MiniMap Layer"))
                {
                    CreateLayer("MiniMap");
                    LayerApply = true;
                }
            }
            else { LayerApply = true; }

            if (LayerApply)
            {
                Space();
                DrawText("Done!, the layer is set up already, continue with the next step.");
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(EditorGUIUtility.IconContent("Collab").image);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }

        void AddMiniMap()
        {
            DrawText("In order to add a minimap in one of your maps / scenes, you simple have to drag one of the mini maps prefabs from the asset folder to the scene hierarchy" +
                "\n \n-The Mini Maps prefabs are located in <i>UGUIMiniMap -> Content -> Prefabs -> *</i>,\nyou will see various example prefabs, each one have a difference design but the same functionality," +
                "select the one that most fit to your needs and drag it to scene the hierarchy.");
            DrawImage(GetServerImage(0));
            DownArrow();
            DrawText("Now you will see the Mini Map in the upper left corner in the Game View window, now you can start to set up it to you needs (see the next step)");
        }

        bool isSetupReady = false;
        void SetUpMiniMap()
        {
            if (subStep == 0)
            {
                DrawText("Now that you have the minimap in you map, you have to set up a few require settings and if you want many others optionals to personalize it,\n \n" +
                    "First you have to set the <b>Player</b> Object, in order to the minimap follow the player you have to let it know which object is the player in the scene, so simple select the player object and set it in" +
                    "MiniMap -> bl_MiniMap -> General Settings -> <b>Target</b>");
                DrawImage(GetServerImage(1));
                DownArrow();
                DrawText("Now there maybe the case where the player is not in the scene by default and instead it's instanced in runtime like in a multiplayer game for example, you can't assign it in that way because you can't set the player prefab (from the folder)," +
                    "if is this your case then you have to write a few line of code, you have set the player object to bl_MiniMap.cs after this is instanced in the scene, something like this:");
                Space();
                DrawCodeText("bl_MiniMapUtils.GetMiniMap(0).Target = MyPlayerObjectReference;");
                Space();
                DrawText("You can put that code in the same script -> function where you instance the player");
            }
            else if (subStep == 1)
            {
                DrawText("Next thing that you have to set up is the MiniMap Layer, as you may remember in the get started you add a new Layer in the Project Settings, well now you have to let the MiniMap which layer is this," +
                    " simply go to the MiniMap -> bl_MiniMap -> General Settings -> MiniMap Layer -> Select the option called <b>MiniMap</b>, OR you can do try to do it automatically clicking in the button bellow:");
                if (DrawButton("Setup MiniMap Layer"))
                {
                    bl_MiniMap[] mn = FindObjectsOfType<bl_MiniMap>();
                    foreach (bl_MiniMap m in mn)
                    {
                        m.MiniMapLayer = LayerMask.NameToLayer("MiniMap");
                        EditorUtility.SetDirty(m);
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        Debug.Log("MiniMap layer set up!");
                    }
                    isSetupReady = true;
                }
                if (isSetupReady)
                {
                    DownArrow();
                    DrawText("Great!, All require settings are ready, but you have a bunch of optionals settings that you can play with to customize the MiniMap functionality, " +
                        "they will be cover in the next step so you can understand for what are they");
                }
            }
            else if (subStep == 2)
            {
                DrawText("Here I'll explain a little for what or what do each settings in bl_MiniMap.cs so you can understand they and you decide if you want use or not or customize to your needs.");
                Space();
                GUILayout.BeginVertical("General Settings", "window");
                DrawPropertieInfo("LevelName", "string", "used to show a custom map name in the full screen minimap UI");
                DrawPropertieInfo("MiniMapLayer", "LayerMask", "The layer of the minimap, that was set up in the previous step.");
                DrawPropertieInfo("ModeToggleKey", "KeyCode", "The KeyCode with which players can switch the MiniMap to FullScreen Map and vise versa");
                DrawPropertieInfo("Draw Mode", "Enum", "Tell the minimap how to render the map, Realtime: From a Camera or a Picture (Performed)");
                DrawPropertieInfo("Orthographic", "Bool", "use the MiniMap Camera in orthographic mode? useful for 2D games (for RealTime mode only)");
                DrawPropertieInfo("Map Mode", "Enum", "Local: follow the player, Global: static and render the whole map");
                DrawPropertieInfo("Map Texture", "Texture2D", "when use Picture Mode, you have to assign the screen shot of the map");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Zoom Settings", "window");
                DrawPropertieInfo("Default Zoom", "Float", "The default zoom of the MiniMap");
                DrawPropertieInfo("Zoom MinMax", "MinMax", "the minimum and maximum zoom allowed for the minimap");
                DrawPropertieInfo("Zoom Scroll Sensitivity", "Float", "Amount of change zoom with each mouse scroll");
                DrawPropertieInfo("Icon Size Multiplier", "Float", "Multiple the minimap icons size by this value");
                DrawPropertieInfo("Zoom Speed", "Float", "Zoom lerp speed");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Position Settings", "window");
                DrawPropertieInfo("FullScreenMapPosition", "Vector3", "The AnchoredPosition of the MiniMap when is in full screen mode");
                DrawPropertieInfo("FullScreenMapRotation", "Vector3", "The rotation of the minimap when is in full screen mode (and 3D draw mode)");
                DrawPropertieInfo("FullScreen Map Size", "Vector3", "The sizeDelta of the minimap anchor when is in full screen mode");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Rotation Settings", "window");
                DrawPropertieInfo("Circle Mode", "Bool", "is the MiniMap a circle or a square?");
                DrawPropertieInfo("Circle Size", "Float", "The ratio of the circle where icons will rotate when are off screen");
                DrawPropertieInfo("Rotate with player", "Bool", "Rotate the minimap camera with the player or keep a static rotation?");
                DrawPropertieInfo("Always in Front", "Bool", "force minimap icons to rotate and keep they original perspective");
                DrawPropertieInfo("Smooth Rotation", "Bool", "use lerp for rotate the minimap");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Grip Settings", "window");
                DrawPropertieInfo("Show Grip", "Bool", "show a grip pattern over the minimap render?");
                DrawPropertieInfo("Row Grip Size", "Float", "The size of the grip pattern");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Map Pointers Settings", "window");
                DrawPropertieInfo("Allow Map Pointers Grip", "Bool", "Players can create interest points by clicking the minimap?");
                DrawPropertieInfo("Allow multiples marks", "Bool", "Can create multiples interest points or just one per player?");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Drag Settings", "window");
                DrawPropertieInfo("Active Drag MiniMap", "Bool", "Allow player to drag the mini map focus area");
                DrawPropertieInfo("Only on full screen", "Bool", "Allowed only in full screen mode");
                DrawPropertieInfo("Auto reset position", "Bool", "Reset the focus area when switch the minimap size");
                DrawPropertieInfo("Horiz / Vert Speed", "Vector2", "The drag sensitivity");
                DrawPropertieInfo("MinMax Horiz / Vert", "Vector2", "The drag area bounds limits");
                DrawPropertieInfo("Cursor X / Y offset", "Float", "offset the drag cursor image");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Animations Settings", "window");
                DrawPropertieInfo("Show Level Name", "Bool", "Show the custom map name in the minimap UI when is in full screen");
                DrawPropertieInfo("Show Panel Info", "Bool", "Show a panel in the left side of the full screen minimap");
                DrawPropertieInfo("Fade on full screen", "Bool", "Fade effect when switch to full screen minimap");
                DrawPropertieInfo("FullscreenTransitionSpeed", "Float", "Speed of the transition of the minimap when switch size");
                DrawPropertieInfo("Damage effect speed", "Float", "Speed of the red flash effect when call the OnDamage function");
                GUILayout.EndVertical();
                Space();
                GUILayout.BeginVertical("Render Settings", "window");
                DrawPropertieInfo("Player Color", "Color", "The color of the Main Player icon in the minimap UI");
                DrawPropertieInfo("Tint Color", "Color", "Shader tint color of the map render (when use Picture mode)");
                DrawPropertieInfo("Specular Color", "Color", "Shader specular color of the map render (when use Picture mode)");
                DrawPropertieInfo("Emission Color", "Color", "Shader Emission color of the map render (when use Picture mode)");
                DrawPropertieInfo("Emission Amount", "Float", "Amount of the emission (brightness) of the map render (when use Picture mode)");
                GUILayout.EndVertical();
            }
        }

        void PictureMode()
        {
            if (subStep == 0)
            {
                DrawText("You can render the map in the mini map in two modes: <b>RealTime</b> and <b>Picture</b>, for the first as the name say it render the map in real time (from camera), " +
                    "and with the Picture Mode you render just a texture (screen shot) of the map, this last one is much more performed that the real time, it just take 1 draw call, indispensable for mobile" +
                    " platforms, that is the way that almost all games use for render mini maps. \n \n-Now for use Real Time mode you simply need set the option in <i>bl_MiniMap -> General Settings -> Render Mode -> RealTime</i> and you are ready to go," +
                    " but with picture mode you have to do some extra steps.");
                DownArrow();
                DrawText("- First you have to set up the Mini Map to use the Picture mode, so go to the MiniMap in your scene -> bl_MiniMap.cs inspector -> General Settings -> Render Mode -> Select <b>Picture</b>");
                DrawImage(GetServerImage(2));
            }
            else if (subStep == 1)
            {
                DrawText("Now you have setup the map bounds, the visible area, for this you have to scale the <b>Map Boundary</b> transform in the hierarchy, to automatically select this object simply go to" +
                    " bl_MiniMap inspector -> General Settings -> Click on the button in the bottom <b>Set Bounds</b>");
                DrawImage(GetServerImage(3));
                DownArrow();
                DrawText("After click the button in the Scene View you will see a plane gizmo selected with the Rect Tool automatically, you will have something like this:");
                DrawImage(GetServerImage(4));
                DrawText("Now you have to move and scale to fit the map bounds, use the orthographic editor camera mode if is necessary to set exactly the same size and position as the map," +
                    " your result should be something like this for example:");
                DrawImage(GetServerImage(5));
                DownArrow();
                DrawText("<i>Note that your map may not be exactly a square like in the example image, but the logic is the same, set the bounds to the limit of each axis of your map</i>");
            }
            else if (subStep == 2)
            {
                DrawText("Now, you have to take a screen shot of your map bounds, for this I provide an easy tool to do it, " +
                    "go to bl_MiniMap -> General Settings -> Click in the button in the bottom called <b>Take ScreenShot</b>");
                DrawImage(GetServerImage(6));
                DownArrow();
                DrawText("Now the in the Inspector window you will see some setting for take a screen shot, use the Game View for preview how the screen shot looks like," +
                    " you have to make sure that your map bounds match with the Screen bounds, for example you can have this scenario:");
                DrawImage(GetServerImage(7));
                DrawText("You have to center the map in the screen and match the map bounds with the screen bounds, for center the map automatically you can use the button <b>Center Bounds</b> and for match" +
                    " the bounds you can modify the <b>Height</b> slider value, your final result should seen similar to this:");
                DrawImage(GetServerImage(8));
                DownArrow();
                DrawText("When you have it, click in the button <b>Take Screen Shot</b> and select the folder where to save the screen shot");
                DrawImage(GetServerImage(9));
                DownArrow();
                DrawText("<b>Note:</b> if your screen shot have extra margin from your map bounds you can cut it out with any third party program like Photoshop or Gimp");
            }
            else if (subStep == 3)
            {
                DrawText("Now you simply need to assign the screen shot in bl_MiniMap -> General Settings -> <b>Map Texture</b>");
                DrawImage(GetServerImage(10));
                DownArrow();
                DrawText("Finally you have to remove the MiniMap layer from the Culling Mask of your player camera or any other camera in the scene, this in order to player not be able to see the mini map texture in the 3D world, " +
                    "you can do this automatically clicking in the button bellow:");
                if (DrawButton("Setup Cameras"))
                {

                }

            }
        }

        void AddIcon()
        {
            DrawText("In order to show an icon in the minimap over an object in the 3D world, you simply have to attach the script <b>bl_MiniMapItem.cs</b> in the object.\n \n" +
                "so select the object that you want add the icon and add the script bl_MiniMapItem.cs");
            DrawImage(GetServerImage(11));
            DownArrow();
            DrawText("Now you have in the added script you can customize some settings for reach what you want, the only required one is the <b>Icon</b>, bellow I'll describe for what is each or what do each one of the settings.");
            DownArrow();
            DrawPropertieInfo("Target", "Transform", "The object to follow, you can leave this empty and the object where the script is attached will use as Target");
            DrawPropertieInfo("Icon", "Sprite", "The sprite icon for show in the minimap.");
            DrawPropertieInfo("Death Icon", "Sprite", "The sprite icon for show in the minimap when the object get destroyed.");
            DrawPropertieInfo("StartRenderDelay", "Float", "Delay seconds before the icon appear in the minimap after this is instanced");
            DrawPropertieInfo("Icon Color", "Color", "Color of the minimap icon");
            DrawPropertieInfo("Show Circle Area", "Bool", "Show a outline circle around this icon in the minimap");
            DrawPropertieInfo("Radius", "Float", "The Radius of the circle icon");
            DrawPropertieInfo("Border Offset", "Float", "Off set of the minimap border for this icon");
            DrawPropertieInfo("OffScreenIconSize", "Float", "Size of the icon when is off screen / out of the minimap focus area.");
            DrawPropertieInfo("is Interact able", "Bool", "This icon can detect when mouse down over it (like a button) and show a text when this happen?");
            DrawPropertieInfo("Text", "String", "Text to show when click over this icon");
            DrawPropertieInfo("Effect", "Enum", "UI loop effect of this icon in the minimap");
            DrawPropertieInfo("Destroy With Object", "Bool", "Destroy the minimap icon when the object where the script is attached get destroyed.");
            DrawPropertieInfo("is Placeholder", "Bool", "is this a icon created temporally?");
        }

        void PlayerIcon()
        {
            DrawText("As you will noticed the icon for main player is not set up like for other, you don't have to add the bl_MiniMapItem.cs script to the main player, because the icon for this is already instanced," +
                " you only have to add the player in the Target field of bl_MiniMap.cs, so for change the icon for this you can assign it in <b>bl_MiniMap inspector -> Render Settings -> Player Icon.</b>");
            DrawImage(GetServerImage(12));
        }

        void DrawIconText()
        {
            DrawText("If you want to show some information when player hover or touch one of the icons in the minimap like this:");
            DrawImage(GetServerImage(13));
            DownArrow();
            DrawText("in the bl_MiniMapItem.cs inspector <i>(of the minimap icon target)</i> toggle ON the field <b>isInteractable</b>, then set the text to display in the Text Area and select when the text will be displayed (OnHover or OnTouch the icon).");
            DrawImage(GetServerImage(14));
            DownArrow();
            DrawText("if you wanna change the text in runtime, first you have to have a reference of the bl_MiniMapItem that you wanna edit, then just call: miniMapItem.SetIconText(\"My New Text\");");
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

        public static bool LayerExist(string layerName)
        {
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layerProps = tagManager.FindProperty("layers");
            var propCount = layerProps.arraySize;

            for (var i = 0; i < propCount; i++)
            {
                var layerProp = layerProps.GetArrayElementAtIndex(i);
                var stringValue = layerProp.stringValue;
                if (stringValue == layerName) return true;

                if (i < 8 || stringValue != string.Empty) continue;
            }

            return false;
        }

        [MenuItem("Window/MiniMap/Setup Scene Cameras")]
        static void SetupSceneCameras()
        {
            Camera[] all = FindObjectsOfType<Camera>();
            int layerID = LayerMask.NameToLayer("MiniMap");
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i].gameObject.name.Contains("MiniMap")) continue;
                int cm = all[i].cullingMask;
                int ncm = cm & ~(1 << layerID);
                all[i].cullingMask = ncm;
                EditorUtility.SetDirty(all[i]);
                Debug.Log("Camera: " + all[i].gameObject.name + " setup correctly");
            }
        }
    }
}