//////////////////////////////////////////////////////
// MK Glow Debug									//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////

#ifndef MK_GLOW_DEBUG
	#define MK_GLOW_DEBUG

	#include "../Inc/Common.hlsl"

	#if defined(MK_DEBUG_RAW_BLOOM)
		UNIFORM_SOURCE_SAMPLER_AND_TEXTURE(_SourceTex)
		uniform float2 _SourceTex_TexelSize;
		#ifndef MK_NATURAL
			uniform half2 _BloomThreshold;
		#endif
		uniform half _LumaScale;
	#elif defined(MK_DEBUG_COMPOSITE)
		UNIFORM_SOURCE_SAMPLER_AND_TEXTURE(_SourceTex)
		UNIFORM_SAMPLER_AND_TEXTURE_2D(_BloomTex)
		uniform float2 _BloomTex_TexelSize;
		uniform half _BloomSpread;
		uniform half _BloomIntensity;

		#ifdef MK_LENS_SURFACE
			uniform half _LensSurfaceDirtIntensity;
			uniform half _LensSurfaceDiffractionIntensity;
			uniform float4 _LensSurfaceDirtTex_ST;
			UNIFORM_SAMPLER_AND_TEXTURE_2D_NO_SCALE(_LensSurfaceDirtTex)
			UNIFORM_SAMPLER_AND_TEXTURE_2D_NO_SCALE(_LensSurfaceDiffractionTex)
		#endif
	#else
		uniform float2 _BloomTex_TexelSize;
		UNIFORM_SAMPLER_AND_TEXTURE_2D(_BloomTex)
		uniform half _BloomSpread;
		uniform half _BloomIntensity;
	#endif

	#ifdef MK_DEBUG_COMPOSITE
		#if defined(MK_LENS_SURFACE)
			#define VertGeoOutput VertGeoOutputPlus
			#define MK_LENS_SURFACE_DIFFRACTION_UV uv1.xy
			#define LENS_FLARE_SPREAD uv1.xy
		#else
			#define VertGeoOutput VertGeoOutputAdvanced
		#endif
	#else
		#define VertGeoOutput VertGeoOutputAdvanced
	#endif

	VertGeoOutput vert (VertexInputOnlyPosition i0)
	{
		VertGeoOutput o;
		UNITY_SETUP_INSTANCE_ID(i0);
		UNITY_INITIALIZE_OUTPUT(VertGeoOutput, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.pos = TransformMeshPos(i0.vertex);

		#ifdef MK_LEGACY_BLIT
			o.uv0.xy = i0.texcoord0;
			#if UNITY_UV_STARTS_AT_TOP
				#if defined(MK_DEBUG_RAW_BLOOM)
					if (_SourceTex_TexelSize.y < 0)
						o.uv0.xy = 1-o.uv0.xy;
				#elif defined(MK_DEBUG_COMPOSITE)
					if (_BloomTex_TexelSize.y < 0)
						o.uv0.xy = 1-o.uv0.xy;
				#else //MK_DEBUG_BLOOM
					if (_BloomTex_TexelSize.y < 0)
						o.uv0.xy = 1-o.uv0.xy;
				#endif
			#endif
		#else
			o.uv0.xy = SetMeshUV(o.pos.xy);
		#endif

		#if defined(MK_DEBUG_BLOOM) || defined(MK_DEBUG_COMPOSITE)
			o.uv0.zw = BLOOM_TEXEL_SIZE * _BloomSpread;
		#endif

		#if defined(MK_DEBUG_COMPOSITE) && defined(MK_LENS_SURFACE)
			o.MK_LENS_SURFACE_DIFFRACTION_UV = LensSurfaceDiffractionUV(o.uv0.xy);
		#endif

		return o;
	}

	#if defined(MK_DEBUG_RAW_BLOOM)
		#define HEADER half4 frag (VertGeoOutput o) : SV_Target
	#endif
	#ifdef MK_DEBUG_RAW_BLOOM
		HEADER
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
			RETURN_TARGET_TEX ConvertToColorSpace(half4(LuminanceThreshold(SampleSourceTex(PASS_SOURCE_TEXTURE_2D(_SourceTex, sampler_SourceTex), UV_0).rgb, BLOOM_THRESHOLD), 1));
		}
	#elif defined(MK_DEBUG_COMPOSITE)
		#define HEADER half4 frag (VertGeoOutput o) : SV_Target

		HEADER
		{
			#include "CompositeSample.hlsl"
		}
	#else
		#define HEADER half4 frag (VertGeoOutput o) : SV_Target
		HEADER
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);

			half4 g = SampleTex2D(PASS_TEXTURE_2D(_BloomTex, sampler_BloomTex), UV_0);
			g *= BLOOM_INTENSITY;

			RETURN_TARGET_TEX ConvertToColorSpace(g);
		}
	#endif
#endif