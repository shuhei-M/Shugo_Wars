// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Disolve"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_DisolveTex("DisolveTex (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}
		//_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		//_OcclusionMap("Occlusion", 2D) = "white" {}
		_Threshold("Threshold", Range(0,1)) = 0.0
	}
	SubShader
	{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _DisolveTex;

			struct Input {
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			half _Threshold;
			fixed4 _Color;
			fixed4 _MetallicGlossMap;
			fixed4 _OcclusionMap;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 m = tex2D(_DisolveTex, IN.uv_MainTex);
				half g = m.r * 0.2 + m.g * 0.7 + m.b * 0.1;
				if (g < _Threshold) {
					discard;
				}

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _MetallicGlossMap * _Metallic;
				o.Smoothness = _Glossiness;
				//o.Occlusion =  _OcclusionMap /** _OcclusionStrength*/;
				o.Alpha = c.a;
			}
			ENDCG
	}
	FallBack "Diffuse"
}
