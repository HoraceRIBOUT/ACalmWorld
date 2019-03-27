using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInstrument : MonoBehaviour
{
    [Header("State data")]
    public bool on = false;
    public int currentState = 0;
    public float rtpcValue = -48;

    [Header("Wwise info")]
    [SerializeField] private AK.Wwise.Event nameEventUnMute;
    [SerializeField] private AK.Wwise.Event nameEventMute;
    [SerializeField] private AK.Wwise.Switch switchGroup;
    [SerializeField] private List<AK.Wwise.State> states;
    [SerializeField] private AK.Wwise.RTPC rtpcId;

    [Header("Change")]
    public List<Change> changeCmpt = new List<Change>();

    public void Start()
    {
        if(changeCmpt.Count == 0)
        {
            foreach(Change ch in GetComponents<Change>())
            {
                changeCmpt.Add(ch);
            }
        }


        ChangeOnStart();
    }

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
            //Debug.Log("Unmute " + nameEventUnMute.Id);
        }
        else if (currentState == states.Count)
        {
            on = false;
            currentState = 0;
            AkSoundEngine.PostEvent(nameEventMute.Id, gameObject);
            //Debug.Log("Mute " + nameEventMute.Id);
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

    protected void ChangeOnStart()
    {
        foreach (Change ch in changeCmpt)
            ch.ChangeOnStart();
    }
    protected void ChangeOnClick()
    {
        foreach (Change ch in changeCmpt)
            ch.ChangeOnClick(currentState, on);
    }
    protected void ChangeOnUpdate()
    {
        foreach (Change ch in changeCmpt)
            ch.ChangeOnUpdate(rtpcValue);
    }

}
