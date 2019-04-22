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
    public List<GameObject> collForEachState = new List<GameObject>();

    public override void ChangeOnClick(int currentState, bool on)
    {
        animator.SetLayerWeight(currentLayer, 0);
        currentLayer++;
        if (currentLayer >= animator.layerCount)
            currentLayer = 0;
        animator.SetLayerWeight(currentLayer, 1);

        //Move
        this.transform.localPosition = posRotScaForEachState[currentLayer].position;
        transfToRotate.transform.localRotation = Quaternion.Euler(posRotScaForEachState[currentLayer].rotation);
        this.transform.localScale = posRotScaForEachState[currentLayer].scale;

        //Collision
        if (currentLayer == 0)
            collForEachState[collForEachState.Count - 1].SetActive(false);
        else
            collForEachState[currentLayer - 1].SetActive(false);
        collForEachState[currentLayer].SetActive(true);
    }


}
