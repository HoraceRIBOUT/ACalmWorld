Shader "ACalm3D/Hair"
{
	Properties{
		_Blend("Blend", Range(0,1)) = 0.5
		_MainTex("Texture 1", 2D) = ""
	}

		Category{
			SubShader {
				Pass {
		Cull Off
		//Lighting On
		AlphaTest Greater[_Blend]
		SetTexture[_MainTex] {
			ConstantColor(0,0,0,[_Blend])
			Combine texture Lerp(constant alpha) previous alpha
		}
	}
}

// pre-3GS devices, including the September 2009 8GB iPod touch
SubShader {
	Pass {
		AlphaTest Greater[_Blend]
		SetTexture[_MainTex]
		SetTexture[_Texture2] {
			ConstantColor(0,0,0,[_Blend])
			Combine texture Lerp(constant alpha) previous alpha
		}
		SetTexture[_MainTex] {
			Combine texture * previous alpha
		}
		SetTexture[_MainTex] {
			Combine texture * previous alpha
		}
	}
	Pass {
		//Lighting On
		Blend DstColor SrcColor
	}
}
	}

}