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

    // Start is called before the first frame update
    void Start()
    {
        dateText.text = DateTime.Now.ToString().Substring(0, 6)+"????";
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5);
    }

    public void OnPushPlay()
    {
        SceneManager.LoadScene("FirstCompo");
    }

    public void OnPushQuit()
    {
        Application.Quit();
    }

    public void OnPushOption()
    {
        panelOption.SetActive(true);
    }
}
