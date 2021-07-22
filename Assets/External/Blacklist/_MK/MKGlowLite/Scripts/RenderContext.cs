//////////////////////////////////////////////////////
// MK Glow RenderContext 	    	    	       	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using UnityEngine;

namespace MK.Glow
{
	#if UNITY_2018_3_OR_NEWER
    #if ENABLE_VR
    using XRSettings = UnityEngine.XR.XRSettings;
    #endif
    #endif

	internal sealed class RenderContext : IDimension
	{
		#if UNITY_2017_1_OR_NEWER
		private RenderTextureDescriptor _descriptor;
		public RenderTextureDescriptor descriptor { get{ return _descriptor; } }
		public RenderDimension renderDimension { get{ return new RenderDimension(_descriptor.width, _descriptor.height); } }
		#else
		private RenderDimension _descriptor;
		public RenderDimension descriptor { get{ return _descriptor; } }
		public RenderDimension renderDimension { get{ return _descriptor; } }
		#endif

		public int width { get{ return _descriptor.width; } }
		public int height { get{ return _descriptor.height; } }

		/// <summary>
		/// Create the rendercontext based on XR settings
		/// </summary>
		internal RenderContext()
		{
			#if UNITY_2018_3_OR_NEWER
			#if ENABLE_VR
			_descriptor = XRSettings.enabled ? XRSettings.eyeTextureDesc : new RenderTextureDescriptor();
			#else
			_descriptor = new RenderTextureDescriptor();
			#endif
			_descriptor.msaaSamples = 1;
			_descriptor.useMipMap = false;
            _descriptor.autoGenerateMips = false;
			#elif UNITY_2017_1_OR_NEWER
			_descriptor = new RenderTextureDescriptor();
			_descriptor.msaaSamples = 1;
			_descriptor.useMipMap = false;
            _descriptor.autoGenerateMips = false;
			#else
			_descriptor = new RenderDimension(0, 0);
			#endif

			#if UNITY_2019_2_OR_NEWER
				_descriptor.mipCount = 1;
			#endif
		}

		/// <summary>
		/// Doublewide the dimension if single pass stereo is enabled
		/// </summary>
		/// <param name="stereoEnabled"></param>
		internal void SinglePassStereoAdjustWidth(bool stereoEnabled)
		{
			_descriptor.width = stereoEnabled && PipelineProperties.singlePassStereoDoubleWideEnabled ? _descriptor.width * 2 : _descriptor.width;
		}

		/// <summary>
		/// Update a render context based on rendering settings including xr
		/// </summary>
		/// <param name="camera"></param>
		/// <param name="format"></param>
		/// <param name="depthBufferBits"></param>
		/// <param name="dimension"></param>
		internal void UpdateRenderContext(UnityEngine.Camera camera, RenderTextureFormat format, int depthBufferBits, RenderDimension dimension)
        {
			#if UNITY_2018_3_OR_NEWER
			#if ENABLE_VR
			_descriptor.dimension = camera.stereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.dimension : UnityEngine.Rendering.TextureDimension.Tex2D;
            _descriptor.vrUsage = camera.stereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage : VRTextureUsage.None;
            _descriptor.volumeDepth = camera.stereoEnabled ? UnityEngine.XR.XRSettings.eyeTextureDesc.volumeDepth : 1;
			#else
			_descriptor.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            _descriptor.vrUsage = VRTextureUsage.None;
            _descriptor.volumeDepth = 1;
			#endif
			#elif UNITY_2017_1_OR_NEWER
			_descriptor.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            _descriptor.vrUsage = VRTextureUsage.None;
            _descriptor.volumeDepth = 1;
			#endif

			#if UNITY_2017_1_OR_NEWER
            _descriptor.colorFormat = format;
            _descriptor.depthBufferBits = depthBufferBits;
            _descriptor.width = dimension.width;
            _descriptor.height = dimension.height;
            _descriptor.memoryless = RenderTextureMemoryless.None;
            _descriptor.sRGB = RenderTextureReadWrite.Default != RenderTextureReadWrite.Linear;
			#else
			_descriptor.width = dimension.width;
            _descriptor.height = dimension.height;
			#endif
        }
	}
}
