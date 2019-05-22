using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionController : MonoBehaviour
{
    public GameObject YesLockButton;
    public GameObject NoLockButton;
    public AK.Wwise.Event clickEvent;


    public void pressYesButton()
    {
        if (PlayerPrefs.GetInt("AlwaysPlaying",0) == 0 )
        {
            PlayerPrefs.SetInt("AlwaysPlaying", 1 );
            YesLockButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
            NoLockButton.GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
            AkSoundEngine.PostEvent(clickEvent.Id, this.gameObject);
        }
    }

    public void pressNoButton()
    {
        if (PlayerPrefs.GetInt("AlwaysPlaying", 0) == 1)
        {
            PlayerPrefs.SetInt("AlwaysPlaying", 0);
            NoLockButton.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
            YesLockButton.GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
            AkSoundEngine.PostEvent(clickEvent.Id, this.gameObject);
        }
    }
}
