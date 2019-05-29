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
        //mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mainInstrument = mI;
    }

    public void ChangeOnClick()
    {
        matSource.ChangeColor(changeToColor[mainInstrument.instruData.currentState]);
        matSource.ChangeRange(changeToRange[mainInstrument.instruData.currentState]);
    }

    /*public void ChangeOnUpdate()
    {
    Color col = ;
        matSource.ChangeColor(col);
    }*/


    public void OnDestroy()
    {
        matSource.ChangeColor(changeToColor[0]);
        matSource.ChangeRange(changeToRange[0]);

    }
}
