using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionIntr : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (this.gameObject.activeSelf)
        {
            Touched();
        }
    }

    private void Touched()
    {
        Debug.Log("here go the transition !");
        Sound_Manager.instance.LaunchTransition();
    }
}
