﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Change
{
    public List<GameObject> objectToChange = new List<GameObject>();
    public int layerWhenClick = 9;
    private int layerWhenDefault = 0;

    public float duration = 0.3f;
    public float intensity = 0.5f;
    private float timer = 0.0f;

    public bool glitchOn = false;

    public override void ChangeOnStart()
    {
        if (objectToChange.Count == 0)
            objectToChange.Add(this.gameObject);
        layerWhenDefault = objectToChange[0].layer;
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        glitchOn = true;
        ReSetGlitch();
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
                glitchOn = false;
                ReSetGlitch();
                timer = 0;
            }
        }
    }

    private void ReSetGlitch()
    {
        foreach(GameObject glitchObj in objectToChange)
        {
            if (glitchOn)
                glitchObj.layer = layerWhenClick;
            else
                glitchObj.layer = layerWhenDefault;
        }
    }
}
