// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ACalm3D/water"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_AmplitudeInX("Amplitude in X", float) = 1
		_AmplitudeInY("Amplitude in Y", float) = 1
		_PeriodeInX("Periode in X", float) = 1
		_PeriodeInY("Periode in Y", float) = 1
		_TimeInfluence("TimeInfluence", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float val : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			float4 _Color;

			float _AmplitudeInX;
			float _AmplitudeInY;

			float _PeriodeInX;
			float _PeriodeInY;

			float _TimeInfluence;


            v2f vert (appdata v)
            {
                v2f o;

				float val = 0;
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				val = cos(worldPos.x * _PeriodeInX) * _AmplitudeInX + sin(worldPos.z * _PeriodeInY) * _AmplitudeInY;
				v.vertex.y += sin(val * _Time.x * _TimeInfluence);

				

                o.vertex = UnityObjectToClipPos(v.vertex);


				o.val = sin(val * _Time.x);
				//o.vertex.z += sin(.x);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float NdotL = ((i.val) + 2) / 3;
				fixed4 col = _Color;
				col.g *= 1-NdotL;
				//col = fixed4(frac(i.val), 0, 0, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
