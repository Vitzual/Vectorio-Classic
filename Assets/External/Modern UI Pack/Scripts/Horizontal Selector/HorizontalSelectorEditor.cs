using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(HorizontalSelector))]
    [System.Serializable]
    public class HorizontalSelectorEditor : Editor
    {
        // Variables
        private HorizontalSelector hsTarget;
        private int currentTab;

        private void OnEnable()
        {
            // Set target
            hsTarget = (HorizontalSelector)target;
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
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("HS Top Header"));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // Toolbar content
            GUIContent[] toolbarTabs = new GUIContent[4];
            toolbarTabs[0] = new GUIContent("Items");
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
            if (GUILayout.Button(new GUIContent("Items", "Items"), customSkin.FindStyle("Toolbar Items")))
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
            var itemList = serializedObject.FindProperty("itemList");
            var selectorEvent = serializedObject.FindProperty("selectorEvent");

            var label = serializedObject.FindProperty("label");
            var labelHelper = serializedObject.FindProperty("labelHelper");
            var indicatorParent = serializedObject.FindProperty("indicatorParent");
            var indicatorObject = serializedObject.FindProperty("indicatorObject");

            var saveValue = serializedObject.FindProperty("saveValue");
            var selectorTag = serializedObject.FindProperty("selectorTag");

            var enableIndicators = serializedObject.FindProperty("enableIndicators");
            var invokeAtStart = serializedObject.FindProperty("invokeAtStart");
            var invertAnimation = serializedObject.FindProperty("invertAnimation");
            var loopSelection = serializedObject.FindProperty("loopSelection");
            var defaultIndex = serializedObject.FindProperty("defaultIndex");

            // Draw content depending on tab index
            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(20);
                    GUILayout.Label("ITEMS", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(itemList, new GUIContent("Selector Items"), true);
                    itemList.isExpanded = true;

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);

                    if (GUILayout.Button("+  Add a new item", customSkin.button))
                        hsTarget.AddNewItem();

                    GUILayout.Space(18);
                    GUILayout.Label("DYNAMIC EVENT", customSkin.FindStyle("Header"));
                    GUILayout.Space(-6);

                    EditorGUILayout.PropertyField(selectorEvent, new GUIContent("Selector Event"), true);

                    GUILayout.Space(4);
                    break;

                case 1:
                    GUILayout.Space(20);
                    GUILayout.Label("RESOURCES", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(label, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label Helper"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(labelHelper, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorParent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorObject, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    break;

                case 2:
                    GUILayout.Space(20);
                    GUILayout.Label("SAVING", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    saveValue.boolValue = GUILayout.Toggle(saveValue.boolValue, new GUIContent("Save Selection"), customSkin.FindStyle("Toggle"));
                    saveValue.boolValue = GUILayout.Toggle(saveValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (saveValue.boolValue == true)
                    {
                        EditorGUI.indentLevel = 2;
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Tag:"), customSkin.FindStyle("Text"), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(selectorTag, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        EditorGUI.indentLevel = 0;
                        EditorGUILayout.HelpBox("Each selector should has its own unique tag.", MessageType.Info);
                    }

                    GUILayout.Space(4);
                    break;

                case 3:
                    GUILayout.Space(20);
                    GUILayout.Label("SETTINGS", customSkin.FindStyle("Header"));
                    GUILayout.Space(2);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableIndicators.boolValue = GUILayout.Toggle(enableIndicators.boolValue, new GUIContent("Enable Indicators"), customSkin.FindStyle("Toggle"));
                    enableIndicators.boolValue = GUILayout.Toggle(enableIndicators.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    if (enableIndicators.boolValue == true)
                    {
                        if (hsTarget.indicatorObject == null)
                            EditorGUILayout.HelpBox("'Indicator Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);

                        if (hsTarget.indicatorParent == null)
                            EditorGUILayout.HelpBox("'Indicator Parent' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);

                        else
                            hsTarget.indicatorParent.gameObject.SetActive(true);
                    }
                    
                    else
                    {
                        if (hsTarget.indicatorParent != null)
                            hsTarget.indicatorParent.gameObject.SetActive(false);
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    invokeAtStart.boolValue = GUILayout.Toggle(invokeAtStart.boolValue, new GUIContent("Invoke At Start"), customSkin.FindStyle("Toggle"));
                    invokeAtStart.boolValue = GUILayout.Toggle(invokeAtStart.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    invertAnimation.boolValue = GUILayout.Toggle(invertAnimation.boolValue, new GUIContent("Invert Animation"), customSkin.FindStyle("Toggle"));
                    invertAnimation.boolValue = GUILayout.Toggle(invertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    loopSelection.boolValue = GUILayout.Toggle(loopSelection.boolValue, new GUIContent("Loop Selection"), customSkin.FindStyle("Toggle"));
                    loopSelection.boolValue = GUILayout.Toggle(loopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Selected Item Index:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    defaultIndex.intValue = EditorGUILayout.IntSlider(defaultIndex.intValue, 0, hsTarget.itemList.Count - 1);

                    GUILayout.Space(2);

                    EditorGUILayout.LabelField(new GUIContent(hsTarget.itemList[defaultIndex.intValue].itemTitle), customSkin.FindStyle("Text"));

                    GUILayout.EndVertical();

                    if (saveValue.boolValue == true)
                    {
                        GUILayout.Space(2);
                        EditorGUILayout.HelpBox("Save Selection is enabled. This option won't be used if there's a stored value.", MessageType.Info);
                    }

                    GUILayout.Space(4);
                    break;
            }

            // Apply the changes
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif