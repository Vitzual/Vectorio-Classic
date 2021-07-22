//////////////////////////////////////////////////////
// MK Glow Composite Sample							//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.           
 //
//////////////////////////////////////////////////////
#ifndef MK_GLOW_COMPOSITE_SAMPLE
	#define MK_GLOW_COMPOSITE_SAMPLE
	
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(o);
	
	FragmentOutputAuto fO;
	UNITY_INITIALIZE_OUTPUT(FragmentOutputAuto, fO);

	half4 g = SampleTex2D(PASS_TEXTURE_2D(_BloomTex, sampler_BloomTex), UV_0);

	#ifdef COLORSPACE_GAMMA
		half3 source = GammaToLinearSpace(SAMPLE_SOURCE.rgb).rgb;
	#else
		half3 source = SAMPLE_SOURCE.rgb;
	#endif

	#ifdef MK_GLOW_DEBUG
		source = 0;
	#endif

	g.rgb *= BLOOM_INTENSITY;

	#ifdef MK_LENS_SURFACE
		half3 dirt = SampleTex2DNoScale(PASS_TEXTURE_2D(_LensSurfaceDirtTex, sampler_LensSurfaceDirtTex), LENS_SURFACE_DIRT_UV).rgb;
		half3 diffraction = SampleTex2DNoScale(PASS_TEXTURE_2D(_LensSurfaceDiffractionTex, sampler_LensSurfaceDiffractionTex), LENS_DIFFRACTION_UV).rgb;

		#ifdef COLORSPACE_GAMMA
			dirt = GammaToLinearSpace(dirt);
			diffraction = GammaToLinearSpace(diffraction);
		#endif

		dirt *= LENS_SURFACE_DIRT_INTENSITY;
		diffraction *= LENS_SURFACE_DIFFRACTION_INTENSITY;
		g.rgb = lerp(g.rgb * 3, g.rgb + g.rgb * dirt + g.rgb * diffraction, 0.5) * 0.3333h;
	#endif
	
	//When using gamma space at least try to get a nice looking result by adding the glow in the linear space of the source even if the base color space is gamma
	#ifdef MK_GLOW_COMPOSITE
		#ifdef COLORSPACE_GAMMA
			#ifndef MK_NATURAL
				g.rgb += source.rgb;
			#endif
			RETURN_TARGET_TEX ConvertToColorSpace(g);
		#else
			#ifndef MK_NATURAL
				g.rgb += source.rgb;
			#endif
			RETURN_TARGET_TEX g;
		#endif
	#else
		RETURN_TARGET_TEX ConvertToColorSpace(g);
	#endif
#endif