﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifController : MonoBehaviour
{
    public UnityEngine.UI.Text debugText;

    private void OnApplicationFocus(bool focus)
    {
#if !UNITY_EDITOR
        if (!focus)
        {
            LockOrBackground();
        }
        else
        {
            ComeBack();
        }
#endif
    }
    

    public void LockOrBackground()
    {
        if(GameManager.KeepPlaying() && !GameManager.instance.pause)
        {
            Debug.Log("Pause " + Time.realtimeSinceStartup);
            GameManager.instance.Pause();
        }
    }

    public void ComeBack()
    {
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