//////////////////////////////////////////////////////
// MK Glow Lite 	    	    	                //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MK.Glow.Legacy
{
	#if UNITY_2018_3_OR_NEWER
        [ExecuteAlways]
    #else
        [ExecuteInEditMode]
    #endif
    [DisallowMultipleComponent]
    [ImageEffectAllowedInSceneView]
    [RequireComponent(typeof(UnityEngine.Camera))]
	public class MKGlowLite : MonoBehaviour
	{
        #if UNITY_EDITOR && !UNITY_CLOUD_BUILD
        public bool showEditorMainBehavior = true;
		public bool showEditorBloomBehavior;
		public bool showEditorLensSurfaceBehavior;
        #endif

        //Main
        public DebugView debugView = MK.Glow.DebugView.None;
        public Workflow workflow = MK.Glow.Workflow.Threshold;
        public LayerMask selectiveRenderLayerMask = -1;
        [Range(-1f, 1f)]
        public float anamorphicRatio = 0f;

        //Bloom
        [MK.Glow.MinMaxRange(0, 10)]
        public MinMaxRange bloomThreshold = new MinMaxRange(1.25f, 10f);
        [Range(1f, 10f)]
		public float bloomScattering = 7f;
		public float bloomIntensity = 1f;

        //LensSurface
        public bool allowLensSurface = false;
		public Texture2D lensSurfaceDirtTexture;
		public float lensSurfaceDirtIntensity = 2.5f;
		public Texture2D lensSurfaceDiffractionTexture;
		public float lensSurfaceDiffractionIntensity = 2.0f;

        private Effect _effect;

        private RenderTarget _source, _destination;

		private UnityEngine.Camera renderingCamera
		{
			get { return GetComponent<UnityEngine.Camera>(); }
		}

		public void OnEnable()
		{
            _effect = new Effect();
			_effect.Enable(RenderPipeline.Legacy);

            enabled = Compatibility.IsSupported;
		}

		public void OnDisable()
		{
			_effect.Disable();
		}

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if(workflow == Workflow.Selective && PipelineProperties.xrEnabled)
            {
                Graphics.Blit(source, destination);
                return;
            }

            _source.renderTexture = source;
            _destination.renderTexture = destination;

            SettingsLegacy settings = this;

			_effect.Build(_source, _destination, settings, null, renderingCamera);

            Graphics.Blit(source, destination, _effect.renderMaterialNoGeometry, _effect.currentRenderIndex);
            _effect.AfterCompositeCleanup();
        }
	}
}