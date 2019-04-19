using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;

    [System.Serializable]
    public class InstruData
    {
        [HideInInspector] public string id;
        public GameObject gameObjectOfTheInstrument;

        [Header("Wwise info")]
        [SerializeField] public AK.Wwise.Event nameEventUnMute;
        [SerializeField] public AK.Wwise.Event nameEventMute;
        [SerializeField] public List<AK.Wwise.Switch> switches;
        [SerializeField] public AK.Wwise.RTPC rtpcId;

        [Header("Data")]
        public bool on = false;
        public int currentState = 0;
        public float rtpcValue = -48;


        [System.Serializable] public class SpaceFalse { }
        public SpaceFalse __________________________________;
    }

    [System.Serializable]
    public class CombinaisonGagnante
    {
        public AK.Wwise.Event eventToPlay;

        [System.Serializable] public class InstruEtNum
        {
            public InstruPossible instru = InstruPossible.pianovintage;
            [Tooltip("0 = off")]
            public int stateNeeded = 1;
        }

        public List<InstruEtNum> instruAndStateNeeded = new List<InstruEtNum>();
    }

    public enum InstruPossible
    {
        pianovintage,
        low,
        mid,
        aeri,
        kick,
    }

    [SerializeField]
    public List<InstruData> listInstru = new List<InstruData>();
    private int numberInstruOn = 0;

    [SerializeField]
    public List<CombinaisonGagnante> voiceCombi = new List<CombinaisonGagnante>();

    public AK.Wwise.Event startEvent;

    public AkCallbackType callBackToSeek = AkCallbackType.AK_MusicSyncBeat;
    public AK.Wwise.Event callBackEvent;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AkSoundEngine.PostEvent(startEvent.Id, instance.gameObject);

            callBackEvent.Post(this.gameObject, (uint)callBackToSeek, CallBackFunction);

            for (int i = 0; i < listInstru.Count; i++)
            {
                listInstru[i].gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>().indexForSoundManager = i;
            }
        }
        else
            Debug.Log("More than one sound manager");
    }

    void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
        //Debug.Log("Bar ");
        //Verif
        VoiceCombinaisonVerification();
    }

    private void VoiceCombinaisonVerification(){
        foreach (CombinaisonGagnante combi in voiceCombi)
        {
            bool res = true;
            foreach(CombinaisonGagnante.InstruEtNum data in combi.instruAndStateNeeded)
            {
                if (listInstru[(int)data.instru].currentState != data.stateNeeded)
                {
                    res = false;
                }
            }
            if (res)
            {
                Debug.Log("Post the event !");
                AkSoundEngine.PostEvent(combi.eventToPlay.Id, gameObject);
            }
        }
    }

    public InstruData getData(int index)
    {
        return listInstru[index];
    }

    public void UnMute(int indexForSoundManager)
    {
        AkSoundEngine.PostEvent(getData(indexForSoundManager).nameEventUnMute.Id, gameObject);

        //Part for the prog
        GameManager.instance.UpdateShaderIntensity(++numberInstruOn, listInstru.Count);
    }

    public void Mute(int indexForSoundManager)
    {
        AkSoundEngine.PostEvent(getData(indexForSoundManager).nameEventMute.Id, gameObject);

        //Part for the prog
        GameManager.instance.UpdateShaderIntensity(--numberInstruOn, listInstru.Count);
    }

    public void Switch(int indexForSoundManager)
    {
        Sound_Manager.InstruData instruData = getData(indexForSoundManager);
        AkSoundEngine.SetSwitch(instruData.switches[instruData.currentState].GroupId, instruData.switches[instruData.currentState].Id, gameObject);
        Debug.Log(instruData.switches[instruData.currentState].ToString() + " State == "+ instruData.currentState);
        instruData.currentState++;
    }


    public void UpdateRTPCValue(int index)
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(listInstru[index].rtpcId.Id, gameObject, 0, out listInstru[index].rtpcValue, ref type);
    }


}
