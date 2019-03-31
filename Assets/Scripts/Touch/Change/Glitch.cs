using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Change
{
    public GameObject objectToChange;
    public int layerWhenClick;

    public override void ChangeOnStart()
    {
       //Do Nothing
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        if(objectToChange == null)
        {
            int layerValue = gameObject.layer;
            gameObject.layer = layerWhenClick;
            layerWhenClick = layerValue;
        }
        else
        {
            int layerValue = objectToChange.layer;
            objectToChange.layer = layerWhenClick;
            layerWhenClick = layerValue;
        }
        //launch a timer to go back after some times. If so, re set the timer if click again
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        //nothing
    }
}
