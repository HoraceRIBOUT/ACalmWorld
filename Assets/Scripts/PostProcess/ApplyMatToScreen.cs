using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyMatToScreen : MonoBehaviour
{
    public Material matToApply;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
#if !UNITY_EDITOR
        //This is a delay for when the shader cannot load (it's then backbuffer to the previous (so last) Render. 
        if(Time.fixedTime > 3f) {
#endif
        if (matToApply != null)
        {
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
