//////////////////////////////////////////////////////
// MK Glow Pre Sample								//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////

#ifndef MK_GLOW_PRE_SAMPLE
	#define MK_GLOW_PRE_SAMPLE

	#include "../Inc/Common.hlsl"
	
	UNIFORM_SOURCE_SAMPLER_AND_TEXTURE(_SourceTex)
	uniform float2 _SourceTex_TexelSize;
	uniform half _LumaScale;
	#ifdef MK_BLOOM
		uniform half2 _BloomThreshold;
	#endif

	#ifdef MK_COPY
	#endif
		
	#define HEADER FragmentOutputAuto frag (VertGeoOutputSimple o)

	HEADER
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
		FragmentOutputAuto fO;
		UNITY_INITIALIZE_OUTPUT(FragmentOutputAuto, fO);
		
		#ifdef MK_BLOOM
			half4 bloom = 0;

			bloom = SampleSourceTex(PASS_SOURCE_TEXTURE_2D(_SourceTex, sampler_SourceTex), BLOOM_UV);
			bloom = half4(LuminanceThreshold(bloom.rgb, BLOOM_THRESHOLD), 1);

			#ifdef COLORSPACE_GAMMA
				bloom = GammaToLinearSpace4(bloom);
			#endif
			BLOOM_RENDER_TARGET = bloom;
		#endif

		#ifdef MK_COPY
			COPY_RENDER_TARGET =  SampleSourceTex(PASS_SOURCE_TEXTURE_2D(_SourceTex, sampler_SourceTex), UV_COPY);
		#endif

		return fO;
	}
#endif