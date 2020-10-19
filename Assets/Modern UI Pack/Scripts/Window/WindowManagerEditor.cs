using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(WindowManager))]
    [System.Serializable]
    public class WindowManagerEditor : Editor
    {
        // Variables
        private WindowManager wmTarget;
        private int currentTab;

        private void OnEnable()
        {
            // Set target
            wmTarget = (WindowManager)target;
        }

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
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Window Top Header"));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Toolbar content
            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Items");
            toolbarTabs[1] = new GUIContent("Settings");

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
            if (GUILayout.Button(new GUIContent("Items", "Items"), customSkin.FindStyle("Toolbar Items")))
                currentTab = 0;

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Toolbar Settings")))
                currentTab = 1;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Property variables
            var windows = serializedObject.FindProperty("windows");
            var currentWindowIndex = serializedObject.FindProperty("currentWindowIndex");
            var windowFadeIn = serializedObject.FindProperty("windowFadeIn");
            var windowFadeOut = serializedObject.FindProperty("windowFadeOut");
            var buttonFadeIn = serializedObject.FindProperty("buttonFadeIn");
            var buttonFadeOut = serializedObject.FindProperty("buttonFadeOut");

            // Draw content depending on tab index
            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(20);
                    GUILayout.Label("CONTENT", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(windows, new GUIContent("Window Items"), true);
                    windows.isExpanded = true;

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);

                    if (GUILayout.Button("+  Add a new window", customSkin.button))
                        wmTarget.AddNewItem();

                    GUILayout.Space(4);
                    break;

                case 1:
                    GUILayout.Space(20);
                    GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Window In Anim"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(windowFadeIn, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Window Out Anim"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(windowFadeOut, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Button In Anim"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(buttonFadeIn, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Button Out Anim"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(buttonFadeOut, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (wmTarget.windows.Count != 0)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Selected Window:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        currentWindowIndex.intValue = EditorGUILayout.IntSlider(currentWindowIndex.intValue, 0, wmTarget.windows.Count - 1);

                        GUILayout.Space(2);

                        EditorGUILayout.LabelField(new GUIContent(wmTarget.windows[currentWindowIndex.intValue].windowName), customSkin.FindStyle("Text"));

                        GUILayout.EndVertical();
                    }

                    else
                        EditorGUILayout.HelpBox("Window List is empty. Create a new item to see more options.", MessageType.Info);

                    GUILayout.Space(4);
                    break;
            }

            // Apply the changes
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif