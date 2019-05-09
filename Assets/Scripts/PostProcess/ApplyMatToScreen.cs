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
        public Vector4 decalageBleu = Vector4.zero;
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
            decalageBleu = Vector4.zero;
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
    public AnimationCurve forTramIntensityTransition = AnimationCurve.Linear(0, 0, 1, 1);
    [Range(0, 1)]
    public List<float> lerpForTarget = new List<float>();

    public int resolutionDivision = 2;
    private Vector2 size;



    public void Awake()
    {
        CreateRenderTexture();
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
        GetComponentInChildren<ApplyGlitch>().GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;

        CreateLerpValue();
    }

    public void CreateLerpValue()
    {
        lerpForTarget.Clear();
        foreach (VHSShaderValue vs in targetEffect)
        {
            lerpForTarget.Add(0);
        }
        lerpForTarget[0] = 0.2f;
    }

    public void CreateRenderTexture()
    {
        RenderTexture rT = new RenderTexture((int)(Camera.main.pixelWidth / resolutionDivision),
                                                         (int)(Camera.main.pixelHeight / resolutionDivision), texturesGlitch.depth);
        size.x = texturesGlitch.width;
        size.y = texturesGlitch.height;
        rT.name = "TextSize" + rT.width + "_" + rT.height;
        texturesGlitch = rT;
        GetComponentInChildren<ApplyGlitch>().GetComponent<Camera>().targetTexture = texturesGlitch;
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    private void Update()
    {
        //Test screen size : if it change --> change the texteures Glitch size
        if (size.x != (int)(Camera.main.pixelWidth / resolutionDivision) || size.y != (int)(Camera.main.pixelHeight / resolutionDivision))
        {
            Debug.Log("Resize");
            //Create new buffer and delete old one ?
            CreateRenderTexture();
        }

        if(targetEffect.Count != lerpForTarget.Count)
        {
            CreateLerpValue();
        }
    }
#endif

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
#if !UNITY_EDITOR
        //This is a delay for when the shader cannot load (it's then backbuffer to the previous (so last) Render. 
        if(Time.fixedTime > 1f  || !Debug.isDebugBuild) {
#endif
        if (matToApply != null)
        {
#if !UNITY_EDITOR

        //This is a delay for the RenderTexture, to test if the shader don't work or if the Render Text don't work
        if(Time.fixedTime > 2f || !Debug.isDebugBuild) {
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
                VHSShaderValue val = GetCurrentVHSEffect();
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

    public VHSShaderValue GetCurrentVHSEffect()
    {
        VHSShaderValue res = Lerp(targetEffect[0], targetEffect[1], (lerpForTarget[0] + lerpForTarget[1]));
        for (int i = 2; i < targetEffect.Count; i++)
        {
            if(lerpForTarget[i] != 0)
                res = Lerp(res, targetEffect[i], lerpForTarget[i]);
        }
        return res;
    }

    public VHSShaderValue Lerp(VHSShaderValue val1, VHSShaderValue val2, float lerp)
    {
        VHSShaderValue res = new VHSShaderValue();

        //Blur
        res.blurIntensity = Mathf.Lerp(val1.blurIntensity, val2.blurIntensity, lerp);
        res.blurBlueIntensity = Mathf.Lerp(val1.blurBlueIntensity, val2.blurBlueIntensity, lerp);
        //Color
        res.tint = Color.Lerp(val1.tint, val2.tint, lerp);
        res.decalageBleu = Vector4.Lerp(val1.decalageBleu, val2.decalageBleu, lerp);
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
        res.tramIntensity = Mathf.Lerp(val1.tramIntensity, val2.tramIntensity, forTramIntensityTransition.Evaluate(lerp));
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
        mat.SetFloat("_Speed", val.vitesseBug);

        mat.SetFloat("_ToleranceBWNoise", val.tolerance);
        mat.SetColor("_NoiseColor", val.noiseColor);

        mat.SetFloat("_Tramage", val.tramFrac);
        mat.SetFloat("_Tram2", val.tramRythm);
        mat.SetFloat("_TramIntensity", val.tramIntensity);
        mat.SetColor("_TramColor", val.tramColor);
    }

    public void OnEachBeat()
    {
        Vector4 vec = targetEffect[1].decalageBleu;
        vec.z = Random.Range(-1.0f,1.0f);
        vec.w = Random.Range(-1.0f, 1.0f);
        targetEffect[1].decalageBleu = vec;
        print("Update !");
    }
    public void OnEachBar()
    {
        //Do something
    }
}
