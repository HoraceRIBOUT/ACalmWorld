using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyColor : ApplyMatToScreen
{
    public void ChangeColorOfScreen(Color value)
    {
        matToApply.SetColor("_Color", value);
    }
}
