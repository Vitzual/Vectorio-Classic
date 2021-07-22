//////////////////////////////////////////////////////
// MK Glow Editor Helper Styles	    	    	   	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;

namespace MK.Glow.Editor
{
    internal static partial class EditorHelper
    {
        //Based on Postprocessing stack styles to match the postprocessing stack ui
        private static class EditorStyles
        {
            internal static readonly GUIStyle rightAlignetLabel = new GUIStyle(UnityEditor.EditorStyles.label) { alignment = TextAnchor.MiddleRight };

            internal static readonly GUIStyle largeHeader = new GUIStyle(UnityEditor.EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 18 };

            internal static readonly GUIStyle headerCheckbox = new GUIStyle("ShurikenCheckMark");
            internal static readonly GUIStyle headerCheckboxMixed = new GUIStyle("ShurikenCheckMarkMixed");

            internal static readonly GUIStyle smallTickbox = new GUIStyle("ShurikenToggle");

            static readonly Color splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
            static readonly Color splitterLight = splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);
            internal static Color splitter { get { return EditorGUIUtility.isProSkin ? splitterDark : splitterLight; } }

            static readonly Texture2D paneOptionsIconDark;
            static readonly Texture2D paneOptionsIconLight;

            internal static Texture2D paneOptionsIcon { get { return EditorGUIUtility.isProSkin ? paneOptionsIconDark : paneOptionsIconLight; } }

            internal static readonly GUIStyle headerLabel = new GUIStyle(UnityEditor.EditorStyles.miniLabel);

            static readonly Color headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
            static readonly Color headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);
            internal static Color headerBackground { get { return EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight; } }

            internal static readonly GUIStyle wheelLabel = new GUIStyle(UnityEditor.EditorStyles.miniLabel);
            internal static readonly GUIStyle wheelThumb = new GUIStyle("ColorPicker2DThumb");
            internal static readonly Vector2 wheelThumbSize = new Vector2(
                    !Mathf.Approximately(wheelThumb.fixedWidth, 0f) ? wheelThumb.fixedWidth : wheelThumb.padding.horizontal,
                    !Mathf.Approximately(wheelThumb.fixedHeight, 0f) ? wheelThumb.fixedHeight : wheelThumb.padding.vertical
                );

            internal static readonly GUIStyle preLabel = new GUIStyle("ShurikenLabel");

            static EditorStyles()
            {
                paneOptionsIconDark = (Texture2D)EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
                paneOptionsIconLight = (Texture2D)EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");
            }
        }
    }
}
#endif