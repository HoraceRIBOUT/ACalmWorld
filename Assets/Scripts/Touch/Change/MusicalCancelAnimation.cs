﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalCancelAnimation : Animated
{

    public bool canDoIt = true;
    public float seuil = -14;

    public override void ChangeOnUpdate(float rtpcValue)
    {
        if (canDoIt)
        {
            if(rtpcValue > seuil)
            {
                canDoIt = false;
                ActionAtSeuil();
            }
        }
        else
        {
            if (rtpcValue < seuil)
            {
                canDoIt = true;
                ActionAfterSeuil();
            }
        }
    }

    protected void ActionAtSeuil()
    {
        animator.SetTrigger("Loop");
    }


    protected void ActionAfterSeuil()
    {
        //nothing
    }

}