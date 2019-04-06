using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    [System.Serializable]
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
        public float tramIntensity = 0;
        public Color tramColor = Color.white;

        public VHSShaderValue()
        {
            blurIntensity = 0;
            blurBlueIntensity = 0;
            //Couleur
            tint = Color.white;
            decalageBleu = Vector2.zero;
            saturation = 1;
            noirEtBlanc = 0;
            //Bug
            tailleBug = 0;
            decalageDansLeBug = 0;
            verticalGlitch = false;
            vitesseBug = 0;
            //Noise
            tolerance = 0;
            noiseColor = Color.black;
            //Tram
            tramFrac = 1;
            tramRythm = -2;
            tramIntensity = 0;
            tramColor = Color.white;
        }
    }


    public Material matToApply;
    public RenderTexture texturesGlitch;
    
    public List<VHSShaderValue> targetEffect = new List<VHSShaderValue>();
    [Range(0,1)]
    public float lerpValue = 0.2f;

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
            if (targetEffect.Count > 1)
            {
                VHSShaderValue val = Lerp(targetEffect[0], targetEffect[1], lerpValue);
                ApplyVHSValueOnMat(val, matToApply);
            }

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
    }



    public VHSShaderValue Lerp(VHSShaderValue val1, VHSShaderValue val2, float lerp)
    {
        VHSShaderValue res = new VHSShaderValue();

        //Blur
        res.blurIntensity = Mathf.Lerp(val1.blurIntensity, val2.blurIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        //Color
        res.tint = Color.Lerp(val1.tint, val2.tint, lerp);
        res.decalageBleu = Vector2.Lerp(val1.decalageBleu, val2.decalageBleu, lerp);
        res.saturation = Mathf.Lerp(val1.saturation, val2.saturation, lerp);
        res.noirEtBlanc = Mathf.Lerp(val1.noirEtBlanc, val2.noirEtBlanc, lerp);
        //Bug
        res.tailleBug = Mathf.Lerp(val1.tailleBug, val2.tailleBug, lerp);
        res.decalageDansLeBug = Mathf.Lerp(val1.decalageDansLeBug, val2.decalageDansLeBug, lerp);
        res.verticalGlitch = val1.verticalGlitch || val2.verticalGlitch;
        res.vitesseBug = Mathf.Lerp(val1.vitesseBug, val2.vitesseBug, lerp);
        //Noise
        res.tolerance = Mathf.Lerp(val1.tolerance, val2.tolerance, lerp);
        res.noiseColor = Color.Lerp(val1.noiseColor, val2.noiseColor, lerp);
        //Tram
        res.tramFrac = Mathf.Lerp(val1.tramFrac, val2.tramFrac, lerp);
        res.tramRythm = Mathf.Lerp(val1.tramRythm, val2.tramRythm, lerp);
        res.tramIntensity = Mathf.Lerp(val1.tramIntensity, val2.tramIntensity, lerp);
        res.tramColor = Color.Lerp(val1.tramColor, val2.tramColor, lerp);

        return res;
    }

    public void ApplyVHSValueOnMat(VHSShaderValue val, Material mat)
    {
        mat.SetFloat("_Blur", val.blurIntensity);
        mat.SetFloat("_BlurForBlue", val.blurBlueIntensity);

        mat.SetColor("_Color", val.tint);
        mat.SetVector("_OffsetBlue", val.decalageBleu);
        mat.SetFloat("_Saturation", val.saturation);
        mat.SetFloat("_NbIntensity", val.noirEtBlanc);

        mat.SetFloat("_Taille", val.tailleBug);
        mat.SetFloat("_Decalage", val.decalageDansLeBug);
        mat.SetFloat("_typeOfBug", val.verticalGlitch ? 1 : 0);
        mat.SetFloat("_Speed", val.blurIntensity);

        mat.SetFloat("_ToleranceBWNoise", val.tolerance);
        mat.SetColor("_NoiseColor", val.noiseColor);

        mat.SetFloat("_Tramage", val.tramFrac);
        mat.SetFloat("_Tram2", val.tramRythm);
        mat.SetFloat("_TramIntensity", val.tramIntensity);
        mat.SetColor("_TramColor", val.tramColor);
    }
}
