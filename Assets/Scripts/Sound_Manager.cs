using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("More than one sound manager");
    }





    public AK.Wwise.Event startEvent;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent(startEvent.Id, instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
