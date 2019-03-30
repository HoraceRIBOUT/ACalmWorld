using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAndLight : Animated
{
    public Material skybox;
    public Light ambientLight;

    public Color skyboxSky = Color.white;
    public Color skyboxEquator = Color.white;
    public Color skyboxGround = Color.white;
    public Color lightColor = Color.white;

    
#if UNITY_EDITOR
    private void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            ChangeOnUpdate(0.0f);
            Debug.Log("Change validate");
        }
    }
#endif 
    

    public override void ChangeOnUpdate(float rtpcValue)
    {
        skybox.SetColor("_SkyTint", skyboxSky);
        skybox.SetColor("_GroundColor", skyboxGround);
        RenderSettings.skybox = skybox;
        ambientLight.color = lightColor;
        RenderSettings.ambientSkyColor = skyboxSky;
        RenderSettings.ambientEquatorColor = skyboxEquator;
        RenderSettings.ambientGroundColor = skyboxGround;
    }
}
