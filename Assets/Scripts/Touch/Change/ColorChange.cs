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

    public bool debug = false;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
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
    
    public void ChangeOnUpdate()
    {
        ChangeColor(debug ? (mainInstrument.instruData.currentState != 0 ? 0 : -48) : mainInstrument.instruData.rtpcValue);
    }

    public void ChangeColor(float rtpcValue)
    {
        float valueZerOne = (rtpcValue + 48) / 48;

        valueZerOne = colorIntensityFunction.Evaluate(valueZerOne);
        //change the color to the right one

        int colState = mainInstrument.instruData.currentState - 1;
        if (colState < 0)
            colState = 0;
        if (spriteRdr != null)
        {
            spriteRdr.color = Color.Lerp(startColor, changeToColor[colState], valueZerOne);
        }
        else if (meshRdr != null)
        {
            meshRdr.material.color = Color.Lerp(startColor, changeToColor[colState], valueZerOne);
        }
    }
}
