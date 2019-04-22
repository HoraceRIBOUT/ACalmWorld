using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpriteLight : Animated
{
    public Transform skyFolder;
    private SpriteRenderer[] skySprites;
    public Light ambientLight;

    public Color skyColor= Color.white;
    public Color upLight = Color.white;
    public Color averageLight = Color.white;
    public Color downLight = Color.white;
    public Color lightColor = Color.white;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(base.ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public override void ChangeOnStart()
    {
        skySprites = skyFolder.GetComponentsInChildren<SpriteRenderer>(true);
        base.ChangeOnStart();
    }

    public void ChangeOnUpdate()
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
