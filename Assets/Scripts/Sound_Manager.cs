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

    [SerializeField]
    public List<InstruData> listInstru = new List<InstruData>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AkSoundEngine.PostEvent(startEvent.Id, instance.gameObject);
        }
        else
            Debug.Log("More than one sound manager");
    }
    
    public AK.Wwise.Event startEvent;
    

    
}
