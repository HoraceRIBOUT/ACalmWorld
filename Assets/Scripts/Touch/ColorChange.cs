using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MainInstrument
{
    public Color changeToColor = Color.green;
    public SpriteRenderer spriteRdr;
    public MeshRenderer meshRdr;
    private Color startColor;

    public void Start()
    {
        if (spriteRdr != null)
        {
            startColor = spriteRdr.color;
        }
        else if (meshRdr != null)
        {
            startColor = meshRdr.material.color;
        }
    }

    protected void ChangeColor()
    {
        float valueZerOne = (rtpcValue + 48) / 48;
        //change the color to the right one
        if (spriteRdr != null)
        {
            spriteRdr.color = startColor + changeToColor * valueZerOne;

        }
        else if (meshRdr != null)
        {
            meshRdr.material.color = startColor + changeToColor * valueZerOne;

        }
    }

    protected override void ChangeOnClick()
    {
        //nothing
    }

    protected override void ChangeOnUpdate()
    {
        ChangeColor();
    }
}
