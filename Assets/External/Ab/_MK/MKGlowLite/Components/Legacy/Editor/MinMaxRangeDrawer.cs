//////////////////////////////////////////////////////
// MK Glow Range Drawer    	    	       			//
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

namespace MK.Glow.Legacy.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	internal class MinMaxRangeDrawer : PropertyDrawer
	{
		public override void OnGUI( Rect startRect, SerializedProperty property, GUIContent label )
		{
			EditorGUI.BeginProperty(startRect, label, property);
			MinMaxRangeAttribute range = attribute as MinMaxRangeAttribute;
			SerializedProperty minRange = property.FindPropertyRelative("minValue");
			SerializedProperty maxRange = property.FindPropertyRelative("maxValue");
			float minValue = minRange.floatValue;
			float maxValue = maxRange.floatValue;

			Rect minRect = new Rect(EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth * 0.33f, startRect.y, EditorGUIUtility.fieldWidth, startRect.height);
			float p = minRect.x + EditorGUIUtility.standardVerticalSpacing * 2f + EditorGUIUtility.fieldWidth;
			Rect sliderRect = new Rect(p, startRect.y, startRect.width - p - EditorGUIUtility.fieldWidth + EditorGUIUtility.standardVerticalSpacing * 5f, startRect.height);
			Rect maxRect = new Rect(sliderRect.x + sliderRect.width + EditorGUIUtility.standardVerticalSpacing * 2f, startRect.y, EditorGUIUtility.fieldWidth, startRect.height);

			EditorGUI.LabelField(startRect, label);
			minValue = EditorGUI.FloatField(minRect, minValue);

			EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, range.minLimit, range.maxLimit);
			maxValue = EditorGUI.FloatField(maxRect, maxValue);

			minRange.floatValue = minValue;
			maxRange.floatValue = maxValue;
			EditorGUI.EndProperty();
		}
	}
}
#endif