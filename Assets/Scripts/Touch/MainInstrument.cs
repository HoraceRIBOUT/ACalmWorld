using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainInstrument : MonoBehaviour
{
    public Sound_Manager sound_manager;
    [HideInInspector] public int indexForSoundManager;
    
    [HideInInspector] public UnityEvent onStartEvent;
    [HideInInspector] public UnityEvent onClickEvent;
    [HideInInspector] public UnityEvent onUpdatEvent;
    [HideInInspector] public UnityEvent onSloMoEvent;

    [HideInInspector]public Sound_Manager.InstruData instruData;

    public bool active = true;

    public void Start()
    {
        sound_manager = Sound_Manager.instance;

        foreach (Change ch in GetComponentsInChildren<Change>(true))
        {
            ch.AddEventOnListener(this);
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
        if (!GameManager.instance.pause)
        {
            UpdateRTPCValue();

            ChangeOnUpdate();
        }

        if (GameManager.instance.transitionOnSlowMo != 1)
            ChangeOnSlowMo();
    }

    [ContextMenu("Next state")]
    public void Touched()
    {
        if (GameManager.instance.timerAtBeginning != 1 || GameManager.instance.pause || !active)
            return;

        instruData = sound_manager.getData(indexForSoundManager);
        if (!instruData.on)
        {
            instruData.on = true;
            instruData.currentState = 0;
            sound_manager.UnMute(indexForSoundManager);
            //Debug.Log("Unmute " + nameEventUnMute.Id);
        }
        else if (instruData.currentState == instruData.switches.Count || instruData.switches.Count == 0)
        {
            instruData.on = false;
            instruData.currentState = 0;
            sound_manager.Mute(indexForSoundManager);
            //Debug.Log("Mute " + nameEventMute.Id);
        }

        if(instruData.switches.Count != 0)
        {
            sound_manager.Switch(indexForSoundManager);
            if(!instruData.on)
                instruData.currentState = 0;
        }
        else
        {
            Debug.Log("Error : did not have any state ", sound_manager.gameObject);
            instruData.currentState = instruData.on? 1:0;
        }

        ChangeOnClick();
    }

    public void UpdateRTPCValue()
    {
        sound_manager.UpdateRTPCValue(indexForSoundManager);
    }

    protected void ChangeOnStart()
    {
        onStartEvent.Invoke();
    }
    protected void ChangeOnClick()
    {
        onClickEvent.Invoke();
    }
    protected void ChangeOnUpdate()
    {
        instruData = sound_manager.getData(indexForSoundManager);
        onUpdatEvent.Invoke();
    }

    protected void ChangeOnSlowMo()
    {
        onSloMoEvent.Invoke();
    }

}
