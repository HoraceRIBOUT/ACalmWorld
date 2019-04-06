Shader "CustomLy/MirorDepht" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Depth("Depth", Range(0,1)) = 0

	}
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					o.uv2 = v.texcoord;
					return o;
				}

				sampler2D _MainTex;
				sampler2D _CameraDepthTexture;

				float _Depth;


				fixed4 frag(v2f i) : SV_Target
				{
					i.uv = float2(frac(i.uv.x), frac(i.uv.y));
					fixed depthR = Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r);
					fixed depthL = Linear01Depth(tex2D(_CameraDepthTexture, half2(1 - i.uv.x, i.uv.y)).r);

					depthR = lerp(i.uv.x, depthR, _Depth);
					depthL = lerp(0.5f, depthL, _Depth);

					if(depthR > depthL)
						i.uv.x = 1 - i.uv.x;

					

					fixed4 col = tex2D(_MainTex, i.uv);



					return col;
				}

				ENDCG
			}
		}
}
