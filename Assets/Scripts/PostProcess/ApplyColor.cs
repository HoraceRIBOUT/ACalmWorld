using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColor : ApplyMatToScreen
{
    public static ApplyColor instance;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
           //  Destroy(this);  //because lol it's just a debug !
        }
    }


    public void ChangeColorOfScreen(Color value)
    {
        matToApply.SetColor("_Color", value);
    }
}
