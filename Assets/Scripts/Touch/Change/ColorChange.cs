using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : Change
{
    public List<Color> changeToColor = new List<Color>();
    public AnimationCurve colorIntensityFunction = AnimationCurve.Linear(0,0,1,1);
    public SpriteRenderer spriteRdr;
    public MeshRenderer meshRdr;
    private Color startColor;
    private int state = 0;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public void ChangeOnStart()
    {
        if (spriteRdr != null)
        {
            startColor = spriteRdr.color;
        }
        else if (meshRdr != null)
        {
            startColor = meshRdr.material.color;
        }
    }

    public void ChangeOnClick()
    {
        state = mainInstrument.instruData.currentState - 1;
    }

    public void ChangeOnUpdate()
    {
        if (state >= 0)
            ChangeColor(mainInstrument.instruData.rtpcValue);
    }

    public void ChangeColor(float rtpcValue)
    {
        float valueZerOne = (rtpcValue + 48) / 48;
        valueZerOne = colorIntensityFunction.Evaluate(valueZerOne);
        //change the color to the right one
        if (spriteRdr != null)
        {
            spriteRdr.color = Color.Lerp(startColor, changeToColor[state], valueZerOne);

        }
        else if (meshRdr != null)
        {
            meshRdr.material.color = Color.Lerp(startColor, changeToColor[state], valueZerOne);
        }
    }
}
