//////////////////////////////////////////////////////
// MK Glow Downsample		 						//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////
#ifndef MK_GLOW_DOWNSAMPLE
	#define MK_GLOW_DOWNSAMPLE

	#include "../Inc/Common.hlsl"
	
	#ifdef MK_BLOOM
		UNIFORM_SAMPLER_AND_TEXTURE_2D(_BloomTex)
		uniform float2 _BloomTex_TexelSize;
	#endif

	#define HEADER FragmentOutputAuto frag (VertGeoOutputSimple o)

	HEADER
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
		FragmentOutputAuto fO;
		UNITY_INITIALIZE_OUTPUT(FragmentOutputAuto, fO);

		#ifdef MK_BLOOM
			BLOOM_RENDER_TARGET = DownsampleLQ(PASS_TEXTURE_2D(_BloomTex, sampler_BloomTex), BLOOM_UV, BLOOM_TEXEL_SIZE);
		#endif

		return fO;
	}
#endif