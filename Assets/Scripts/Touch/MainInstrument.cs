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
    [SerializeField] public AK.Wwise.Event nameEventUnMute;
    [SerializeField] public AK.Wwise.Event nameEventMute;
    [SerializeField] public List<AK.Wwise.Switch> switches;
    [SerializeField] public AK.Wwise.RTPC rtpcId;

    public GameObject sound_manager;
    [HideInInspector] public int indexForSoundManager;

    [Header("Change")]
    public List<Change> changeCmpt = new List<Change>();

    public void Start()
    {
        sound_manager = Sound_Manager.instance.gameObject;

        if (changeCmpt.Count == 0)
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
            AkSoundEngine.PostEvent(nameEventUnMute.Id, sound_manager);
            //Debug.Log("Unmute " + nameEventUnMute.Id);
        }
        else if (currentState == switches.Count)
        {
            on = false;
            currentState = 0;
            AkSoundEngine.PostEvent(nameEventMute.Id, sound_manager);
            //Debug.Log("Mute " + nameEventMute.Id);
        }

        if(switches.Count != 0)
        {
            AkSoundEngine.SetSwitch(switches[currentState].GroupId, switches[currentState].Id, sound_manager);
            currentState++;
        }
        else
            Debug.Log("Error : did not have any state ", sound_manager);

        ChangeOnClick();
    }

    public void GetRTPCValue()
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(rtpcId.Id, sound_manager, 0, out rtpcValue, ref type);
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
