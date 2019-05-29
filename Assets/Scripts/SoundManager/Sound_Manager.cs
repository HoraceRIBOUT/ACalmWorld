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
            public InstruPossible instru = InstruPossible.instr1;
            [Tooltip("0 = off")]
            public int stateNeeded = 1;
        }

        public List<InstruEtNum> instruAndStateNeeded = new List<InstruEtNum>();
    }

    public enum InstruPossible
    {
        instr1,
        instr2,
        instr3,
        instr4,
        instr5,
    }
    public enum InstruCompo1
    {
        pianovintage,
        mid,
        low,
        aeri,
        kick,
    }

    public AK.Wwise.Event startEvent;
    public AK.Wwise.Event glitchAppearEvent;

    [SerializeField]
    public List<InstruData> listInstru = new List<InstruData>();
    private int numberInstruOn = 0;
    
    [Header("Pause")]
    public AK.Wwise.Event pauseEvent;
    public AK.Wwise.Event resumeEvent;

    [Header("Voice")]
    [SerializeField]
    public List<CombinaisonGagnante> voiceCombi = new List<CombinaisonGagnante>();
    private int currentVoice = -1;
    private int numberOfEndToIgnore = 0;
    public AkCallbackType callBackToSeek = AkCallbackType.AK_MusicSyncBeat;
    public AkCallbackType callBackForVoiceEnd = AkCallbackType.AK_EndOfEvent;

    [Header("Effect list")]
    public AK.Wwise.Event stopEffet;
    public AK.Wwise.Event rainEvent;
    public AK.Wwise.Event snowEvent;
    public AK.Wwise.Event reverseEvent;
    [HideInInspector] public int stateEffect = 0;

    public AK.Wwise.Event firstClickEvent;

    [Header("Transition")]
    public GameObject transitionInstrument;
    public CombinaisonGagnante combiForTransition;
    public AK.Wwise.Event startTransition;
    public AK.Wwise.Event endTransition;
    public bool onTransition = false;

    [Header("Event every X beat")]
    public AK.Wwise.Event regularBeatEvent;
    public int averageDelayBetweenEvent = 4;
    public Vector2 possibleOffset = Vector2.zero;
    private int currentBeat = 0;
    private int nextBeat = 4;

    [Header("Bank")]
    public AK.Wwise.Bank bankToLoad;
    private AK.Wwise.Bank bankToUnload = null;
    private bool firstClick = true;

    [Header("Autoplay")]
    public bool autoPlaying = false;
    public AutoPlay autoPlay;

    public void Awake()
    {
        if (instance == null)
        {
            //premier appel
            AwakeCall();
        }
        else
        {
            //wait... that the same call ?
            bankToUnload = instance.bankToLoad;
            instance.Unload();
            //not anymore !
            autoPlaying = instance.autoPlaying;
            GameManager.instance.UpdateAutoPlayButton(false);

            AwakeCall();
        }
    }

    public void Unload()
    {
        //nothing for now
    }

    private void AwakeCall()
    {
        instance = this;
        uint bankId;
        AKRESULT eResult = AkSoundEngine.LoadBank(
            bankToLoad.Name,       // Identifier of the bank to be loaded.
            AkSoundEngine.AK_DEFAULT_POOL_ID,      // Memory pool ID (data is written in the default sound engine memory pool when AK_DEFAULT_POOL_ID is passed).
            out bankId
        );
        if (eResult != AKRESULT.AK_Success)
        {
            Debug.LogError("Bank didn't load " + eResult.ToString());
        }

        AkSoundEngine.PostEvent(startEvent.Id, instance.gameObject);

        startEvent.Post(this.gameObject, (uint)callBackToSeek, CallBackFunction);

        for (int i = 0; i < listInstru.Count; i++)
        {
            listInstru[i].gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>().indexForSoundManager = i;
        }
    }

    private bool callBackDone = false;

    void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
        if (!callBackDone)
        {
            callBackDone = true;
            //Verif
            VoiceCombinaisonVerification();
            ////Glitch in rythm
            //GameManager.instance.shaderHandler.OnEachBar();

            if(autoPlaying)
                autoPlay.BarCall(listInstru, numberInstruOn);

            if(GameManager.instance.transitionOnSlowMo == 1)
                Invoke("CallBackUnDone", 0.1f);


            if(averageDelayBetweenEvent == currentBeat )
            {
                currentBeat = 0;
                nextBeat = averageDelayBetweenEvent + Random.Range((int)possibleOffset.x, (int)possibleOffset.y);
                if(regularBeatEvent.IsValid())
                    AkSoundEngine.PostEvent(regularBeatEvent.Id, gameObject);
            }
            else
            {
                currentBeat++;
            }

        }
    }

    void CallBackUnDone()
    {
        callBackDone = false;
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

    public void StartGlitchAppear()
    {
        if(glitchAppearEvent.IsValid())
        AkSoundEngine.PostEvent(glitchAppearEvent.Id, gameObject);
    }

    public InstruData getData(int index)
    {
        return listInstru[index];
    }


    public void UnMute(int indexForSoundManager)
    {
        if (firstClick )
        {
            firstClick = false;
            if (firstClickEvent.IsValid())
            {
                AkSoundEngine.PostEvent(firstClickEvent.Id, gameObject);
            }
            if(bankToUnload != null)
            {
                System.IntPtr intPtr = new System.IntPtr();
                AKRESULT eResult = AkSoundEngine.UnloadBank(bankToUnload.Id, intPtr);
                Debug.Log("Unload !" + eResult.ToString());
            }
        }
        AkSoundEngine.PostEvent(getData(indexForSoundManager).nameEventUnMute.Id, gameObject);

        //Part for the prog
        GameManager.instance.UpdateShaderIntensity(++numberInstruOn, listInstru.Count);
    }

    public void Mute(int indexForSoundManager, bool updateShader = true)
    {
        AkSoundEngine.PostEvent(getData(indexForSoundManager).nameEventMute.Id, gameObject);
        --numberInstruOn;
        if (updateShader)
        {
            //Part for the prog
            GameManager.instance.UpdateShaderIntensity(numberInstruOn, listInstru.Count);
        }
    }

    public void Switch(int indexForSoundManager)
    {
        InstruData instruData = getData(indexForSoundManager);
        AkSoundEngine.SetSwitch(instruData.switches[instruData.currentState].GroupId, instruData.switches[instruData.currentState].Id, gameObject);
        instruData.currentState++;

        VerificationCompoTransition();
    }

    public void Switch(int instrumentIndexForSoundManager, int variationIndex)
    {
        InstruData instruData = getData(instrumentIndexForSoundManager);
        AkSoundEngine.SetSwitch(instruData.switches[variationIndex].GroupId, instruData.switches[variationIndex].Id, gameObject);
        instruData.currentState = variationIndex + 1;

        VerificationCompoTransition();
    }

    public void VerificationCompoTransition()
    {
        if (combiForTransition.onPlay)
            return;

        bool result = true;
        foreach(CombinaisonGagnante.InstruEtNum instrNum in combiForTransition.instruAndStateNeeded)
        {
            if (listInstru[(int)instrNum.instru].currentState != instrNum.stateNeeded)
            {
                result = false;
            }
        }
        if (result)
        {
            ActivateTransitionInstru();
            combiForTransition.onPlay = true;
        }
    }

    public void ActivateTransitionInstru()
    {
        transitionInstrument.SetActive(true);
    }

    public void LaunchTransition()
    {
        //deactivate other instr 
        foreach(InstruData instr in listInstru)
        {
            instr.gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>().active = false;
            //deactivate all instr 
        }
        if(startTransition.IsValid())
            AkSoundEngine.PostEvent(startTransition.Id, gameObject);

        onTransition = true;
        GameManager.instance.shaderHandler.lerpForTarget[GameManager.instance.shaderHandler.lerpForTarget.Count - 1] = ((float)listInstru.Count - numberInstruOn) / (float)listInstru.Count;
        GameManager.instance.LaunchTransition();
    }
    
    public void TransitionFinish()
    {
        Debug.Log("Finish trnasition in sound manager !");

        AkSoundEngine.PostEvent(endTransition.Id, gameObject);
        onTransition = false;
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
