using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainInstrument : MonoBehaviour
{
    [Header("State data")]
    public bool on = false;
    public int currentState = 0;
    public float rtpcValue = -48;

    [Header("Wwise info")]
    public AK.Wwise.Event nameEventUnMute;
    public AK.Wwise.Event nameEventMute;
    public AK.Wwise.Switch switchGroup;
    public List<AK.Wwise.State> states;
    public AK.Wwise.RTPC rtpcId;

    //public int maxSize = 3;

    private void OnMouseDown()
    {
        //call the function of the child 
        Touched();
    }

    public void Update()
    {
        GetRTPCValue();

        ChangeOnUpdate();
    }
    
    public void Touched()
    {
        if (!on)
        {
            on = true;
            currentState = 0;
            AkSoundEngine.PostEvent(nameEventUnMute.Id, gameObject);
            Debug.Log("Unmute " + nameEventUnMute.Id);
        }
        else if (currentState == states.Count)
        {
            on = false;
            currentState = 0;
            AkSoundEngine.PostEvent(nameEventMute.Id, gameObject);
            Debug.Log("Mute " + nameEventMute.Id);
        }

        if(states.Count != 0 && switchGroup != null)
        {
            AkSoundEngine.SetSwitch(switchGroup.Id, states[currentState].Id, this.gameObject);
            currentState++;
        }
        else
            Debug.Log(switchGroup == null ? "Error : did not have switch group " : "Error : did not have any state ", this.gameObject);

        ChangeOnClick();
    }

    public void GetRTPCValue()
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(rtpcId.Id, gameObject, 0, out rtpcValue, ref type);
    }

    protected abstract void ChangeOnClick();
    protected abstract void ChangeOnUpdate();

}
