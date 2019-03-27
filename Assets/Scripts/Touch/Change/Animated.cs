using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated : Change
{
    public Animator animator;
    protected int state;

    public override void ChangeOnStart()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        animator.SetTrigger("Click");
        state = currentState - 1;
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        //nothing
    }
}
