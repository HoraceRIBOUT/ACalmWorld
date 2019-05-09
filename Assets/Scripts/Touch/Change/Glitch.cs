using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glitch : Change
{
    public List<GameObject> objectToChange = new List<GameObject>();
    public int layerWhenClick = 9;
    public int layerWhenDefault = 0;

    public float duration = 0.3f;
    public float intensity = 0.5f;
    private float timer = 0.0f;

    public bool glitchOn = false;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public void ChangeOnStart()
    {
        if (objectToChange.Count == 0)
            objectToChange.Add(this.gameObject);
    }

    public void ChangeOnClick()
    {
        glitchOn = true;
        ReSetGlitch();
        //launch a timer to go back after some times. If so, re set the timer if click again
        GameManager.instance.glitchHandler.IWannaGlitch(intensity);
        timer = duration;
    }

    //TO DO : handle all  glitch timer on one script only (manager ?) 
    //To kept : - if you click again, it reset the timer
    public void ChangeOnUpdate()
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
        int goodLayer = glitchOn ? layerWhenClick : layerWhenDefault;

        foreach (GameObject glitchObj in objectToChange)
        {
            glitchObj.layer = goodLayer;
        }
    }
}
