using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifController : MonoBehaviour
{
    public UnityEngine.UI.Text debugText;

    private void OnApplicationFocus(bool focus)
    {
//#if !UNITY_EDITOR
        if (!focus)
        {
            LockOrBackground();
        }
        else
        {
            ComeBack();
        }
//#endif
    }

   /* private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            LockOrBackground();
        }
        else
        {
            ComeBack();
        }
    }*/

    public void LockOrBackground()
    {
        if(PlayerPrefs.GetInt("AlwaysPlaying") == 0 && !GameManager.instance.pause)
        {
            debugText.text += "Pause " + Time.realtimeSinceStartup + "\n";
            Debug.Log("Pause " + Time.realtimeSinceStartup);
            GameManager.instance.Pause();
        }
    }

    public void ComeBack()
    {
        debugText.text += "Resume " + Time.realtimeSinceStartup + "\n";
        Debug.Log("Resume " + Time.realtimeSinceStartup);
    }

    public void CallBack_PauseResume()
    {
        GameManager.instance.Pause();
    }

    public void CallBack_Return()
    {
        ComeBack();
    }

    public void CallBack_Autoplay()
    {
        //TO DO ! on off ?
    }
}
