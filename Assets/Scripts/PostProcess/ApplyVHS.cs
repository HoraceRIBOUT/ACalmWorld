using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyVHS : MonoBehaviour
{
    public Material matToApply;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (matToApply != null)
        {
            RenderTexture tmp = destination;
            Graphics.Blit(source, tmp, matToApply);
            Graphics.Blit(tmp, destination);
        }
    }
}
