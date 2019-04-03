// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "ACalm3D/Rim"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimLimit("Rim Limit", Range(0,1)) = 0.5
		_RimPower("Rim Power", Range(0,8.0)) = 1.0
		_Source("Source position", Vector) = (0,0,0)
		_Rayon("Rayon", float) = 1
		_SphereForm("Sphere scale", Vector) = (1,1,1)
		_BlackNoiseTex("Texture", 2D) = "white" {}
		_NoiseEffect("Noise Effect", Range(0,1)) = 0.4
		_RearEffect("Rear Effect", Range(0,1)) = 0.4
    }


	SubShader{

	  Tags { "RenderType" = "Opaque" }

	  CGPROGRAM

	  #pragma surface surf Lambert vertex:vert
	  #pragma target 3.0
	  struct Input {
		  float2 uv_MainTex;
		  float3 customColor;
		  float3 vectorFromSource;
		  float distanceFromSource;
	  };

	  float3 _Source;
	  float _Rayon;
	  float3 _SphereForm;

	  void vert(inout appdata_full v, out Input o) {
		  UNITY_INITIALIZE_OUTPUT(Input,o);
		  o.customColor = -(v.normal);
		  //float3 dist = (mul(unity_ObjectToWorld, v.vertex) - _Source);
		  //float distance = dist.x * dist.x + dist.y * dist.y + dist.z * dist.z;
		  o.vectorFromSource = (mul(unity_ObjectToWorld, v.vertex) - _Source);
		  float distance = o.vectorFromSource.x * o.vectorFromSource.x * 1/_SphereForm.x + o.vectorFromSource.y * o.vectorFromSource.y * 1 / _SphereForm.y + o.vectorFromSource.z * o.vectorFromSource.z * 1 /_SphereForm.z;
		  o.distanceFromSource = clamp(1 + (-(distance) / (_Rayon * _Rayon)), 0, 1);

	  }

	  sampler2D _MainTex;
	  float4 _RimColor;
	  float _RimLimit;
	  float _RimPower;
	  sampler2D _BlackNoiseTex;
	  float _NoiseEffect;
	  float _RearEffect;

	  void surf(Input IN, inout SurfaceOutput o) {
		  //float3 viewDir = float3(0,1,0);

		  o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		  o.Normal = IN.customColor;
		  half rim = IN.distanceFromSource * _RimPower;
		  float4 colBW = tex2D(_BlackNoiseTex, IN.uv_MainTex);
		  colBW = max(colBW - _NoiseEffect, 0);
		  float blackNoiseVal = 1 - (max(sign(colBW - rim), 0));

		  half near = clamp(dot(normalize(IN.vectorFromSource), o.Normal), 0,1);
		  //rim *= o.;
		  //rim = clamp((rim - _RimLimit) * 2, 0, 1);
		  float3 val = _RimColor.rgb * rim * blackNoiseVal;
		  o.Emission = lerp(val * 0.5, (val * near), _RearEffect);
	  }
	  ENDCG
	}
}
