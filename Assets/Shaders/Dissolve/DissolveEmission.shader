Shader "DissolverShader/DissolveShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_NormalStrenght ("Normal Strength", Range(0, 1.5)) = 0.5
		_DissolveMap ("Dissolve Map", 2D) = "white" {}
		_DissolveAmount ("DissolveAmount", Range(0,1)) = 0
		_DissolveColor ("DissolveColor", Color) = (1,1,1,1)
		_DissolveEmission ("DissolveEmission", Range(0,1)) = 1
		_DissolveWidth ("DissolveWidth", Range(0,0.1)) = 0.05
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalMap;
		sampler2D _DissolveMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv_DissolveMap;
		};

		half _DissolveAmount;
		half _NormalStrenght;
		half _Glossiness;
		half _Metallic;
		half _DissolveEmission;
		half _DissolveWidth;
		fixed4 _Color;
		fixed4 _DissolveColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;			
			fixed4 mask = tex2D (_DissolveMap, IN.uv_DissolveMap);

			if(mask.r < _DissolveAmount)
				discard;

			o.Albedo = c.rgb;

			if(mask.r < _DissolveAmount + _DissolveWidth) {
				o.Albedo = _DissolveColor;
				o.Emission = _DissolveColor * _DissolveEmission;
			}
			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Normal = UnpackScaleNormal(tex2D(_NormalMap, IN.uv_NormalMap), _NormalStrenght);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
