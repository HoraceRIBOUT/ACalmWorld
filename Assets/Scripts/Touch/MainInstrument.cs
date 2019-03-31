using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInstrument : MonoBehaviour
{
    public Sound_Manager sound_manager;
    [HideInInspector] public int indexForSoundManager;

    [Header("Change")]
    public List<Change> changeCmpt = new List<Change>();

    public void Start()
    {
        sound_manager = Sound_Manager.instance;

        if (changeCmpt.Count == 0)
        {
            foreach(Change ch in GetComponentsInChildren<Change>())
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
        UpdateRTPCValue();

        ChangeOnUpdate();
    }
    
    public void Touched()
    {
        Sound_Manager.InstruData instruData = sound_manager.getData(indexForSoundManager);
        if (!instruData.on)
        {
            instruData.on = true;
            instruData.currentState = 0;
            AkSoundEngine.PostEvent(instruData.nameEventUnMute.Id, sound_manager.gameObject);
            //Debug.Log("Unmute " + nameEventUnMute.Id);
        }
        else if (instruData.currentState == instruData.switches.Count)
        {
            instruData.on = false;
            instruData.currentState = 0;
            AkSoundEngine.PostEvent(instruData.nameEventMute.Id, sound_manager.gameObject);
            //Debug.Log("Mute " + nameEventMute.Id);
        }

        if(instruData.switches.Count != 0)
        {
            AkSoundEngine.SetSwitch(instruData.switches[instruData.currentState].GroupId, instruData.switches[instruData.currentState].Id, sound_manager.gameObject);
            instruData.currentState++;
        }
        else
            Debug.Log("Error : did not have any state ", sound_manager.gameObject);

        ChangeOnClick();
    }

    public void UpdateRTPCValue()
    {
        sound_manager.UpdateRTPCValue(indexForSoundManager);
    }

    protected void ChangeOnStart()
    {
        Sound_Manager.InstruData instruData = sound_manager.getData(indexForSoundManager);
        foreach (Change ch in changeCmpt)
            ch.ChangeOnStart();
    }
    protected void ChangeOnClick()
    {
        Sound_Manager.InstruData instruData = sound_manager.getData(indexForSoundManager);
        foreach (Change ch in changeCmpt)
            ch.ChangeOnClick(instruData.currentState, instruData.on);
    }
    protected void ChangeOnUpdate()
    {
        Sound_Manager.InstruData instruData = sound_manager.getData(indexForSoundManager);
        foreach (Change ch in changeCmpt)
            ch.ChangeOnUpdate(instruData.rtpcValue);
    }

}
