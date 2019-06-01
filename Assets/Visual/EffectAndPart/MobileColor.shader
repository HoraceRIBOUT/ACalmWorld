Shader "ACalm3D/MobileColor"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
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
		
		half4 _Color;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			//o.Normal = IN.worldNormal;
		}
		ENDCG
		}

			FallBack "Mobile/Diffuse"
}
