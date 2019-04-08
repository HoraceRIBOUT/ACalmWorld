using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpriteLight : Animated
{
    public Transform skySprite;
    private SpriteRenderer[] skySprites;
    public Light ambientLight;

    public Color skyColor= Color.white;
    public Color upLight = Color.white;
    public Color averageLight = Color.white;
    public Color downLight = Color.white;
    public Color lightColor = Color.white;

    public override void ChangeOnStart()
    {
        skySprites = skySprite.GetComponentsInChildren<SpriteRenderer>();
        base.ChangeOnStart();
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        foreach (SpriteRenderer sprRdr in skySprites)
        {
            sprRdr.color = skyColor;
        }
        ambientLight.color = lightColor;
        RenderSettings.ambientSkyColor = upLight;
        RenderSettings.ambientEquatorColor = averageLight;
        RenderSettings.ambientGroundColor = downLight;
    }
}
