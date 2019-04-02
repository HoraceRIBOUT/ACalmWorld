using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMove : Animated
{
    public int currentLayer = 0;

    [System.Serializable]
    public class PosRotSca
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public List<PosRotSca> posRotScaForEachState;
    public Transform transfToRotate;
    public List<Collider2D> collForEachState = new List<Collider2D>();

    
    public override void ChangeOnClick(int currentState, bool on)
    {
        animator.SetLayerWeight(currentLayer, 0);
        currentLayer++;
        if (currentLayer >= animator.layerCount)
            currentLayer = 0;
        animator.SetLayerWeight(currentLayer, 1);

        //Move
        this.transform.localPosition = posRotScaForEachState[currentState].position;
        transfToRotate.transform.localRotation = Quaternion.Euler(posRotScaForEachState[currentState].rotation);
        this.transform.localScale = posRotScaForEachState[currentState].scale;

        //Collision
        if (currentState == 0)
            collForEachState[collForEachState.Count - 1].enabled = false;
        else
            collForEachState[currentState - 1].enabled = false;
        collForEachState[currentState].enabled = true;
    }


}
