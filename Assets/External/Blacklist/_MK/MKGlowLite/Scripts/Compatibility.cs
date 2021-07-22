//////////////////////////////////////////////////////
// MK Glow Compatibility	    	    	       	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MK.Glow
{
	public static class Compatibility
    {
        private static readonly bool _defaultHDRFormatSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR);
        private static readonly bool _11R11G10BFormatSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB111110Float);
        private static readonly bool _2A10R10G10BFormatSupported = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010);
        //RenderToTexture and a hdr color format required
        public static readonly bool IsSupported = _11R11G10BFormatSupported ? true : _2A10R10G10BFormatSupported ? true : _defaultHDRFormatSupported ? true : false;

        /// <summary>
        /// Returns the supported rendertexture format used for rendering
        /// </summary>
        /// <returns></returns>
        internal static RenderTextureFormat CheckSupportedRenderTextureFormat()
        {
            //return _defaultHDRFormatSupported ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
            return _11R11G10BFormatSupported ? RenderTextureFormat.RGB111110Float : _2A10R10G10BFormatSupported ? RenderTextureFormat.ARGB2101010 : _defaultHDRFormatSupported ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
        }
    }
}