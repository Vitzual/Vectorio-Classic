//////////////////////////////////////////////////////
// MK Glow Common									//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////

//////////////////////////////////////////////////////
// Keyword replacement matrix                       //
// These keywords are already build-in so we can use//
// some slots to avoid reaching keyword limit (256) //
//////////////////////////////////////////////////////
// _MK_BLOOM             	    | _NORMALMAP                           | MK_BLOOM             	
// _MK_LENS_SURFACE      	    | _ALPHATEST_ON                        | MK_LENS_SURFACE      	
// _MK_LENS_FLARE        	    | _ALPHABLEND_ON                       | MK_LENS_FLARE        	
// _MK_GLARE_1             	    | _ALPHAPREMULTIPLY_ON                 | MK_GLARE_1             
// _MK_GLARE_2             	    | DIRECTIONAL                          | MK_GLARE_2             
// _MK_GLARE_3             	    | DIRECTIONAL_COOKIE                   | MK_GLARE_3             
// _MK_GLARE_4             	    | POINT                                | MK_GLARE_4             
// _MK_DEBUG_RAW_BLOOM      	| _EMISSION                            | MK_DEBUG_RAW_BLOOM      	
// _MK_DEBUG_RAW_LENS_FLARE 	| _METALLICGLOSSMAP                    | MK_DEBUG_RAW_LENS_FLARE 	
// _MK_DEBUG_RAW_GLARE      	| _DETAIL_MULX2                        | MK_DEBUG_RAW_GLARE      	
// _MK_DEBUG_BLOOM          	| _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A | MK_DEBUG_BLOOM          	
// _MK_DEBUG_LENS_FLARE     	| _SPECULARHIGHLIGHTS_OFF              | MK_DEBUG_LENS_FLARE     	
// _MK_DEBUG_GLARE          	| _GLOSSYREFLECTIONS_OFF               | MK_DEBUG_GLARE          	
// _MK_COPY              	    | _PARALLAXMAP                         | MK_COPY              	
// _MK_DEBUG_COMPOSITE      	| EDITOR_VISUALIZATION                 | MK_DEBUG_COMPOSITE      	
// _MK_LEGACY_BLIT      		| _COLOROVERLAY_ON                     | MK_LEGACY_BLIT      		
// _MK_RENDER_PRIORITY_QUALITY  | _COLORCOLOR_ON                       | MK_RENDER_PRIORITY_QUALITY
// _MK_RENDER_PRIORITY_BALANCED | POINT_COOKIE                         | MK_RENDER_PRIORITY_BALANCED
// _MK_NATURAL                  | SPOT                                 | MK_NATURAL         

//////////////////////////////////////////////////////
// Supported features based on shader model         //
//////////////////////////////////////////////////////
// 2.0  | Bloom, Lens Surface
// 2.5  | Bloom, Lens Surface
// 3.0  | Bloom, Lens Surface, Lens Flare
// 3.5  | Bloom, Lens Surface, Lens Flare
// 4.0  | Bloom, Lens Surface, Lens Flare, Glare, Geometry Shaders
// 4.5+ | Bloom, Lens Surface, Lens Flare, Glare, Geometry Shaders, Direct Compute

///////////////////////////////////
// Direct Compute Feature Matrix //
///////////////////////////////////
//   2x4  |   3x8	  |   4x16
//0	 --		  ---		  ----
//1	 +-		  +--		  +---
//2	 -+		  -+-		  -+--
//3	 ++		  ++-		  ++--
//4			  --+		  --+-
//5			  +++		  +++-
//6			  -++		  -++-
//7			  +-+		  +-+-
//8						  -+-+
//9 			  		  ---+
//10			  		  --++
//11					  -+++
//12				 	  ++-+
//13					  ++++
//14					  +-++
//15					  +--+

///////////////////////////////
//		CBuffer Inputs		 //
///////////////////////////////
// Index | Buffer | Size
// 0 | _BloomThreshold | 2
// 2 | _LumaScale | 1
// 3 | _BloomSpread | 1
// 4 | _BloomIntensity | 1
// 5 | _Blooming  | 1
// 6 | _LensSurfaceDirtIntensity | 1
// 7 | _LensSurfaceDiffractionIntensity | 1
// 8 | _LensFlareThreshold | 2
// 10 | _LensFlareGhostParams | 4
// 14 | _LensFlareHaloParams | 3
// 17 | _LensFlareSpread | 1
// 18 | _LensFlareChromaticAberration | 1
// 19 | _GlareThreshold | 2
// 21 | _GlareScattering | 4
// 25 | _GlareDirection01 | 4
// 29 | _GlareDirection23 | 4
// 33 | _GlareBlend | 1
// 34 | _GlareIntensity | 4
// 38 | _ResolutionScale | 2
// 40 | _GlareOffset | 4
// 44 | _LensSurfaceDirtTex_ST | 4
// 48 | _GlareGlobalIntensity | 1
// 49 | _ViewMatrix | 16
// 65

#ifndef MK_GLOW_COMMON
	#define MK_GLOW_COMMON

	#include "UnityCG.cginc"
	
	uniform half _SinglePassStereoScale;

	#ifdef UNITY_COLORSPACE_GAMMA
		#define COLORSPACE_GAMMA
	#endif

	#ifdef _COLOROVERLAY_ON
		#define MK_LEGACY_BLIT
	#endif	

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Shader Model dependent Macros
	/////////////////////////////////////////////////////////////////////////////////////////////
	#if SHADER_TARGET >= 35
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define UNIFORM_TEXTURE_2D(textureName) uniform Texture2DArray<half4> textureName;
			#define UNIFORM_SAMPLER_AND_TEXTURE_2D(textureName) uniform Texture2DArray<half4> textureName; uniform SamplerState sampler##textureName;
			#define DECLARE_TEXTURE_2D_ARGS(textureName, samplerName) Texture2DArray<half4> textureName, SamplerState samplerName
		#else
			#define UNIFORM_TEXTURE_2D(textureName) uniform Texture2D<half4> textureName;
			#define UNIFORM_SAMPLER_AND_TEXTURE_2D(textureName) uniform Texture2D<half4> textureName; uniform SamplerState sampler##textureName;
			#define DECLARE_TEXTURE_2D_ARGS(textureName, samplerName) Texture2D<half4> textureName, SamplerState samplerName
		#endif

		#define UNIFORM_TEXTURE_2D_NO_SCALE(textureName) uniform Texture2D<half4> textureName;
		#define UNIFORM_SAMPLER_AND_TEXTURE_2D_NO_SCALE(textureName) uniform Texture2D<half4> textureName; uniform SamplerState sampler##textureName;
		#define DECLARE_TEXTURE_2D_NO_SCALE_ARGS(textureName, samplerName) Texture2D<half4> textureName, SamplerState samplerName

		#define PASS_TEXTURE_2D(textureName, samplerName) textureName, samplerName
	#else
		#define UNIFORM_TEXTURE_2D(textureName) uniform sampler2D textureName;
		#define UNIFORM_SAMPLER_AND_TEXTURE_2D(textureName) uniform sampler2D textureName;
		#define DECLARE_TEXTURE_2D_ARGS(textureName, samplerName) sampler2D textureName

		#define UNIFORM_TEXTURE_2D_NO_SCALE(textureName) UNIFORM_TEXTURE_2D(textureName)
		#define UNIFORM_SAMPLER_AND_TEXTURE_2D_NO_SCALE(textureName) UNIFORM_SAMPLER_AND_TEXTURE_2D(textureName)
		#define DECLARE_TEXTURE_2D_NO_SCALE_ARGS(textureName, samplerName) DECLARE_TEXTURE_2D_ARGS(textureName, samplerName)

		#define PASS_TEXTURE_2D(textureName, samplerName) textureName
	#endif

	#define UNIFORM_SOURCE_SAMPLER_AND_TEXTURE(textureName) UNIFORM_SAMPLER_AND_TEXTURE_2D(textureName)
	#define DECLARE_SOURCE_TEXTURE_2D_ARGS(textureName, samplerName) DECLARE_TEXTURE_2D_ARGS(textureName, samplerName)
	#define PASS_SOURCE_TEXTURE_2D(textureName, samplerName) PASS_TEXTURE_2D(textureName, samplerName)

	#if UNITY_SINGLE_PASS_STEREO
		static const float4 _DEFAULT_SCALE_TRANSFORM = float4(0.5,1,0,0);
	#else
		static const float4 _DEFAULT_SCALE_TRANSFORM = float4(1,1,0,0);
	#endif

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Cross compile macros direct compute & shader
	/////////////////////////////////////////////////////////////////////////////////////////////
	//Other
	#define UV_COPY o.uv0.xy
	#define SOURCE_TEXEL_SIZE AutoScaleTexelSize(_SourceTex_TexelSize)
	#define COPY_RENDER_TARGET fO.GET_COPY_RT
	#define SOURCE_UV o.uv0.xy
	#define RETURN_TARGET_TEX return
	#define SAMPLE_SOURCE SampleSourceTex(PASS_SOURCE_TEXTURE_2D(_SourceTex, sampler_SourceTex), SOURCE_UV)
	#define UV_0 o.uv0.xy
	#define VIEW_MATRIX _ViewMatrix
	
	//Bloom
	#define BLOOM_UV o.uv0.xy
	#define BLOOM_RENDER_TARGET fO.GET_BLOOM_RT
	#define BLOOM_THRESHOLD _BloomThreshold
	#define BLOOM_TEXEL_SIZE AutoScaleTexelSize(_BloomTex_TexelSize)
	#define HIGHER_MIP_BLOOM_TEXEL_SIZE AutoScaleTexelSize(_HigherMipBloomTex_TexelSize)
	#define BLOOM_UPSAMPLE_SPREAD o.BLOOM_SPREAD
	#define BLOOM_COMPOSITE_SPREAD o.uv0.zw
	#define BLOOM_INTENSITY _BloomIntensity

	//Lens Surface
	#define LENS_SURFACE_DIRT_INTENSITY _LensSurfaceDirtIntensity
	#define LENS_SURFACE_DIFFRACTION_INTENSITY _LensSurfaceDiffractionIntensity
	#define LENS_SURFACE_DIRT_UV o.uv0.xy * _LensSurfaceDirtTex_ST.xy + _LensSurfaceDirtTex_ST.zw
	#define LENS_DIFFRACTION_UV o.MK_LENS_SURFACE_DIFFRACTION_UV

	//Lens Flare
	#define LENS_FLARE_UV o.uv0.xy
	#define LENS_FLARE_RENDER_TARGET fO.GET_LENS_FLARE_RT
	#define LENS_FLARE_THRESHOLD _LensFlareThreshold
	#define LENS_FLARE_GHOST_COUNT _LensFlareGhostParams.x
	#define LENS_FLARE_GHOST_DISPERSAL _LensFlareGhostParams.y
	#define LENS_FLARE_GHOST_FADE _LensFlareGhostParams.z
	#define LENS_FLARE_GHOST_INTENSITY _LensFlareGhostParams.w
	#define LENS_FLARE_HALO_SIZE _LensFlareHaloParams.x
	#define LENS_FLARE_HALO_FADE _LensFlareHaloParams.y
	#define LENS_FLARE_HALO_INTENSITY _LensFlareHaloParams.z
	#define LENS_FLARE_TEXEL_SIZE AutoScaleTexelSize(_LensFlareTex_TexelSize)
	#define LENS_FLARE_UPSAMPLE_SPREAD o.LENS_FLARE_SPREAD
	#define LENS_FLARE_CHROMATIC_ABERRATION o.LENS_FLARE_SPREAD

	//Glare
	#define GLARE_UV o.uv0.xy
	#define GLARE0_RENDER_TARGET fO.GET_GLARE0_RT
	#define GLARE_THRESHOLD _GlareThreshold
	#define GLARE0_SCATTERING _GlareScattering.x
	#define GLARE1_SCATTERING _GlareScattering.y
	#define GLARE2_SCATTERING _GlareScattering.z
	#define GLARE3_SCATTERING _GlareScattering.w
	#define GLARE0_DIRECTION _GlareDirection01.xy
	#define GLARE1_DIRECTION _GlareDirection01.zw
	#define GLARE2_DIRECTION _GlareDirection23.xy
	#define GLARE3_DIRECTION _GlareDirection23.zw
	#define GLARE0_OFFSET _GlareOffset.x
	#define GLARE1_OFFSET _GlareOffset.y
	#define GLARE2_OFFSET _GlareOffset.z
	#define GLARE3_OFFSET _GlareOffset.w
	#define GLARE1_RENDER_TARGET fO.GET_GLARE1_RT
	#define GLARE2_RENDER_TARGET fO.GET_GLARE2_RT
	#define GLARE3_RENDER_TARGET fO.GET_GLARE3_RT
	#define GLARE0_TEXEL_SIZE AutoScaleTexelSize(_Glare0Tex_TexelSize)
	#define GLARE_BLEND _GlareBlend
	#define GLARE0_INTENSITY _GlareIntensity.x
	#define GLARE1_INTENSITY _GlareIntensity.y
	#define GLARE2_INTENSITY _GlareIntensity.z
	#define GLARE3_INTENSITY _GlareIntensity.w
	#define GLARE0_TEX_TEXEL_SIZE AutoScaleTexelSize(_Glare0Tex_TexelSize)
	#define GLARE_GLOBAL_INTENSITY _GlareGlobalIntensity

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Features
	/////////////////////////////////////////////////////////////////////////////////////////////
	//Bloom
	#ifdef _NORMALMAP
		#define MK_BLOOM 1
		#define BLOOM_RT 0
	#endif

	#ifdef SPOT
		#define MK_NATURAL
	#endif

	//Copy
	#ifdef _PARALLAXMAP
		#define MK_COPY 1
		#define COPY_RT MK_BLOOM
	#endif

	//Lens Surface
	#ifdef _ALPHATEST_ON
		#define MK_LENS_SURFACE 1
	#endif

	//Debug Raw Bloom
	#ifdef _EMISSION
		#define MK_DEBUG_RAW_BLOOM
	#endif

	//Debug Bloom
	#ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
		#define MK_DEBUG_BLOOM
	#endif

	//Debug Composite
	#ifdef EDITOR_VISUALIZATION
		#define MK_DEBUG_COMPOSITE
	#endif

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Sampling
	/////////////////////////////////////////////////////////////////////////////////////////////
	static const half3 REL_LUMA = half3(0.2126h, 0.7152h, 0.0722h);
	#define PI 3.14159265
	#define EPSILON 1.0e-4

	inline half4 SampleTex2D(DECLARE_TEXTURE_2D_ARGS(tex, samplerTex), float2 uv)
	{
		#if SHADER_TARGET >= 35
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				return tex.SampleLevel(samplerTex, float3((uv).xy, (float)unity_StereoEyeIndex), 0);
			#else
				return tex.SampleLevel(samplerTex, UnityStereoTransformScreenSpaceTex(uv), 0);
			#endif
		#else
			return tex2D(tex, UnityStereoTransformScreenSpaceTex(uv));
		#endif
	}

	inline half4 SampleTex2DNoScale(DECLARE_TEXTURE_2D_NO_SCALE_ARGS(tex, samplerTex), float2 uv)
	{
		#if SHADER_TARGET >= 35
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				return tex.SampleLevel(samplerTex, float3(uv,0), 0);
			#else
				return tex.SampleLevel(samplerTex, uv, 0);
			#endif
		#else
			return tex2D(tex, uv);
		#endif
	}

	inline half4 SampleSourceTex(DECLARE_SOURCE_TEXTURE_2D_ARGS(tex, samplerTex), float2 uv)
	{
		return SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv);
	}

	inline float2 AutoScaleTexelSize(float2 texelSize)
	{
		texelSize.x *= _SinglePassStereoScale;
		return texelSize;
	}

	inline half4 ConvertToColorSpace(half4 color)
	{
		#ifdef COLORSPACE_GAMMA
			color.rgb = LinearToGammaSpace(color.rgb);
			return color;
		#else
			return color;
		#endif
	}

	inline half3 LuminanceThreshold(half3 c, half2 threshold)
	{		
		//brightness is defined by the relative luminance combined with the brightest color part to make it nicer to deal with the shader for artists
		//based on unity builtin brightpass thresholding
		//if any color part exceeds a value of 10 (builtin HDR max) then clamp it as a normalized vector to keep the color balance
		c = clamp(c, 0, normalize(c) * threshold.y);
		c *= 0.909;
		//half brightness = lerp(max(dot(c.r, REL_LUMA.r), max(dot(c.g, REL_LUMA.g), dot(c.b, REL_LUMA.b))), max(c.r, max(c.g, c.b)), REL_LUMA);
		//picking just the brightest color part isn´t physically correct at all, but gives nices artistic results
		half brightness = max(c.r, max(c.g, c.b));
		//forcing a hard threshold to only extract really bright parts
		half sP = EPSILON;//threshold.x * 0.0 + EPSILON;
		return max(0, c * max(pow(clamp(brightness - threshold.x + sP, 0, 2 * sP), 2) / (4 * sP + EPSILON), brightness - threshold.x) / max(brightness, EPSILON));
	}

	inline half4 GammaToLinearSpace4(half4 color)
	{
		color.rgb = GammaToLinearSpace(color.rgb);
		return color;
	}

	inline half4 LinearToGammaSpace4(half4 color)
	{
		color.rgb = LinearToGammaSpace(color.rgb);
		return color;
	}

	static const half2 DOWNSAMPLE_LQ_WEIGHT = half2(0.125, 0.03125);
	static const float4 DOWNSAMPLE_LQ_DIRECTION0 = float4(0.9, -0.9, 0.45, -0.45);
	static const float3 DOWNSAMPLE_LQ_DIRECTION1 = float3(0.9, 0.45, 0);
	//0 X 1 X 2
	//X 3 X 4 X
	//5 X 6 X 7
	//X 8 X 9 X
	//0 X 1 X 2
	inline half4 DownsampleLQ(DECLARE_TEXTURE_2D_ARGS(tex, samplerTex), float2 uv, float2 texelSize)
	{
		half3 sample0 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.yy).rgb;
		half3 sample1 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv - texelSize * DOWNSAMPLE_LQ_DIRECTION1.zx).rgb;
		half3 sample2 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.xy).rgb;
		half3 sample3 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.ww).rgb;
		half3 sample4 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.zw).rgb;
		half3 sample5 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv - texelSize * DOWNSAMPLE_LQ_DIRECTION1.xz).rgb;
		half3 sample6 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv).rgb;
		half3 sample7 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION1.xz).rgb;
		half3 sample8 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.wz).rgb;
		half3 sample9 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.zz).rgb;
		half3 sample10 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.yx).rgb;
		half3 sample11 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION1.zx).rgb;
		half3 sample12 = SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + texelSize * DOWNSAMPLE_LQ_DIRECTION0.xx).rgb;

		half4 o = half4((sample3 + sample4 + sample8 + sample9) * DOWNSAMPLE_LQ_WEIGHT.x, 1);
		o.rgb += (sample0 + sample1 + sample6 + sample5).rgb * DOWNSAMPLE_LQ_WEIGHT.y;
		o.rgb += (sample1 + sample2 + sample7 + sample6).rgb * DOWNSAMPLE_LQ_WEIGHT.y;
		o.rgb += (sample5 + sample6 + sample11 + sample10).rgb * DOWNSAMPLE_LQ_WEIGHT.y;
		o.rgb += (sample6 + sample7 + sample12 + sample11).rgb * DOWNSAMPLE_LQ_WEIGHT.y;

		return o;
	}

	static const half3 UPSAMPLE_LQ_WEIGHT = half3(0.25, 0.125, 0.0625);
	static const float3 UPSAMPLE_LQ_DIRECTION = float3(1, -1, 0);
	//012
	//345
	//678
	inline half4 UpsampleLQ(DECLARE_TEXTURE_2D_ARGS(tex, samplerTex), float2 uv, float2 texelSize)
	{
		half4 s = half4(0,0,0,1);
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv).rgb * UPSAMPLE_LQ_WEIGHT.x;

		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv - UPSAMPLE_LQ_DIRECTION.zx * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.y;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv - UPSAMPLE_LQ_DIRECTION.xz * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.y;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + UPSAMPLE_LQ_DIRECTION.xz * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.y;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + UPSAMPLE_LQ_DIRECTION.zx * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.y;

		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv - UPSAMPLE_LQ_DIRECTION.xx * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.z;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + UPSAMPLE_LQ_DIRECTION.xy * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.z;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + UPSAMPLE_LQ_DIRECTION.yx * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.z;
		s.rgb += SampleTex2D(PASS_TEXTURE_2D(tex, samplerTex), uv + UPSAMPLE_LQ_DIRECTION.xx * texelSize).rgb * UPSAMPLE_LQ_WEIGHT.z;

		return s;
	}

	#if defined(MK_LENS_SURFACE)
		uniform float4x4 _ViewMatrix;
		static half3x3 LensSurfaceDiffractionScale0 = half3x3
		(
			//X and Y of scale matrix has to be doubled to get correct pivot
			2, 0, -1,
			0, 2, -1,
			0, 0,  1
		);

		static half3x3 LensSurfaceDiffractionScale1 = half3x3
		(
			0.5, 0, 0.5,
			0, 0.5, 0.5,
			0, 0, 1
		);

		inline float2 LensSurfaceDiffractionUV(float2 uv)
		{
			float rotationView = dot(float3(VIEW_MATRIX._m00, VIEW_MATRIX._m10, VIEW_MATRIX._m20), float3(0,0,1)) + dot(float3(VIEW_MATRIX._m01, VIEW_MATRIX._m11, VIEW_MATRIX._m21), float3(0,1,0));
			float3x3 rotation = float3x3(
				cos(rotationView), -sin(rotationView), 0,
				sin(rotationView), cos(rotationView),  0,
				0, 0, 1
			);

			rotation = mul(mul(LensSurfaceDiffractionScale1, rotation), LensSurfaceDiffractionScale0);
			return mul(rotation, float3(uv, 1.0)).xy;
		}
	#endif

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Default Shader Includes
	/////////////////////////////////////////////////////////////////////////////////////////////
	const static float4 SCREEN_VERTICES[3] = 
	{
		float4(-1.0, -1.0, 0.0, 1.0),
		float4(3.0, -1.0, 0.0, 1.0),
		float4(-1.0, 3.0, 0.0, 1.0)
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Helpers
	/////////////////////////////////////////////////////////////////////////////////////////////
	inline float4 TransformMeshPos(float4 pos)
	{
		#ifdef MK_LEGACY_BLIT
			return UnityObjectToClipPos(pos);
		#else
			return float4(pos.xy, 0.0, 1.0);
		#endif
	}

	inline float2 SetMeshUV(float2 vertex)
	{
		float2 uv = (vertex + 1.0) * 0.5;
		#ifdef UNITY_UV_STARTS_AT_TOP
			uv = uv * float2(1.0, -1.0) + float2(0.0, 1.0);
		#endif
		return uv;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// In / Out Structs
	/////////////////////////////////////////////////////////////////////////////////////////////
	struct VertexInputOnlyPosition
	{
		float4 vertex : POSITION;
		#ifdef MK_LEGACY_BLIT
			float2 texcoord0 : TEXCOORD0;
		#endif
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct VertGeoOutputSimple
	{
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct VertGeoOutputAdvanced
	{
		float4 pos : SV_POSITION;
		float4 uv0 : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct VertGeoOutputPlus
	{
		float4 pos : SV_POSITION;
		float4 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	struct VertGeoOutputDouble
	{
		float4 pos : SV_POSITION;
		float4 uv0 : TEXCOORD0;
		float4 uv1 : TEXCOORD1;
		UNITY_VERTEX_OUTPUT_STEREO
	};

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Vertex
	/////////////////////////////////////////////////////////////////////////////////////////////
	VertGeoOutputSimple vertSimple (VertexInputOnlyPosition i0)
	{
		VertGeoOutputSimple o;

		UNITY_SETUP_INSTANCE_ID(i0);
		UNITY_INITIALIZE_OUTPUT(VertGeoOutputSimple, o);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		o.pos = TransformMeshPos(i0.vertex);
		o.uv0 = SetMeshUV(i0.vertex.xy);
		return o;
	}

	/////////////////////////////////////////////////////////////////////////////////////////////
	// Fragment Output
	/////////////////////////////////////////////////////////////////////////////////////////////
	#define COUNT_ENABLED_TARGETS MK_BLOOM + MK_COPY + MK_LENS_FLARE + MK_GLARE

	#define RENDER_TARGET(target) half4 rt##target : SV_Target##target;
	#define GET_RT(index) rt##index

	#if BLOOM_RT == 0
		#define GET_BLOOM_RT GET_RT(0)
	#endif
	
	struct FragmentOutputAuto
	{	
		RENDER_TARGET(0)
	};
#endif