using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : TouchOnOff
{

    public override void Touched(bool on)
    {
        if (on)
        {
            AkSoundEngine.PostEvent("UnMute_ACW_Lofi_Musique1_Percus", gameObject);
        }

        else
        {
            AkSoundEngine.PostEvent("Mute_ACW_Lofi_Musique_Percus_01_Lp", gameObject);
        }

    }

    public void Update(){
        int type = 1;
        AkSoundEngine.GetRTPCValue(2181839183U, gameObject, 0, out ColorIntensity, ref type);

        base.ChangeColor();
    }
}
