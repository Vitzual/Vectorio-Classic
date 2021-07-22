using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(RadialSlider))]
    [System.Serializable]
    public class RadialSliderEditor : Editor
    {
        // Variables
        private RadialSlider rsTarget;
        private int currentTab;

        private void OnEnable()
        {
            // Set target
            rsTarget = (RadialSlider)target;
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
            var currentValue = serializedObject.FindProperty("currentValue");
            var onValueChanged = serializedObject.FindProperty("onValueChanged");
            var onPointerEnter = serializedObject.FindProperty("onPointerEnter");
            var onPointerExit = serializedObject.FindProperty("onPointerExit");

            var sliderImage = serializedObject.FindProperty("sliderImage");
            var indicatorPivot = serializedObject.FindProperty("indicatorPivot");
            var valueText = serializedObject.FindProperty("valueText");

            var rememberValue = serializedObject.FindProperty("rememberValue");
            var sliderTag = serializedObject.FindProperty("sliderTag");

            var maxValue = serializedObject.FindProperty("maxValue");
            var isPercent = serializedObject.FindProperty("isPercent");
            var decimals = serializedObject.FindProperty("decimals");

            // Draw content depending on tab index
            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(20);
                    GUILayout.Label("CONTENT", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    currentValue.floatValue = EditorGUILayout.Slider(currentValue.floatValue, 0, rsTarget.maxValue);

                    GUILayout.EndHorizontal();

                    if(rsTarget.sliderImage != null && rsTarget.indicatorPivot != null && rsTarget.valueText != null)
                    {
                        rsTarget.SliderValueRaw = currentValue.floatValue;
                        float normalizedAngle = rsTarget.SliderAngle / 360.0f;
                        rsTarget.indicatorPivot.transform.localEulerAngles = new Vector3(180.0f, 0.0f, rsTarget.SliderAngle);
                        rsTarget.sliderImage.fillAmount = normalizedAngle;
                        rsTarget.valueText.text = string.Format("{0}{1}", currentValue.floatValue, rsTarget.isPercent ? "%" : "");
                    }

                    GUILayout.Space(18);
                    GUILayout.Label("EVENTS", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);

                    EditorGUILayout.PropertyField(onValueChanged, new GUIContent("On Value Changed"), true);
                    EditorGUILayout.PropertyField(onPointerEnter, new GUIContent("On Pointer Enter"), true);
                    EditorGUILayout.PropertyField(onPointerExit, new GUIContent("On Pointer Exit"), true);

                    GUILayout.Space(4);
                    break;

                case 1:
                    GUILayout.Space(20);
                    GUILayout.Label("RESOURCES", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Slider Image"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(sliderImage, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Pivot"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorPivot, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Text"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(valueText, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    break;

                case 2:
                    GUILayout.Space(20);
                    GUILayout.Label("SAVING", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    rememberValue.boolValue = GUILayout.Toggle(rememberValue.boolValue, new GUIContent("Save Value"), customSkin.FindStyle("Toggle"));
                    rememberValue.boolValue = GUILayout.Toggle(rememberValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (rememberValue.boolValue == true)
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

                    EditorGUILayout.LabelField(new GUIContent("Max Value"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(maxValue, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Decimals"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(decimals, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    isPercent.boolValue = GUILayout.Toggle(isPercent.boolValue, new GUIContent("Is Percent"), customSkin.FindStyle("Toggle"));
                    isPercent.boolValue = GUILayout.Toggle(isPercent.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

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