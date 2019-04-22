using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalCancelAnimation : Animated
{
    public bool canDoIt = true;
    public List<float> seuilPerState = new List<float>();

    public override void AddEventOnListener(MainInstrument mI)
    {
        base.AddEventOnListener(mI);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
    }

    public void ChangeOnUpdate()
    {
        if (canDoIt)
        {
            if(mainInstrument.instruData.rtpcValue > seuilPerState[state])
            {
                canDoIt = false;
                ActionAtSeuil();
            }
        }
        else
        {
            if (mainInstrument.instruData.rtpcValue < seuilPerState[state])
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
