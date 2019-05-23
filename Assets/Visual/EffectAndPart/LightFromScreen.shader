Shader "ACalm3D/LightFromScreen"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Noise("Noise ", 2D) = "white" {}

		_SourcePos("Source position", Vector) = (0, 0, 0, 1)
		_SourceSiz("Source range", float) = 4
		_SourceCol("Source color", Color) = (1,1,1,1)
		_SourceInt("Source intensity", Range(0,2)) = 1

			_Debug("Debug", float) = 1
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 250

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		sampler2D _Noise;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Noise;
			float3 worldPos;
			float3 worldNormal;
		};

		float3 _SourcePos;
		float _SourceSiz;
		half4 _SourceCol;
		float _SourceInt;

		float _Debug;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			float3 lightDir = float3(IN.worldPos.x - _SourcePos.x, IN.worldPos.y - _SourcePos.y, IN.worldPos.z - _SourcePos.z);
			half NdotL = clamp(dot(IN.worldNormal, lightDir),0,1);

			float distSqr = lightDir.x *  lightDir.x + lightDir.y *  lightDir.y + lightDir.z *  lightDir.z;

			half ratio = clamp(1 - (distSqr / (_SourceSiz*_SourceSiz)), 0, 1);

			half noiseVal = tex2D(_Noise, IN.uv_Noise).r;
			if (noiseVal < ratio * (1 - NdotL))
				noiseVal = 1;

			c.rgb = lerp(c.rgb, c.rgb + _SourceCol, ratio * (1 - NdotL) * noiseVal * _SourceInt);
			//c.rgb += _SourceCol * ratio * (1 - NdotL);

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

		FallBack "Mobile/Diffuse"
}
