Shader "APlaceHolder/LocalBug" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Color("Color", Color) = (1,1,1,1)
		_OffsetGreen("Decalage du vert", float) = 0.001
		_saturation("Saturation", Range(0,2)) = 0.7
		_nbIntensity("NoirEtBlanc", Range(0,2)) = 0.5
		_Hauteur("Hauteur du bug", Range(0,1)) = 0
		_Taille("Taille du bug", Range(0,1)) = 0
		_Decalage("Decalage dans le bug", float) = 0.0001
		_Speed("Vitesse du bug", float) = 1
		_typeOfBug("0 mean only decalage", float) = 0

		_numberHole("Number of Hole", float) = 1
		_sizeVerticalH("Height of the hole", float) = 0.02
		_sizeHorizontH("Width of the hole", float) = 0.06
		_debugPTX("_debugPTX", float) = 0.1
		_debugPTY("_debugPTY", float) = 0.1
		//_debugTry("_debugTry", float) = 0.1
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

	//Color
	float _OffsetGreen;
	float _saturation;
	float _nbIntensity;

	//Hole
	float _numberHole;
	half _sizeVerticalH;
	half _sizeHorizontH;
	half _debugPTX;
	half _debugPTY;
	//float _debugTry;

	//Bug large
	half _Hauteur;
	half _Taille;
	float _Decalage;
	float _Speed;
	//Type du bug
	float _typeOfBug;

	fixed4 frag(v2f i) : SV_Target
	{
		//BUG PART
		i.uv = float2(frac(i.uv.x), frac(i.uv.y));

		half positionLow = _Hauteur + _Time * _Speed;
		half positionUp = positionLow + _Taille;
		if (positionUp > 1)
			positionUp = frac(positionUp);
		if (positionLow > 1)
			positionLow = frac(positionLow);

		if (positionUp > positionLow) {
			if (i.uv.y > positionLow && i.uv.y < positionUp) {
				if (_typeOfBug != 0)
					i.uv.y = positionLow;
				i.uv.x += _Decalage;
			}
		}
		else {
			if (i.uv.y > positionLow || i.uv.y < positionUp) {
				if (_typeOfBug != 0)
					i.uv.y = positionLow;
				i.uv.x += _Decalage;
			}
		}
		//END DECALAGE PART

		fixed4 colOriginal = tex2D(_MainTex, i.uv);
		fixed4 colGreenDec = tex2D(_MainTex, i.uv);
		fixed4 colNoiEtBla = tex2D(_MainTex, i.uv);
		fixed4 colResultat = tex2D(_MainTex, i.uv);

		//Decal Green;
		colGreenDec.r *= _saturation;
		colGreenDec.g = tex2D(_MainTex, i.uv + _OffsetGreen).g * _saturation;
		colGreenDec.b *= _saturation;

		fixed4 greyValue = (colOriginal.r + colOriginal.g + colOriginal.b) / 3;
		colNoiEtBla.r = greyValue;
		colNoiEtBla.g = greyValue;
		colNoiEtBla.b = greyValue;

		colResultat = lerp(colGreenDec, colNoiEtBla, _nbIntensity);
		fixed4 colBis = tex2D(_MainTex, i.uv);


		//HOLE PART
		float trueOrNot = 0;

		for (int numeroOfHole = 1; numeroOfHole < _numberHole; numeroOfHole++) {
			//float debugPTX = rand(0, 1 - _sizeHorizontH, (numeroOfHole+1) * _Time[2]);
			//float debugPTY = rand(0, 1 - _sizeVerticalH, (numeroOfHole+1) * _Time[1]);
			float debugPTX = frac(_debugPTX  * pow(10, -(numeroOfHole + 1)) * (_Time.z*0.1));// *pow(10, numeroOfHole + 1));// *  frac(_debugTry * pow(10, numeroOfHole + 1));// *Time
			float debugPTY = frac(_debugPTY * pow(10, -(numeroOfHole + 1)) * (_Time.y*0.11));// * pow(10, numeroOfHole + 1));// *  frac(_debugTry * pow(10, numeroOfHole + 1));// *Time

			if (i.uv.x > debugPTX && i.uv.x < debugPTX + _sizeHorizontH
				&& i.uv.y > debugPTY && i.uv.y < debugPTY + _sizeVerticalH)
				colResultat = half4 (0, 0, 0, 1);
			else
				trueOrNot = 1;
		}
		if (!(colResultat.r == 0 && colResultat.g == 0 && colResultat.b == 0 && colResultat.a == 1)) {
			colBis = half4 (0, 0, 0, 1);
		}

		//END HOLE PART

		//WHITE NOISE PART
		/*
		if (abs(frac(i.uv.y + _Time.x) - 0.3f) < 0.001f && abs(i.uv.x%(0.1789 * sin(_Time.x))) > 0.15f * sin(_Time.x))
		colBis.rgb = float3(1, 1, 1);
		*/
		//END WHITE NOISE PART

		colResultat += colBis;

		return colResultat;
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}
