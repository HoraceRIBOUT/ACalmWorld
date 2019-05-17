using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionIntr : MonoBehaviour
{
    public bool touch = false;
    private void OnMouseDown()
    {
        if (this.gameObject.activeSelf)
        {
            Touched();
        }
    }

    [ContextMenu("Touch")]
    private void Touched()
    {
        if (touch)
            return;
        Debug.Log("here go the transition !");
        Sound_Manager.instance.LaunchTransition();
        touch = true;
    }
}
