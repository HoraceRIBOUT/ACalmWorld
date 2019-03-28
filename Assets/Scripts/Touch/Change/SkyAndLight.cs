using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAndLight : Animated
{
    public Material skybox;
    public Light ambientLight;

    public Color skyboxGround = Color.white;
    public Color skyboxSky = Color.white;
    public Color ambient = Color.white;
    
    public override void ChangeOnUpdate(float rtpcValue)
    {
        skybox.SetColor("_SkyTint", skyboxSky);
        skybox.SetColor("_GroundColor", skyboxGround);
        RenderSettings.skybox = skybox;
        ambientLight.color = ambient;
        RenderSettings.ambientLight = ambient;
    }

}
