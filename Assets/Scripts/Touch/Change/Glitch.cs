using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Change
{
    public GameObject objectToChange;
    public int layerWhenClick;

    public float duration = 0.3f;
    private float timer = 0.0f;

    public override void ChangeOnStart()
    {
       //Do Nothing
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        if(timer == 0)
            InverseGlitch();
        //launch a timer to go back after some times. If so, re set the timer if click again
        ApplyGlitch.instance.IWannaGlitch(0.8f);
        timer = duration;
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        if(timer != 0)
        {
            //set timer
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                InverseGlitch();
                timer = 0;
            }
        }
    }

    private void InverseGlitch()
    {
        if (objectToChange == null)
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
    }
}
