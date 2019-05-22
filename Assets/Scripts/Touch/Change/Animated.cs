using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animated : Change
{
    public Animator animator;
    public int currentLayer = 0;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onSloMoEvent.AddListener(ChangeOnSlowMo);
        mainInstrument = mI;
    }

    public virtual void ChangeOnStart()
    {
        if (animator == null)
            animator = this.GetComponent<Animator>();
    }

    public virtual void ChangeOnClick()
    {
        animator.SetLayerWeight(currentLayer, 0);
        currentLayer++;
        if (currentLayer >= animator.layerCount && currentLayer >= mainInstrument.instruData.currentState)
            currentLayer = 0;
        animator.SetLayerWeight(currentLayer, 1);
    }

    public void ChangeOnSlowMo()
    {
        animator.speed = GameManager.instance.transitionOnSlowMo;
    }
}
