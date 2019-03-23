using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSource : MonoBehaviour
{
    [Header("Material affected")]
    public List<SkinnedMeshRenderer> meshToModify;
    public List<Material> matToModify;
    public bool modifMat = false;

    [Header("Light Value")]
    private Vector3 lastPosition;
    public Color lightColor = Color.white;
    private Color lastColor;
    public float intensity = 1;
    private float lastIntensity;
    public float rayon = 1;
    private float lastRayon;
    [Range(0,1)]
    public float noiseIntensity = 1;
    private float lastNoiseIntens;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastIntensity != intensity || lastRayon != rayon || lastNoiseIntens != noiseIntensity || lastPosition != this.transform.position || lightColor != lastColor)
        {
            lastPosition = this.transform.position;
            lastColor = lightColor;
            lastIntensity = intensity;
            lastRayon = rayon;
            lastNoiseIntens = noiseIntensity;
            Vector4 source = new Vector4(lastPosition.x, lastPosition.y, lastPosition.z, 0);
            if (modifMat)
            {
                ModifyMat(source);
            }
            else
            {
                ModifyMat(source);
            }
            
        }
    }

    void ModifyMat(Vector4 source)
    {
            foreach (Material mat in matToModify)
            {
                mat.SetVector("_Source", source);
                mat.SetVector("_RimColor", lightColor);
                mat.SetFloat("_RimPower", intensity);
                mat.SetFloat("_Rayon", rayon);
                mat.SetFloat("_NoiseEffect", noiseIntensity);
            }
    }

    void ModifyMesh(Vector4 source)
    {
        foreach (SkinnedMeshRenderer mes in meshToModify)
        {
            foreach (Material mat in mes.materials)
            {
                mat.SetVector("_Source", source);
                mat.SetVector("_RimColor", lightColor);
                mat.SetFloat("_RimPower", intensity);
                mat.SetFloat("_Rayon", rayon);
                mat.SetFloat("_NoiseEffect", noiseIntensity);
            }
        }
    }
}
