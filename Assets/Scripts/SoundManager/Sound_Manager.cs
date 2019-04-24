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
        public bool onPlay = false;
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
        mid,
        low,
        aeri,
        kick,
    }

    [SerializeField]
    public List<InstruData> listInstru = new List<InstruData>();
    private int numberInstruOn = 0;

    public AK.Wwise.Event stopEffet;
    public AK.Wwise.Event rainEvent;
    public AK.Wwise.Event snowEvent;
    public AK.Wwise.Event reverseEvent;
    [HideInInspector] public int stateEffect = 0;

    [SerializeField]
    public List<CombinaisonGagnante> voiceCombi = new List<CombinaisonGagnante>();
    private int currentVoice = -1;
    private int numberOfEndToIgnore = 0;

    public AK.Wwise.Event startEvent;

    public AK.Wwise.Event pauseEvent;
    public AK.Wwise.Event resumeEvent;

    public AkCallbackType callBackToSeek = AkCallbackType.AK_MusicSyncBeat;
    public AkCallbackType callBackForVoiceEnd = AkCallbackType.AK_EndOfEvent;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AkSoundEngine.PostEvent(startEvent.Id, instance.gameObject);

            startEvent.Post(this.gameObject, (uint)callBackToSeek, CallBackFunction);

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
        //Glitch in rythm
        GameManager.instance.shaderHandler.OnEachBar();
    }

    private void VoiceCombinaisonVerification(){
        int currentIndex = 0;
        foreach (CombinaisonGagnante combi in voiceCombi)
        {
            if (!combi.onPlay)
            {
                bool res = true;
                foreach (CombinaisonGagnante.InstruEtNum data in combi.instruAndStateNeeded)
                {
                    if (listInstru[(int)data.instru].currentState != data.stateNeeded)
                    {
                        res = false;
                    }
                }
                if (res)
                {
                    PlayVoice(combi, currentIndex);
                }
            }
            //end of if
            currentIndex++;
        }
        //end of foreach
    }

    public void PlayVoice(CombinaisonGagnante combinaison, int currentIndex)
    {
        Debug.Log("Post the event !");
        AkSoundEngine.PostEvent(combinaison.eventToPlay.Id, gameObject);
        combinaison.eventToPlay.Post(this.gameObject, (uint)callBackForVoiceEnd, FinishVoice);
        combinaison.onPlay = true;
        if (currentVoice != -1)
        {
            numberOfEndToIgnore++;
            FinishVoice();
        }
        currentVoice = currentIndex;
        GameManager.instance.VoiceGlitch(currentIndex, true);
    }

    private void FinishVoice(object baseObject, AkCallbackType type, object info)
    {
        if (numberOfEndToIgnore == 0)
            FinishVoice();
        else
            numberOfEndToIgnore--;
    }

    private void FinishVoice()
    {
        voiceCombi[currentVoice].onPlay = false;
        GameManager.instance.VoiceGlitch(currentVoice, false);
        currentVoice = -1;
        Debug.Log("Finish voice");
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
        InstruData instruData = getData(indexForSoundManager);
        AkSoundEngine.SetSwitch(instruData.switches[instruData.currentState].GroupId, instruData.switches[instruData.currentState].Id, gameObject);
        instruData.currentState++;
    }

    public void Effet()
    {
        switch (stateEffect)
        {
            case 0:
                AkSoundEngine.PostEvent(stopEffet.Id, gameObject);
                break;
            case 1:
                AkSoundEngine.PostEvent(rainEvent.Id, gameObject);
                break;
            case 2:
                AkSoundEngine.PostEvent(stopEffet.Id, gameObject);
                AkSoundEngine.PostEvent(snowEvent.Id, gameObject);
                break;
            case 3:
                AkSoundEngine.PostEvent(stopEffet.Id, gameObject);
                AkSoundEngine.PostEvent(reverseEvent.Id, gameObject);
                break;
            default:
                Debug.Log("Wath?");
                break;
        }
    }

    public void UpdateRTPCValue(int index)
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(listInstru[index].rtpcId.Id, gameObject, 0, out listInstru[index].rtpcValue, ref type);
    }


    public void Pause(bool pause)
    {
        if (pauseEvent.IsValid() && resumeEvent.IsValid())
        {
            if (pause)
            {
                AkSoundEngine.PostEvent(pauseEvent.Id, gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent(resumeEvent.Id, gameObject);
            }
        }
        else
        {
            Debug.LogError((!pauseEvent.IsValid() ? "Pause event not valid" : "" )+ (!resumeEvent.IsValid() ? "Resume event not valid" : "") + " fallback technique : mute all instr");
            foreach (InstruData instr in listInstru)
            {
                if (pause)
                {
                    AkSoundEngine.PostEvent(instr.nameEventMute.Id, gameObject);
                }
                else
                {
                    if(instr.on)
                        AkSoundEngine.PostEvent(instr.nameEventUnMute.Id, gameObject);
                }
            }
        }
        
    }

}
