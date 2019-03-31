using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    public Material matToApply;
    public RenderTexture texturesGlitch;

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

}
