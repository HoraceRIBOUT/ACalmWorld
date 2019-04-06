using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpriteLight : Animated
{
    public SpriteRenderer skySprite;
    public Light ambientLight;

    public Color skyColor= Color.white;
    public Color upLight = Color.white;
    public Color averageLight = Color.white;
    public Color downLight = Color.white;
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
        skySprite.color = skyColor;
        ambientLight.color = lightColor;
        RenderSettings.ambientSkyColor = upLight;
        RenderSettings.ambientEquatorColor = averageLight;
        RenderSettings.ambientGroundColor = lightColor;
    }
}
