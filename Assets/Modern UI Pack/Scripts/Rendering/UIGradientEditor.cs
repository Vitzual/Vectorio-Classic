using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace UnityEngine.UI
{
    namespace Michsky.UI.ModernUIPack
    {
        [CustomEditor(typeof(UIGradient))]
        [System.Serializable]
        public class UIGradientEditor : Editor
        {
            // Variables
            private int currentTab;

            public override void OnInspectorGUI()
            {
                // GUI skin variables
                GUISkin customSkin;   

                // Select GUI skin depending on the editor theme
                if (EditorGUIUtility.isProSkin == true)
                    customSkin = (GUISkin)Resources.Load("Editor\\Custom Skin Dark");
                else
                    customSkin = (GUISkin)Resources.Load("Editor\\Custom Skin Light");

                GUILayout.Space(-70);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // Top Header
                GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Gradient Top Header"));

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Toolbar content
                GUIContent[] toolbarTabs = new GUIContent[1];
                toolbarTabs[0] = new GUIContent("Settings");

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Space(60);

                currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Toolbar Indicators"));

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Space(50);

                // Draw toolbar tabs as a button
                if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Toolbar Settings")))
                    currentTab = 0;

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // Property variables
                var _effectGradient = serializedObject.FindProperty("_effectGradient");
                var _gradientType = serializedObject.FindProperty("_gradientType");
                var _offset = serializedObject.FindProperty("_offset");

                // Draw content depending on tab index
                switch (currentTab)
                {
                    case 0:
                        GUILayout.Space(20);
                        GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
                        GUILayout.Space(2);
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Gradient"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(_effectGradient, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(_gradientType, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Offset"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(_offset, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Remember that the colors are applied per-vertex, so if you have multiple points on your gradient where the color changes and there aren't enough vertices, you won't see all of the colors.", MessageType.Info);
                        GUILayout.Space(4);
                        break;              
                }

                // Apply the changes
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif