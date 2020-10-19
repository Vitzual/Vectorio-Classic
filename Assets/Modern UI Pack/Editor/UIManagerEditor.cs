using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Presets;
#endif

#if UNITY_EDITOR
namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(UIManager))]
    [System.Serializable]
    public class UIManagerEditor : Editor
    {
        // Variables
        Texture2D muipLogo;

        protected static bool showAnimatedIcon = false;
        protected static bool showButton = false;
        protected static bool showContext = false;
        protected static bool showDropdown = false;
        protected static bool showHorSelector = false;
        protected static bool showInputField = false;
        protected static bool showModalWindow = false;
        protected static bool showNotification = false;
        protected static bool showProgressBar = false;
        protected static bool showScrollbar = false;
        protected static bool showSlider = false;
        protected static bool showSwitch = false;
        protected static bool showToggle = false;
        protected static bool showTooltip = false;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true)
                muipLogo = Resources.Load<Texture2D>("Editor\\MUIP Editor Dark");
            else
                muipLogo = Resources.Load<Texture2D>("Editor\\MUIP Editor Light");
        }

        public override void OnInspectorGUI()
        {
            // GUI skin variables
            GUISkin customSkin;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\Custom Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\Custom Skin Light");

            // Foldout style
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.font = customSkin.font;
            foldoutStyle.fontStyle = FontStyle.Normal;
            foldoutStyle.fontSize = 15;
            foldoutStyle.margin = new RectOffset(12, 55, 6, 6);
            Vector2 contentOffset = foldoutStyle.contentOffset;
            contentOffset.y = -1;
            contentOffset.x = 5;
            foldoutStyle.contentOffset = contentOffset;

            // Top header
            GUILayout.Label(muipLogo, GUILayout.Width(250), GUILayout.Height(40));

            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Animated Icon
            var animatedIconColor = serializedObject.FindProperty("animatedIconColor");
            showAnimatedIcon = EditorGUILayout.Foldout(showAnimatedIcon, "Animated Icon", foldoutStyle);

            if (showAnimatedIcon)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(animatedIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
                                    GUILayout.BeginVertical(EditorStyles.helpBox);

            // Button
            var buttonTheme = serializedObject.FindProperty("buttonThemeType");
            var buttonFont = serializedObject.FindProperty("buttonFont");
            var buttonFontSize = serializedObject.FindProperty("buttonFontSize");
            var buttonBorderColor = serializedObject.FindProperty("buttonBorderColor");
            var buttonFilledColor = serializedObject.FindProperty("buttonFilledColor");
            var buttonTextBasicColor = serializedObject.FindProperty("buttonTextBasicColor");
            var buttonTextColor = serializedObject.FindProperty("buttonTextColor");
            var buttonTextHighlightedColor = serializedObject.FindProperty("buttonTextHighlightedColor");
            var buttonIconBasicColor = serializedObject.FindProperty("buttonIconBasicColor");
            var buttonIconColor = serializedObject.FindProperty("buttonIconColor");
            var buttonIconHighlightedColor = serializedObject.FindProperty("buttonIconHighlightedColor");
            showButton = EditorGUILayout.Foldout(showButton, "Button", foldoutStyle);

            if (showButton && buttonTheme.enumValueIndex == 0)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonTheme, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(buttonFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonBorderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonFilledColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            if (showButton && buttonTheme.enumValueIndex == 1)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonTheme, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(buttonFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonBorderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Filled Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonFilledColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Basic Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonTextBasicColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Hover Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonTextHighlightedColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Basic Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonIconBasicColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Hover Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(buttonIconHighlightedColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Context Menu
            var contextBackgroundColor = serializedObject.FindProperty("contextBackgroundColor");
            showContext = EditorGUILayout.Foldout(showContext, "Context Menu", foldoutStyle);

            if (showContext)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(contextBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("Context Menu is currently in beta, expect major changes in future releases.", MessageType.Info);
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Dropdown
            var dropdownTheme = serializedObject.FindProperty("dropdownThemeType");
            var dropdownAnimationType = serializedObject.FindProperty("dropdownAnimationType");
            var dropdownFont = serializedObject.FindProperty("dropdownFont");
            var dropdownFontSize = serializedObject.FindProperty("dropdownFontSize");
            var dropdownItemFont = serializedObject.FindProperty("dropdownItemFont");
            var dropdownItemFontSize = serializedObject.FindProperty("dropdownItemFontSize");
            var dropdownColor = serializedObject.FindProperty("dropdownColor");
            var dropdownTextColor = serializedObject.FindProperty("dropdownTextColor");
            var dropdownIconColor = serializedObject.FindProperty("dropdownIconColor");
            var dropdownItemColor = serializedObject.FindProperty("dropdownItemColor");
            var dropdownItemTextColor = serializedObject.FindProperty("dropdownItemTextColor");
            var dropdownItemIconColor = serializedObject.FindProperty("dropdownItemIconColor");
            showDropdown = EditorGUILayout.Foldout(showDropdown, "Dropdown", foldoutStyle);

            if (showDropdown && dropdownTheme.enumValueIndex == 0)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownTheme, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Animation Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownAnimationType, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(dropdownFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Item Background"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownItemColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("Item values will be applied at start.", MessageType.Info);
            }

            if (showDropdown && dropdownTheme.enumValueIndex == 1)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownTheme, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Animation Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownAnimationType, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(dropdownFont, new GUIContent(""));

                GUILayout.EndHorizontal();;
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Item Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownItemFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(dropdownItemFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Item Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownItemColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Item Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownItemTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Item Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(dropdownItemIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("Item values will be applied at start.", MessageType.Info);
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Horizontal Selector
            var selectorFont = serializedObject.FindProperty("selectorFont");
            var hSelectorFontSize = serializedObject.FindProperty("hSelectorFontSize");
            var selectorColor = serializedObject.FindProperty("selectorColor");
            var selectorHighlightedColor = serializedObject.FindProperty("selectorHighlightedColor");
            var hSelectorInvertAnimation = serializedObject.FindProperty("hSelectorInvertAnimation");
            var hSelectorLoopSelection = serializedObject.FindProperty("hSelectorLoopSelection");
            showHorSelector = EditorGUILayout.Foldout(showHorSelector, "Horizontal Selector", foldoutStyle);

            if (showHorSelector)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(hSelectorFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(selectorFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(selectorColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Highlighted Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(selectorHighlightedColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Invert Animation"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                hSelectorInvertAnimation.boolValue = GUILayout.Toggle(hSelectorInvertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle"));
                hSelectorInvertAnimation.boolValue = GUILayout.Toggle(hSelectorInvertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Loop Selection"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                hSelectorLoopSelection.boolValue = GUILayout.Toggle(hSelectorLoopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle"));
                hSelectorLoopSelection.boolValue = GUILayout.Toggle(hSelectorLoopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Input Field
            var inputFieldFont = serializedObject.FindProperty("inputFieldFont");
            var inputFieldFontSize = serializedObject.FindProperty("inputFieldFontSize");
            var inputFieldColor = serializedObject.FindProperty("inputFieldColor");
            showInputField = EditorGUILayout.Foldout(showInputField, "Input Field", foldoutStyle);

            if (showInputField)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(inputFieldFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(inputFieldFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(inputFieldColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Modal Window
            var modalWindowTitleFont = serializedObject.FindProperty("modalWindowTitleFont");
            var modalWindowContentFont = serializedObject.FindProperty("modalWindowContentFont");
            var modalWindowTitleColor = serializedObject.FindProperty("modalWindowTitleColor");
            var modalWindowDescriptionColor = serializedObject.FindProperty("modalWindowDescriptionColor");
            var modalWindowIconColor = serializedObject.FindProperty("modalWindowIconColor");
            var modalWindowBackgroundColor = serializedObject.FindProperty("modalWindowBackgroundColor");
            var modalWindowContentPanelColor = serializedObject.FindProperty("modalWindowContentPanelColor");
            showModalWindow = EditorGUILayout.Foldout(showModalWindow, "Modal Window", foldoutStyle);

            if (showModalWindow)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Title Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowTitleFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Content Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowContentFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Title Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowTitleColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Description Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowDescriptionColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Content Panel Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(modalWindowContentPanelColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                EditorGUILayout.HelpBox("These values will only affect 'Style 1 - Standard' window.", MessageType.Info);
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Notification
            var notificationTitleFont = serializedObject.FindProperty("notificationTitleFont");
            var notificationTitleFontSize = serializedObject.FindProperty("notificationTitleFontSize");
            var notificationDescriptionFont = serializedObject.FindProperty("notificationDescriptionFont");
            var notificationDescriptionFontSize = serializedObject.FindProperty("notificationDescriptionFontSize");
            var notificationBackgroundColor = serializedObject.FindProperty("notificationBackgroundColor");
            var notificationTitleColor = serializedObject.FindProperty("notificationTitleColor");
            var notificationDescriptionColor = serializedObject.FindProperty("notificationDescriptionColor");
            var notificationIconColor = serializedObject.FindProperty("notificationIconColor");
            showNotification = EditorGUILayout.Foldout(showNotification, "Notification", foldoutStyle);

            if (showNotification)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Title Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationTitleFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(notificationTitleFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Description Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationDescriptionFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(notificationDescriptionFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Title Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationTitleColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Description Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationDescriptionColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Icon Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(notificationIconColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Progress Bar
            var progressBarLabelFont = serializedObject.FindProperty("progressBarLabelFont");
            var progressBarLabelFontSize = serializedObject.FindProperty("progressBarLabelFontSize");
            var progressBarColor = serializedObject.FindProperty("progressBarColor");
            var progressBarBackgroundColor = serializedObject.FindProperty("progressBarBackgroundColor");
            var progressBarLoopBackgroundColor = serializedObject.FindProperty("progressBarLoopBackgroundColor");
            var progressBarLabelColor = serializedObject.FindProperty("progressBarLabelColor");
            showProgressBar = EditorGUILayout.Foldout(showProgressBar, "Progress Bar", foldoutStyle);

            if (showProgressBar)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(progressBarLabelFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(progressBarLabelFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(progressBarColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(progressBarLabelColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(progressBarBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Loop BG Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(progressBarLoopBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Scrollbar
            var scrollbarColor = serializedObject.FindProperty("scrollbarColor");
            var scrollbarBackgroundColor = serializedObject.FindProperty("scrollbarBackgroundColor");
            showScrollbar = EditorGUILayout.Foldout(showScrollbar, "Scrollbar", foldoutStyle);

            if (showScrollbar)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Bar Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(scrollbarColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(scrollbarBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Slider
            var sliderThemeType = serializedObject.FindProperty("sliderThemeType");
            var sliderLabelFont = serializedObject.FindProperty("sliderLabelFont");
            var sliderLabelFontSize = serializedObject.FindProperty("sliderLabelFontSize");
            var sliderColor = serializedObject.FindProperty("sliderColor");
            var sliderLabelColor = serializedObject.FindProperty("sliderLabelColor");
            var sliderPopupLabelColor = serializedObject.FindProperty("sliderPopupLabelColor");
            var sliderHandleColor = serializedObject.FindProperty("sliderHandleColor");
            var sliderBackgroundColor = serializedObject.FindProperty("sliderBackgroundColor");
            showSlider = EditorGUILayout.Foldout(showSlider, "Slider", foldoutStyle);

            if (showSlider && sliderThemeType.enumValueIndex == 0)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderThemeType, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderLabelFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(sliderLabelFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Popup Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderLabelColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            if (showSlider && sliderThemeType.enumValueIndex == 1)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Theme Type"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderThemeType, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderLabelFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(sliderLabelFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderLabelColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Label Popup Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderPopupLabelColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Handle Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderHandleColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(sliderBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Switch
            var switchBorderColor = serializedObject.FindProperty("switchBorderColor");
            var switchBackgroundColor = serializedObject.FindProperty("switchBackgroundColor");
            var switchHandleOnColor = serializedObject.FindProperty("switchHandleOnColor");
            var switchHandleOffColor = serializedObject.FindProperty("switchHandleOffColor");
            showSwitch = EditorGUILayout.Foldout(showSwitch, "Switch", foldoutStyle);

            if (showSwitch)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(switchBorderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(switchBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Handle On Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(switchHandleOnColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Handle Off Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(switchHandleOffColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Toggle
            var toggleFont = serializedObject.FindProperty("toggleFont");
            var toggleFontSize = serializedObject.FindProperty("toggleFontSize");
            var toggleTextColor = serializedObject.FindProperty("toggleTextColor");
            var toggleBorderColor = serializedObject.FindProperty("toggleBorderColor");
            var toggleBackgroundColor = serializedObject.FindProperty("toggleBackgroundColor");
            var toggleCheckColor = serializedObject.FindProperty("toggleCheckColor");
            showToggle = EditorGUILayout.Foldout(showToggle, "Toggle", foldoutStyle);

            if (showToggle)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(toggleFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(toggleFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(toggleTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(toggleBorderColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(toggleBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Check Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(toggleCheckColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(2);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Tooltip
            var tooltipFont = serializedObject.FindProperty("tooltipFont");
            var tooltipFontSize = serializedObject.FindProperty("tooltipFontSize");
            var tooltipTextColor = serializedObject.FindProperty("tooltipTextColor");
            var tooltipBackgroundColor = serializedObject.FindProperty("tooltipBackgroundColor");
            showTooltip = EditorGUILayout.Foldout(showTooltip, "Tooltip", foldoutStyle);

            if (showTooltip)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(tooltipFontSize, new GUIContent(""), GUILayout.Width(40));
                EditorGUILayout.PropertyField(tooltipFont, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Text Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(tooltipTextColor, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(tooltipBackgroundColor, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(7);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(6);

            var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent("Update Values"), customSkin.FindStyle("Toggle"));
            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            var enableExtendedColorPicker = serializedObject.FindProperty("enableExtendedColorPicker");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent("Extended Color Picker"), customSkin.FindStyle("Toggle"));
            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            if (enableExtendedColorPicker.boolValue == true)
                EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 1);

            else
                EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 0);

            var editorHints = serializedObject.FindProperty("editorHints");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent("UI Manager Hints"), customSkin.FindStyle("Toggle"));
            editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            if (editorHints.boolValue == true)
            {
                EditorGUILayout.HelpBox("These values are universal and will affect any object that contains 'UI Manager' component.", MessageType.Info);
                EditorGUILayout.HelpBox("Remove 'UI Manager' component from the object in order to get unique values.", MessageType.Info);
				EditorGUILayout.HelpBox("You can press 'CTRL + SHIFT + M' to open UI Manager quickly.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset to defaults", customSkin.button))
                ResetToDefaults();

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Apply & Update button
            GUILayout.Space(4);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            GUILayout.Label("Need help? Contact me via:", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();

            if (GUILayout.Button("Discord", customSkin.button))
                Discord();

            if (GUILayout.Button("E-mail", customSkin.button))
                Email();

            if (GUILayout.Button("YouTube", customSkin.button))
                YouTube();

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Website", customSkin.button))
                Website();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(12);
            GUILayout.Label("Loved the package and wanna support us even more?", customSkin.FindStyle("Text"));

            if (GUILayout.Button("Write a review", customSkin.button))
                Review();
        }

        void Discord()
        {
            Application.OpenURL("https://discord.gg/VXpHyUt");
        }

        void Email()
        {
            Application.OpenURL("mailto:support@michsky.com?subject=Contact");
        }

        void YouTube()
        {
            Application.OpenURL("https://www.youtube.com/c/michsky");
        }

        void Website()
        {
            Application.OpenURL("https://www.michsky.com/");
        }

        void Review()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/modern-ui-pack-150824/reviews/?page=1&sort_by=helpful");
        }

        void ResetToDefaults()
        {
            if (EditorUtility.DisplayDialog("Reset to defaults", "Are you sure you want to reset UI Manager values to default?", "Yes", "Cancel"))
            {
                try
                {
                    Preset defaultPreset = Resources.Load<Preset>("UI Manager Presets/Default");
                    defaultPreset.ApplyTo(Resources.Load("MUIP Manager"));
                    Selection.activeObject = null;
                    Debug.Log("UI Manager - Resetting successful.");
                }

                catch
                {
                    Debug.LogWarning("UI Manager - Resetting failed.");
                }
            }    
        }
    }
}
#endif