using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyGlitch : MonoBehaviour
{
    public bool on = false;
    private List<GameObject> objectInGlitchMode;
    private List<int> objectLayerInt;


    public Material matToApply;

    //[Range(0,1)]
    public float _trauma;
    public float periodValue = 0.1f;
    public AnimationCurve periodOverTrauma = AnimationCurve.Linear(0, 0, 1, 1);
    public float amplitudeValue = 0.1f;
    public AnimationCurve amplitudeOverTrauma = AnimationCurve.Linear(0, 0.001f, 1, 0.02f);

    public void IWannaGlitch(float traumaValue)
    {
        _trauma += traumaValue;
        CheckTrauma();
    }

    public void PutObjectInRender(List<GameObject> interacObj)
    {
        objectInGlitchMode = new List<GameObject>();
        objectLayerInt = new List<int>();
        foreach (GameObject gO in interacObj)
        {
            objectInGlitchMode.Add(gO);
            objectLayerInt.Add(gO.layer);
            gO.layer = 9;
        }
        _trauma = 2;
        on = false;
    }
    public void ReleaseThem()
    {
        for (int i = 0; i < objectInGlitchMode.Count; i++)
        {
            objectInGlitchMode[i].layer = objectLayerInt[i];
        }
    }

    public void Update()
    {
        if(_trauma > 0 )
        {
            if(on)
                _trauma -= Time.deltaTime;
            CheckTrauma();
        }
    }

    private void CheckTrauma()
    {
        if (_trauma > 1)
            _trauma = 1;
        else if (_trauma < 0)
            _trauma = 0;

        periodValue = periodOverTrauma.Evaluate(_trauma);
        amplitudeValue = amplitudeOverTrauma.Evaluate(_trauma);
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
#if !UNITY_EDITOR
        //This is a delay for when the shader cannot load (it's then backbuffer to the previous (so last) Render. 
        if(Time.fixedTime > 3f) {
#endif
        if (matToApply != null)
        {
            RenderTexture tmp = destination;
            matToApply.SetFloat("_Period", periodValue);
            matToApply.SetFloat("_Amplitude", amplitudeValue);
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
