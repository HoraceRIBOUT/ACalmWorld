using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated : Change
{
    public Animator animator;

    public override void ChangeOnStart()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        animator.SetTrigger("Click");
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {
        //nothing
    }
}
