using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToMainInst : MonoBehaviour
{
    public MainInstrument mainInst;

    private void OnMouseDown()
    {
        if (mainInst == null)
            mainInst = GetComponentInParent<MainInstrument>();

        //call the function of the child 
        mainInst.Touched();

    }
}
