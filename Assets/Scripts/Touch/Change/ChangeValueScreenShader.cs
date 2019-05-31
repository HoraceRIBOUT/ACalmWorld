using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValueScreenShader : Change
{
    public SourceOfMat matSource;

    public List<Color> changeToColor = new List<Color>();
    public List<float> changeToRange = new List<float>();

    //rtpc ?

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public void ChangeOnClick()
    {
        matSource.ChangeColor(changeToColor[mainInstrument.instruData.currentState]);
        matSource.ChangeRange(changeToRange[mainInstrument.instruData.currentState]);
    }

    public void ChangeOnUpdate()
    {
        float valueZerOne = (mainInstrument.instruData.rtpcValue + 48) / 48;
        
        matSource.ChangeRange(changeToRange[mainInstrument.instruData.currentState] * (1 + valueZerOne * GameManager.instance.transitionOnSlowMo * 0.5f) );
    }


    public void OnDestroy()
    {
        matSource.ChangeColor(changeToColor[0]);
        matSource.ChangeRange(changeToRange[0]);

    }
}
