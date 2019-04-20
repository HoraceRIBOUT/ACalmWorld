using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyVHS : MonoBehaviour
{
    public Material matToApply;
    public AnimationCurve xPos;
    public AnimationCurve yPos;

    public float speed = 0.1f;

#if !UNITY_ANDROID
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (matToApply != null)
        {
            RenderTexture tmp = destination;
            Graphics.Blit(source, tmp, matToApply);
            Graphics.Blit(tmp, destination);
        }
    }

    public void Update()
    {
        Vector4 vec = Vector4.zero;
        vec.x = xPos.Evaluate((Time.timeSinceLevelLoad * speed) % 1) * Random.Range(0.6f, 1f);
        vec.y = yPos.Evaluate((Time.timeSinceLevelLoad * speed) % 1) * Random.Range(0.6f, 1f);
        vec.z = Random.Range(0f, 1f);
        vec.w = Random.Range(0f, 1f);
        matToApply.SetVector("_OffsetBlue", vec);
    }
#endif
}
