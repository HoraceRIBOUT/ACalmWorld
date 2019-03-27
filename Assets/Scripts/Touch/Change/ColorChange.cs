using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : Change
{
    public Color changeToColor = Color.green;
    public AnimationCurve colorIntensityFunction = AnimationCurve.Linear(0,0,1,1);
    public SpriteRenderer spriteRdr;
    public MeshRenderer meshRdr;
    private Color startColor;

    public override void ChangeOnStart()
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

    public override void ChangeOnClick(int currentState, bool on)
    {
        //nothing
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        ChangeColor(rtpcValue);
    }

    public void ChangeColor(float rtpcValue)
    {
        float valueZerOne = (rtpcValue + 48) / 48;
        valueZerOne = colorIntensityFunction.Evaluate(valueZerOne);
        //change the color to the right one
        if (spriteRdr != null)
        {
            spriteRdr.color = startColor + changeToColor * valueZerOne;

        }
        else if (meshRdr != null)
        {
            meshRdr.material.color = startColor + changeToColor * valueZerOne;
        }
    }
}
