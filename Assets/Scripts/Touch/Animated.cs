using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated : MainInstrument
{
    public Animator animator;

    public void Start()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
    }

    protected override void ChangeOnClick()
    {
        animator.SetTrigger("Click");
    }

    protected override void ChangeOnUpdate()
    {
        //nothing
    }
}
