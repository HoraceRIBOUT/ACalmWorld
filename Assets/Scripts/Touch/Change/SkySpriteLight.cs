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
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public override void ChangeOnStart()
    {
        skySprites = skyFolder.GetComponentsInChildren<SpriteRenderer>(true);
        base.ChangeOnStart();
    }

    public override void ChangeOnClick()
    {

        //transformer ça en transition via un lerp qui va s'echanger
        animator.SetLayerWeight(currentLayer, 0);
        currentLayer++;
        if (currentLayer >= animator.layerCount || mainInstrument.instruData.currentState == 0)
            currentLayer = 0;
        animator.SetLayerWeight(currentLayer, 1);
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
