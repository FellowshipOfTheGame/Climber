// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Crystal" {
	Properties {
		_SolidMin("Solid Min", Float) = 0.0
		_CrystalMin("Crystal Min", Float) = 0.0
		_Max("Max", Float) = 1.0
		_Bottom ("Bottom Color", Color) = (1,1,1,1)
		_Top ("Top Color", Color) = (1,1,1,1)
		_Noise ("Noise", 2D) = "white" {}
		_NoiseScale ("Noise Scale", Range(0,1)) = 0.25
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Crystalization ("Crystalization", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0


		struct Input {
			float3 worldPos;
			float2 uv_Noise;
		};

		float _SolidMin;
		float _CrystalMin;

		float _Max;

		float _Crystalization;

		sampler2D _Noise;
		float _NoiseScale;

		float _Glossiness;
		float _Metallic;

		fixed4 _Bottom;
		fixed4 _Top;

		void surf (Input input, inout SurfaceOutputStandard output) {
			
			fixed3 localPos =  mul(unity_WorldToObject, float4(input.worldPos, 1.0)).xyz;

			fixed4 noise = tex2D(_Noise, input.uv_Noise);

			float _Min = lerp(_SolidMin, _CrystalMin, _Crystalization);

			float posNormalized = (localPos.z - _Min) / (_Max - _Min);

			float t = posNormalized + _NoiseScale * (noise.r - 0.5) * 2.0 * _Crystalization;
			t = clamp(t, 0, 1);


			float4 color = lerp(_Bottom, _Top, t);

			output.Metallic = _Metallic;
			output.Smoothness = _Glossiness;

			output.Albedo = color.rgb;
			output.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
