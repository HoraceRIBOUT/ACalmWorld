Shader "ACalmPostProcess/Glitch-effect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_Amplitude("Amplitude of glitch", float) = 1
		_Period("Period of glitch", float) = 1

		_Noise("Noise", 2D) = "white" {}

		_Color("Color", Color) = (1,1,1,1)
		_OffsetBlueX("Decalage X du bleu", float) = 0.001
		_OffsetBlueY("Decalage Y du bleu", float) = 0.001
		
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

			sampler2D _MainTex;
			sampler2D _Noise;

			float _Amplitude;
			float _Period;
			
			float4 _Color;
			float _OffsetBlueX;
			float _OffsetBlueY;

			fixed4 frag(v2f i) : SV_Target
			{
				//I.UV PART :
				//DECALAGE PART
				float noise = tex2D(_Noise,i.uv).r;
				i.uv.x += sin(i.uv.y * _Period * _Time.y) * _Amplitude;

				i.uv = float2(frac(i.uv.x), frac(i.uv.y));
				//END DECALAGE PART

				//COLOR PART : 
				fixed4 col = tex2D(_MainTex, i.uv);
				//Final color tint
				col.rgb *= _Color.rgb;
				//END COLOR PART

				return col;
			}
			ENDCG
		}
		}



}
