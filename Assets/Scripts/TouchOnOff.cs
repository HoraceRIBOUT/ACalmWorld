using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchOnOff : MonoBehaviour
{
    public bool on = false;
    public Color changeToColor = Color.green;

    private void OnMouseDown()
    {
        //reverse the on/off
        on = !on;
        //change the color to the right one
        this.GetComponent<MeshRenderer>().material.color = on? changeToColor : Color.white;
        //call the function of the child 
        Touched(on);
    }

    public abstract void Touched(bool on);
}
