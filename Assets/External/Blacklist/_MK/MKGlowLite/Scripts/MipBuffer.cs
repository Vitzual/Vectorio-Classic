//////////////////////////////////////////////////////
// MK Glow Mip Buffer	    	    	       		//
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
	/// <summary>
	/// Renderbuffer based on a mip setup
	/// </summary>
	internal sealed class MipBuffer
	{
		private RenderTarget[] _renderTargets = new RenderTarget[PipelineProperties.renderBufferSize];
		internal RenderTarget[] renderTargets { get { return _renderTargets; } }

		public MipBuffer(string name, RenderPipeline renderPipeline)
		{
			if(renderPipeline == RenderPipeline.SRP)
			{
				for(int i = 0; i < PipelineProperties.renderBufferSize; i++)
				{
					_renderTargets[i].identifier = Shader.PropertyToID(name + i);
					#if UNITY_2018_2_OR_NEWER
					_renderTargets[i].renderTargetIdentifier = new RenderTargetIdentifier(renderTargets[i].identifier, 0, CubemapFace.Unknown, -1);
					#else
					_renderTargets[i].renderTargetIdentifier = new RenderTargetIdentifier(renderTargets[i].identifier);
					#endif
				}
			}
			else
			{
				for(int i = 0; i < PipelineProperties.renderBufferSize; i++)
				{
					renderTargets[i].identifier = Shader.PropertyToID(name + i);
				}
			}
		}

		/// <summary>
		/// Create a specific level of the the buffer
		/// </summary>
		/// <param name="renderContext"></param>
		/// <param name="level"></param>
		/// <param name="cmd"></param>
		/// <param name="format"></param>
		internal void CreateTemporary(RenderContext[] renderContext, int level, CommandBuffer cmd, RenderTextureFormat format, RenderPipeline renderPipeline)
		{
			if(renderPipeline == RenderPipeline.SRP)
			{
				#if UNITY_2017_1_OR_NEWER
				cmd.GetTemporaryRT(renderTargets[level].identifier, renderContext[level].descriptor, FilterMode.Bilinear);
				#else
				cmd.GetTemporaryRT(renderTargets[level].identifier, renderContext[level].width, renderContext[level].height, 0, FilterMode.Bilinear, format, RenderTextureReadWrite.Default, 1);
				#endif
			}
			else
			{
				renderTargets[level].renderTexture = PipelineExtensions.GetTemporary(renderContext[level], format);
			}
		}

		/// <summary>
		/// Clear a specific level of the buffer
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="level"></param>
		internal void ClearTemporary(CommandBuffer cmd, int level, RenderPipeline renderPipeline)
		{
			if(renderPipeline == RenderPipeline.SRP)
				cmd.ReleaseTemporaryRT(renderTargets[level].identifier);
			else
				RenderTexture.ReleaseTemporary(renderTargets[level].renderTexture);
		}
	}
}