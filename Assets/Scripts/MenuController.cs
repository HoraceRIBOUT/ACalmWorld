using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text timeText;
    public Text dateText;
    public GameObject panelOption;
    public Text yes;
    public Text no;
    public Color colorOn = Color.white;
    public Color colorOff = Color.white;
    
    public AK.Wwise.Event startEvent;
    public AK.Wwise.Event clickEvent;
    public AK.Wwise.Event launchGameEvent;

    // Start is called before the first frame update
    void Start()
    {
        dateText.text = DateTime.Now.ToString().Substring(0, 6)+"19??";

        if(startEvent.IsValid())
            AkSoundEngine.PostEvent(startEvent.Id, this.gameObject);

        ChangeYesNo();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
    }

    public void OnPushPlay()
    {
        //need to have a little animation / glitch here before launching (load in a dark screen would be perfect)
        if (launchGameEvent.IsValid())
            AkSoundEngine.PostEvent(launchGameEvent.Id, this.gameObject);
        //TO DO : suppr ça ! 
        AkSoundEngine.StopAll();
        SceneManager.LoadScene("FirstCompo");
    }

    public void OnPushQuit()
    {
        Application.Quit();
    }

    public void OnPushOption()
    {
        panelOption.SetActive(!panelOption.activeSelf);
    }

    public void OnKeepPlaying()
    {
        //change PlayerSave

        PlayerPrefs.SetInt("AlwaysPlaying", PlayerPrefs.GetInt("AlwaysPlaying") == 0 ? 1 : 0);
        //call for the change
        ChangeYesNo();
    }

    public void ChangeYesNo()
    {
#if UNITY_ANDROID
        if(PlayerPrefs.GetInt("AlwaysPlaying", 0) == 0)
#else
        if(PlayerPrefs.GetInt("AlwaysPlaying", 1) == 0)
#endif
        {
            yes.color = colorOff;
            no.color = colorOn;
        }
        else
        {
            yes.color = colorOn;
            no.color = colorOff;
        }
    }

    public void PlayButtonSound()
    {
        if (clickEvent.IsValid())
            AkSoundEngine.PostEvent(clickEvent.Id, this.gameObject);
    }
}
