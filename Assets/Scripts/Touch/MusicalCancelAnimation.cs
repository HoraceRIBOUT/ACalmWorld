using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalCancelAnimation : Animated
{

    public bool canDoIt = true;
    public float seuil = -14;

    protected override void ChangeOnUpdate()
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
