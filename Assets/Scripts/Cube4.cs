using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube4 : TouchOnOff
{

    public override void Touched(bool on)
    {
        if (on)
        {
            AkSoundEngine.PostEvent("UnMute_ACW_Lofi_Musique1_Synth_Mid", gameObject);
        }

        else
        {
            AkSoundEngine.PostEvent("Mute_ACW_Lofi_Musique1_Synth_Mid", gameObject);
        }
    }
    public void Update()
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(3115982782U, gameObject, 0, out ColorIntensity, ref type);

        base.ChangeColor();
    }
}