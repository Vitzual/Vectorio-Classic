//////////////////////////////////////////////////////
// MK Glow Editor Helper Main  	    	       		//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MK.Glow.Editor
{
    internal static partial class EditorHelper
    {
        /// <summary>
        /// Draw a default splitter
        /// </summary>
        internal static void DrawSplitter()
        {
            var rect = GUILayoutUtility.GetRect(0f, 1f);

            rect.xMin = 0f;
            rect.width += 4f;

            if(Event.current.type != EventType.Repaint)
                return;

            EditorGUI.DrawRect(rect, EditorStyles.splitter);
        }

        /// <summary>
        /// Foldout for settings
        /// </summary>
        /// <param name="title"></param>
        /// <param name="titleRight"></param>
        /// <returns></returns>
        private static Rect DrawFoldoutHeader(string title, string titleRight = "")
        {
            var gap = GUILayoutUtility.GetRect(0f, 0f);
            gap.xMin = 0f;
            gap.width += 4f;
            EditorGUI.DrawRect(gap, Color.clear);
            DrawSplitter();
            var rect = GUILayoutUtility.GetRect(16f, 16f);

            rect.xMin = 0f;
            rect.width += 4f;

            var lavelRect = new Rect(rect);
            lavelRect.xMin += 22;
            EditorGUI.DrawRect(rect, EditorStyles.headerBackground);
            EditorGUI.LabelField(lavelRect, title, UnityEditor.EditorStyles.boldLabel);
            EditorGUI.LabelField(lavelRect, titleRight, EditorStyles.rightAlignetLabel);

            return rect;
        }
        
        /// <summary>
        /// Creates a empty space with the height of 1
        /// </summary>
        internal static void VerticalSpace()
        {
            GUILayoutUtility.GetRect(1f, EditorGUIUtility.standardVerticalSpacing);
        }

        /// <summary>
        /// Draws a header
        /// </summary>
        /// <param name="text"></param>
        internal static void DrawHeader(string text)
        {
            EditorGUILayout.LabelField(text, UnityEditor.EditorStyles.boldLabel);
        }

		/// <summary>
		/// Draw a clickable behavior including a checkbox for a feature
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="title"></param>
		/// <param name="titleRight"></param>
		/// <param name="behavior"></param>
		/// <param name="feature"></param>
		/// <returns></returns>
        internal static bool HandleBehavior(UnityEngine.Object obj, string title, string titleRight, SerializedProperty behavior, SerializedProperty feature)
        {
            Rect rect = DrawFoldoutHeader(title, titleRight);
                
            var e = Event.current;

            var foldoutRect = new Rect(EditorGUIUtility.currentViewWidth * 0.5f, rect.y, 13f, 13f);
            if(behavior.hasMultipleDifferentValues)
            {
                foldoutRect.x -= 13;
            }

            //DrawSplitter();
            if(feature != null)
            {
                EditorGUI.showMixedValue = feature.hasMultipleDifferentValues;
                var toggleRect = new Rect(rect.x + 4f, rect.y + ((feature.hasMultipleDifferentValues) ? 0.0f : 2.0f), 13f, 13f);
                bool fn = feature.boolValue;
                EditorGUI.BeginChangeCheck();

                fn = EditorGUI.Toggle(toggleRect, "", fn, EditorStyles.headerCheckbox);

                if(EditorGUI.EndChangeCheck())
                {
                    feature.boolValue = fn;
                    if(feature.boolValue)
                        Undo.RegisterCompleteObjectUndo(obj, feature.displayName + " enabled");
                    else
                        Undo.RegisterCompleteObjectUndo(obj, feature.displayName + " disabled");
                }
                EditorGUI.showMixedValue = false;

                EditorGUI.showMixedValue = behavior.hasMultipleDifferentValues;
            }

            EditorGUI.BeginChangeCheck();
            if(e.type == EventType.MouseDown)
            {
                if(rect.Contains(e.mousePosition))
                {
                    if(behavior.hasMultipleDifferentValues)
                        behavior.boolValue = false;
                    else
                        behavior.boolValue = !behavior.boolValue;
                    e.Use();
                }
            }
            if(EditorGUI.EndChangeCheck())
            {
                if(behavior.boolValue)
                    Undo.RegisterCompleteObjectUndo(obj, behavior.displayName + " Show");
                else
                    Undo.RegisterCompleteObjectUndo(obj, behavior.displayName + " Hide");
            }

            EditorGUI.showMixedValue = false;

            if(e.type == EventType.Repaint && behavior.hasMultipleDifferentValues)
                UnityEditor.EditorStyles.radioButton.Draw(foldoutRect, "", false, false, true, false);
            else
                EditorGUI.Foldout(foldoutRect, behavior.boolValue, "");

            if(behavior.hasMultipleDifferentValues)
                return true;
            else
                return behavior.boolValue;
        }
	}
}
#endif
