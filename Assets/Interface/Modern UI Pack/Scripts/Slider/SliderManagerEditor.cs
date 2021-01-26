using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(SliderManager))]
    [System.Serializable]
    public class SliderManagerEditor : Editor
    {
        // Variables
        private SliderManager sliderTarget;
        private int currentTab;

        private void OnEnable()
        {
            // Set target
            sliderTarget = (SliderManager)target;
        }

        public override void OnInspectorGUI()
        {
            // GUI skin variable
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
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Slider Top Header"));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Toolbar content
            GUIContent[] toolbarTabs = new GUIContent[4];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Saving");
            toolbarTabs[3] = new GUIContent("Settings");

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
            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Toolbar Items")))
                currentTab = 0;

            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Toolbar Resources")))
                currentTab = 1;

            if (GUILayout.Button(new GUIContent("Saving", "Saving"), customSkin.FindStyle("Toolbar Saving")))
                currentTab = 2;

            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Toolbar Settings")))
                currentTab = 3;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Property variables
            var sliderEvent = serializedObject.FindProperty("sliderEvent");

            var sliderObject = serializedObject.FindProperty("mainSlider");
            var valueText = serializedObject.FindProperty("valueText");
            var popupValueText = serializedObject.FindProperty("popupValueText");

            var enableSaving = serializedObject.FindProperty("enableSaving");
            var sliderTag = serializedObject.FindProperty("sliderTag");

            var usePercent = serializedObject.FindProperty("usePercent");
            var useRoundValue = serializedObject.FindProperty("useRoundValue");
            var showValue = serializedObject.FindProperty("showValue");
            var showPopupValue = serializedObject.FindProperty("showPopupValue");

            // Draw content depending on tab index
            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(20);
                    GUILayout.Label("CONTENT", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);

                    if (sliderTarget.mainSlider != null)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Current Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        sliderTarget.mainSlider.value = EditorGUILayout.Slider(sliderTarget.mainSlider.value, sliderTarget.mainSlider.minValue, sliderTarget.mainSlider.maxValue);

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Min Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        sliderTarget.mainSlider.minValue = EditorGUILayout.FloatField(sliderTarget.mainSlider.minValue);

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Max Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        sliderTarget.mainSlider.maxValue = EditorGUILayout.FloatField(sliderTarget.mainSlider.maxValue);

                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        sliderTarget.mainSlider.wholeNumbers = GUILayout.Toggle(sliderTarget.mainSlider.wholeNumbers, new GUIContent("Use Whole Numbers"), customSkin.FindStyle("Toggle"));
                        sliderTarget.mainSlider.wholeNumbers = GUILayout.Toggle(sliderTarget.mainSlider.wholeNumbers, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                        GUILayout.EndHorizontal();
                    }

                    else
                        EditorGUILayout.HelpBox("'Main Slider' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);

                    GUILayout.Space(18);
                    GUILayout.Label("EVENTS", customSkin.FindStyle("Header"));
                    GUILayout.Space(-6);
                    EditorGUILayout.PropertyField(sliderEvent, new GUIContent("On Value Changed"), true);
                    GUILayout.Space(4);
                    break;

                case 1:
                    GUILayout.Space(20);
                    GUILayout.Label("RESOURCES", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Slider Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(sliderObject, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    if (showValue.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Label Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(valueText, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    if (showPopupValue.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Popup Label Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(popupValueText, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(4);
                    break;

                case 2:
                    GUILayout.Space(20);
                    GUILayout.Label("SAVING", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableSaving.boolValue = GUILayout.Toggle(enableSaving.boolValue, new GUIContent("Save Value"), customSkin.FindStyle("Toggle"));
                    enableSaving.boolValue = GUILayout.Toggle(enableSaving.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableSaving.boolValue == true)
                    {
                        EditorGUI.indentLevel = 2;
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Tag:"), customSkin.FindStyle("Text"), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(sliderTag, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        EditorGUI.indentLevel = 0;
                        GUILayout.Space(2);
                        EditorGUILayout.HelpBox("Each slider should has its own unique tag.", MessageType.Info);
                    }

                    GUILayout.Space(4);
                    break;

                case 3:
                    GUILayout.Space(20);
                    GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    usePercent.boolValue = GUILayout.Toggle(usePercent.boolValue, new GUIContent("Use Percent"), customSkin.FindStyle("Toggle"));
                    usePercent.boolValue = GUILayout.Toggle(usePercent.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    showValue.boolValue = GUILayout.Toggle(showValue.boolValue, new GUIContent("Show Label"), customSkin.FindStyle("Toggle"));
                    showValue.boolValue = GUILayout.Toggle(showValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    showPopupValue.boolValue = GUILayout.Toggle(showPopupValue.boolValue, new GUIContent("Show Popup Label"), customSkin.FindStyle("Toggle"));
                    showPopupValue.boolValue = GUILayout.Toggle(showPopupValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    useRoundValue.boolValue = GUILayout.Toggle(useRoundValue.boolValue, new GUIContent("Use Round Value"), customSkin.FindStyle("Toggle"));
                    useRoundValue.boolValue = GUILayout.Toggle(useRoundValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    break;
            }

            // Apply the changes
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif