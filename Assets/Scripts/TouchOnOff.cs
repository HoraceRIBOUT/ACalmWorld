using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchOnOff : MonoBehaviour
{
    public bool on = false;
    public Color changeToColor = Color.green;
    public float ColorIntensity = -48;

    private void OnMouseDown()
    {
        //reverse the on/off
        on = !on;
        //call the function of the child 
        Touched(on);
    }

    protected void ChangeColor()
    {
        float valueZerOne = (ColorIntensity + 48) / 48;
        //change the color to the right one
        this.GetComponent<MeshRenderer>().material.color = Color.white + changeToColor * valueZerOne;
    }

    public abstract void Touched(bool on);
}
