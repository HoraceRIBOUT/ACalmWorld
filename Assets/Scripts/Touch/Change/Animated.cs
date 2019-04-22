using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated : Change
{
    public Animator animator;
    protected int state;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mainInstrument = mI;
    }

    public virtual void ChangeOnStart()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
    }

    public virtual void ChangeOnClick()
    {
        animator.SetTrigger("Click");
        state = mainInstrument.instruData.currentState - 1;
    }
}
