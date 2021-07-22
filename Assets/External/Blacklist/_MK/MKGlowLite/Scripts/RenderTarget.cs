//////////////////////////////////////////////////////
// MK Glow RenderTarget 	    	                //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
namespace MK.Glow
{
	internal struct RenderTarget 
	{
		internal UnityEngine.RenderTexture renderTexture;
		internal UnityEngine.Rendering.RenderTargetIdentifier renderTargetIdentifier;
		internal int identifier;

		/*
		public static implicit operator UnityEngine.RenderTexture(MK.Glow.RenderTarget input)
        {
			return input.renderTexture;
		}
		public static implicit operator UnityEngine.Rendering.RenderTargetIdentifier(MK.Glow.RenderTarget input)
        {
			return input.renderTargetIdentifier;
		}
		*/
	}
}