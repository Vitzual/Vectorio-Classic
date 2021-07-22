//////////////////////////////////////////////////////
// MK Glow Lite Editor Legacy	    			    //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using MK.Glow.Editor;

namespace MK.Glow.Legacy.Editor
{
	using Tooltips = MK.Glow.Editor.EditorHelper.EditorUIContent.Tooltips;

    [CustomEditor(typeof(MK.Glow.Legacy.MKGlowLite))]
    internal class MKGlowEditor : UnityEditor.Editor
	{
		//Behaviors
		private SerializedProperty _showEditorMainBehavior;
		private SerializedProperty _showEditorBloomBehavior;
		private SerializedProperty _showEditorLensSurfaceBehavior;

		//Main
		private SerializedProperty _debugView;
		private SerializedProperty _workflow;
		private SerializedProperty _selectiveRenderLayerMask;
		private SerializedProperty _anamorphicRatio;

		//Bloom
		private SerializedProperty _bloomThreshold;
		private SerializedProperty _bloomScattering;
		private SerializedProperty _bloomIntensity;

		//Lens Surface
		private SerializedProperty _allowLensSurface;
		private SerializedProperty _lensSurfaceDirtTexture;
		private SerializedProperty _lensSurfaceDirtIntensity;
		private SerializedProperty _lensSurfaceDiffractionTexture;
		private SerializedProperty _lensSurfaceDiffractionIntensity;

		public void OnEnable()
		{
			//Editor
			_showEditorMainBehavior = serializedObject.FindProperty("showEditorMainBehavior");
			_showEditorBloomBehavior = serializedObject.FindProperty("showEditorBloomBehavior");
			_showEditorLensSurfaceBehavior = serializedObject.FindProperty("showEditorLensSurfaceBehavior");

			//Main
			_debugView = serializedObject.FindProperty("debugView");
			_workflow = serializedObject.FindProperty("workflow");
			_selectiveRenderLayerMask = serializedObject.FindProperty("selectiveRenderLayerMask");
			_anamorphicRatio = serializedObject.FindProperty("anamorphicRatio");

			//Bloom
			_bloomThreshold = serializedObject.FindProperty("bloomThreshold");
			_bloomScattering = serializedObject.FindProperty("bloomScattering");
			_bloomIntensity = serializedObject.FindProperty("bloomIntensity");

			_allowLensSurface = serializedObject.FindProperty("allowLensSurface");
			_lensSurfaceDirtTexture = serializedObject.FindProperty("lensSurfaceDirtTexture");
			_lensSurfaceDirtIntensity = serializedObject.FindProperty("lensSurfaceDirtIntensity");
			_lensSurfaceDiffractionTexture = serializedObject.FindProperty("lensSurfaceDiffractionTexture");
			_lensSurfaceDiffractionIntensity = serializedObject.FindProperty("lensSurfaceDiffractionIntensity");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorHelper.VerticalSpace();

			EditorHelper.EditorUIContent.IsNotSupportedWarning();
			EditorHelper.EditorUIContent.XRUnityVersionWarning();
			if(_workflow.enumValueIndex == 1)
            {
				EditorHelper.EditorUIContent.SelectiveWorkflowDeprecated();
			}
			
			if(EditorHelper.HandleBehavior(_showEditorMainBehavior.serializedObject.targetObject, EditorHelper.EditorUIContent.mainTitle, "", _showEditorMainBehavior, null))
			{
				EditorGUILayout.PropertyField(_debugView, Tooltips.debugView);
				EditorGUILayout.PropertyField(_workflow, Tooltips.workflow);
				EditorHelper.EditorUIContent.SelectiveWorkflowVRWarning((Workflow)_workflow.enumValueIndex);
                if(_workflow.enumValueIndex == 1)
                {
                    EditorGUILayout.PropertyField(_selectiveRenderLayerMask, Tooltips.selectiveRenderLayerMask);
                }
				EditorGUILayout.PropertyField(_anamorphicRatio, Tooltips.anamorphicRatio);
				EditorHelper.VerticalSpace();
			}
			
			if(EditorHelper.HandleBehavior(_showEditorBloomBehavior.serializedObject.targetObject, EditorHelper.EditorUIContent.bloomTitle, "", _showEditorBloomBehavior, null))
			{
				if(_workflow.enumValueIndex == 0)
					EditorGUILayout.PropertyField(_bloomThreshold, Tooltips.bloomThreshold);
				EditorGUILayout.PropertyField(_bloomScattering, Tooltips.bloomScattering);
				EditorGUILayout.PropertyField(_bloomIntensity, Tooltips.bloomIntensity);
				_bloomIntensity.floatValue = Mathf.Max(0, _bloomIntensity.floatValue);

				EditorHelper.VerticalSpace();
			}

			if(EditorHelper.HandleBehavior(_showEditorLensSurfaceBehavior.serializedObject.targetObject, EditorHelper.EditorUIContent.lensSurfaceTitle, "", _showEditorLensSurfaceBehavior, _allowLensSurface))
			{
				using (new EditorGUI.DisabledScope(!_allowLensSurface.boolValue))
                {
					EditorHelper.DrawHeader(EditorHelper.EditorUIContent.dirtTitle);
					EditorGUILayout.PropertyField(_lensSurfaceDirtTexture, Tooltips.lensSurfaceDirtTexture);
					EditorGUILayout.PropertyField(_lensSurfaceDirtIntensity, Tooltips.lensSurfaceDirtIntensity);
					_lensSurfaceDirtIntensity.floatValue = Mathf.Max(0, _lensSurfaceDirtIntensity.floatValue);
					EditorGUILayout.Space();
					EditorHelper.DrawHeader(EditorHelper.EditorUIContent.diffractionTitle);
					EditorGUILayout.PropertyField(_lensSurfaceDiffractionTexture, Tooltips.lensSurfaceDiffractionTexture);
					EditorGUILayout.PropertyField(_lensSurfaceDiffractionIntensity, Tooltips.lensSurfaceDiffractionIntensity);
					_lensSurfaceDiffractionIntensity.floatValue = Mathf.Max(0, _lensSurfaceDiffractionIntensity.floatValue);
				}
				EditorHelper.VerticalSpace();
			}
			EditorHelper.DrawSplitter();

			serializedObject.ApplyModifiedProperties();
		}
    }
}
#endif