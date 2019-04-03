using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    public class VHSShaderValue
    {
        public float blurIntensity = 0;
        public float blurBlueIntensity = 0;
        //Couleur
        public Color tint = Color.white;
        public Vector2 decalageBleu = Vector2.zero;
        public float saturation = 1;
        public float noirEtBlanc = 0;
        //Bug
        public float tailleBug = 0;
        public float decalageDansLeBug = 0;
        public bool verticalGlitch = false;
        public float vitesseBug = 0;
        //Noise
        public float tolerance = 0;
        public Color noiseColor = Color.black;
        //Tram
        public float tramFrac = 1;
        public float tramRythm = -2;
        public float itramIntensity = 0;
        public Color tramColor = Color.white;
    }
    public Material matToApply;
    public RenderTexture texturesGlitch;

    public List<VHSShaderValue> targetEffect;



    public int resolutionDivision = 2;

    public void Awake()
    {
        texturesGlitch.width = (int)(Screen.currentResolution.width / resolutionDivision);
        texturesGlitch.height = (int)(Screen.currentResolution.height / resolutionDivision);
    }



    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
#if !UNITY_EDITOR
        //This is a delay for when the shader cannot load (it's then backbuffer to the previous (so last) Render. 
        if(Time.fixedTime > 3f) {
#endif
        if (matToApply != null)
        {
#if !UNITY_EDITOR
        //This is a delay for the RenderTexture, to test if the shader don't work or if the Render Text don't work
        if(Time.fixedTime > 6f) {
#endif
            if (texturesGlitch != null)
            {
                matToApply.SetTexture("_TextureGlitch", texturesGlitch);
            }
#if !UNITY_EDITOR
        }
#endif
            RenderTexture tmp = destination;
            Graphics.Blit(source, tmp, matToApply);
            Graphics.Blit(tmp, destination);
        }
        else
            Graphics.Blit(source, destination);
#if !UNITY_EDITOR
        }
        else
            Graphics.Blit(source, destination);
#endif


        /* 
        if (matToUse != null && readyToFuse)
        {
            matToUse.SetTexture("_MainTex", source);
            for (int i = 0; i < NBR_CAMERA; i++)
            {
                matToUse.SetTexture("_Texture0" + (i), textures[i]);
                matToUse.SetTexture("_Depture0" + (i), dephtTextures[i]);
            }
            Graphics.Blit(source, matToUse);
        }
        else
            Graphics.Blit(source, destination);
        */
    }


    public VHSShaderValue Lerp(VHSShaderValue val1, VHSShaderValue val2, float lerp)
    {
        VHSShaderValue res = new VHSShaderValue();
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        res.blurIntensity = Mathf.Lerp(val1.blurIntensity, val2.blurIntensity, lerp);
        res.decalageBleu = Vector2.Lerp(val1.decalageBleu, val2.decalageBleu, lerp);
        res.decalageDansLeBug = Mathf.Lerp(val1.decalageDansLeBug, val2.decalageDansLeBug, lerp);
        res.itramIntensity = Mathf.Lerp(val1.itramIntensity, val2.itramIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);//TO DO


        return res;
    }

    public void ApplyVHSValueOnMat(VHSShaderValue val, Material mat)
    {
        //TO DO
    }

}
