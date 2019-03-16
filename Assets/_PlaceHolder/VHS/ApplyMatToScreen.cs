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
    }
}
