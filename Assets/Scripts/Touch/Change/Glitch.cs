using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Change
{
    public GameObject objectToChange;
    public int layerWhenClick;

    public float duration = 0.3f;
    public float intensity = 0.5f;
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
        GameManager.instance.glitchHandler.IWannaGlitch(intensity);
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
        GameObject tmp_obj = objectToChange;
        if (tmp_obj == null)
            tmp_obj = this.gameObject;

        int layerValue = tmp_obj.layer;
        tmp_obj.layer = layerWhenClick;
        layerWhenClick = layerValue;
    }
}
