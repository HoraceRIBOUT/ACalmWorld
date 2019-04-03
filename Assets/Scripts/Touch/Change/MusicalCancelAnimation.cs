using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalCancelAnimation : Animated
{
    public bool canDoIt = true;
    public List<float> seuilPerState = new List<float>();

    public override void ChangeOnUpdate(float rtpcValue)
    {
        if (canDoIt)
        {
            if(rtpcValue > seuilPerState[state])
            {
                canDoIt = false;
                ActionAtSeuil();
            }
        }
        else
        {
            if (rtpcValue < seuilPerState[state])
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
