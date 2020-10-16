//////////////////////////////////////////////////////
// MK Glow Copy										//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////

#ifndef MK_GLOW_COPY
	#define MK_GLOW_COPY

	#include "../Inc/Common.hlsl"

	UNIFORM_SOURCE_SAMPLER_AND_TEXTURE(_SourceTex)

	#define HEADER half4 frag (VertGeoOutputSimple o) : SV_Target

	HEADER
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
		RETURN_TARGET_TEX SAMPLE_SOURCE;
	}
#endif