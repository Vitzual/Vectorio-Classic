//////////////////////////////////////////////////////
// MK Glow RenderTargetContext 	    	    	    //
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
    //To reduce garbage collection this part is hardcoded
    /// <summary>
    /// Render targets based on a given render context
    /// </summary>
	internal static class RenderTargetContext
	{
		private static int _renderTargetCount;

        private static RenderTargetSetup[] _mrtBindingsLegacy = new RenderTargetSetup[6]
        {
            new RenderTargetSetup
            (
                new RenderBuffer[1], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[1]{RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[1]{RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ) 
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
            ,
            new RenderTargetSetup
            (
                new RenderBuffer[2], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[2]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[2]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
            ,
            new RenderTargetSetup
            (
                new RenderBuffer[3], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[3]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[3]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
            ,
            new RenderTargetSetup
            (
                new RenderBuffer[4], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[4]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[4]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
            ,
            new RenderTargetSetup
            (
                new RenderBuffer[5], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[5]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[5]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
            ,
            new RenderTargetSetup
            (
                new RenderBuffer[6], 
                new RenderBuffer(),
                0,
                CubemapFace.Unknown,
                new RenderBufferLoadAction[6]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[6]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
            #if UNITY_2018_3_OR_NEWER
            { depthSlice = -1 }
            #endif
        };

		#if UNITY_2018_3_OR_NEWER
        private static RenderTargetBinding[] _mrtBindingsSRP = new RenderTargetBinding[6]
        {
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[1], 
                new RenderBufferLoadAction[1]{RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[1]{RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ),
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[2], 
                new RenderBufferLoadAction[2]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[2]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ),
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[3], 
                new RenderBufferLoadAction[3]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[3]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ),
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[4], 
                new RenderBufferLoadAction[4]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[4]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ),
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[5], 
                new RenderBufferLoadAction[5]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[5]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            ),
            new RenderTargetBinding
            (
                new RenderTargetIdentifier[6], 
                new RenderBufferLoadAction[6]{RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare, RenderBufferLoadAction.DontCare},
                new RenderBufferStoreAction[6]{RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store, RenderBufferStoreAction.Store},
                0, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store
            )
        };

        internal static void SetRenderTargetContext(this CommandBuffer cmd, List<RenderTarget> renderTargets)
		{
			_renderTargetCount = renderTargets.Count - 1;
            for(int i = 0; i <= _renderTargetCount; i++)
            {
                _mrtBindingsSRP[_renderTargetCount].colorRenderTargets[i] = renderTargets[i].renderTargetIdentifier;
            }
            _mrtBindingsSRP[_renderTargetCount].depthRenderTarget = _mrtBindingsSRP[_renderTargetCount].colorRenderTargets[0];
            cmd.SetRenderTarget(_mrtBindingsSRP[_renderTargetCount]);
		}
        #else
        private static RenderTargetIdentifier[][] _mrtBindingsSRP = new RenderTargetIdentifier[6][]
        {
            new RenderTargetIdentifier[1]{new RenderTargetIdentifier()},
            new RenderTargetIdentifier[2]{new RenderTargetIdentifier(), new RenderTargetIdentifier()},
            new RenderTargetIdentifier[3]{new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier()},
            new RenderTargetIdentifier[4]{new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier()},
            new RenderTargetIdentifier[5]{new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier(), new RenderTargetIdentifier()},
            new RenderTargetIdentifier[6]{new RenderTargetIdentifier(), new RenderTargetIdentifier() ,new RenderTargetIdentifier(), new RenderTargetIdentifier() ,new RenderTargetIdentifier(), new RenderTargetIdentifier()}
        };

		internal static void SetRenderTargetContext(this CommandBuffer cmd, List<RenderTarget> renderTargets)
		{
			_renderTargetCount = renderTargets.Count - 1;
            for(int i = 0; i <= _renderTargetCount; i++)
            {
                _mrtBindingsSRP[_renderTargetCount][i] = renderTargets[i].renderTargetIdentifier;
            }
            cmd.SetRenderTarget(_mrtBindingsSRP[_renderTargetCount], _mrtBindingsSRP[_renderTargetCount][0]);
		}
        #endif

        internal static void SetRenderTargetContext(List<RenderTarget> renderTargets)
		{
			_renderTargetCount = renderTargets.Count - 1;
            for(int i = 0; i <= _renderTargetCount; i++)
            {
                _mrtBindingsLegacy[_renderTargetCount].color[i] = renderTargets[i].renderTexture.colorBuffer;
            }
            _mrtBindingsLegacy[_renderTargetCount].depth = renderTargets[_renderTargetCount].renderTexture.depthBuffer;
            Graphics.SetRenderTarget(_mrtBindingsLegacy[_renderTargetCount]);
		}
	}
}
