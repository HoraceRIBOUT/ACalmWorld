using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuController : MonoBehaviour
{
    public Text timeText;
    public Text dateText;
    public GameObject panelOption;
    public TextMeshProUGUI yes;
    public TextMeshProUGUI no;
    public Color colorOn = Color.white;
    public Color colorOff = Color.white;

    public AK.Wwise.Event startEvent;
    public AK.Wwise.Event clickEvent;
    public AK.Wwise.Event launchGameEvent;
    public GameObject optionButton;
    public bool isOptionPush = false;
    public GameObject panelCredit;  

    private float heightScreenRatio;
    private float widthScreenRatio;

    // Start is called before the first frame update
    void Start()
    {
        dateText.text = DateTime.Now.ToString().Substring(0, 6) + "19??";

        if (startEvent.IsValid())
            AkSoundEngine.PostEvent(startEvent.Id, this.gameObject);

        ChangeYesNo();
        widthScreenRatio = 1920f / Camera.main.pixelWidth;
        heightScreenRatio = 1080f / Camera.main.pixelHeight;
    }

    public void OnPushOption()
    {
        isOptionPush = true;
        panelOption.SetActive(!panelOption.activeSelf);
        if (panelOption.activeSelf)
            panelCredit.SetActive(false);
    }

    public void OnPushCredit()
    {
        isOptionPush = true;
        panelCredit.SetActive(!panelCredit.activeSelf);
        if (panelCredit.activeSelf)
            panelOption.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        timeText.text = System.DateTime.Now.TimeOfDay.ToString().Substring(0, 5);

        if (panelOption.activeInHierarchy || panelCredit.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 pos = Input.mousePosition;
                if (!isOptionPush)
                    if (pos.x * widthScreenRatio > Camera.main.pixelWidth * widthScreenRatio - (Camera.main.pixelWidth * widthScreenRatio - panelOption.GetComponent<RectTransform>().rect.width) / 2 || pos.x * widthScreenRatio < (Camera.main.pixelWidth * widthScreenRatio - panelOption.GetComponent<RectTransform>().rect.width) / 2)
                    {

                        panelOption.SetActive(false);
                        panelCredit.SetActive(false);
                    }
                    else if (pos.y * heightScreenRatio > Camera.main.pixelHeight * heightScreenRatio - (Camera.main.pixelHeight * heightScreenRatio - panelOption.GetComponent<RectTransform>().rect.height) / 2 || pos.y * heightScreenRatio < (Camera.main.pixelHeight * heightScreenRatio - panelOption.GetComponent<RectTransform>().rect.height) / 2)
                    {
                        panelOption.SetActive(false);
                        panelCredit.SetActive(false);
                    }

            }

            if (Input.touchCount > 0)
            {
                Vector3 pos = Input.GetTouch(0).position;
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (pos.x * widthScreenRatio > Camera.main.pixelWidth * widthScreenRatio - (Camera.main.pixelWidth * widthScreenRatio - panelOption.GetComponent<RectTransform>().rect.width) / 2 || pos.x * widthScreenRatio < (Camera.main.pixelWidth * widthScreenRatio - panelOption.GetComponent<RectTransform>().rect.width) / 2)
                    {
                        panelOption.SetActive(false);
                        panelCredit.SetActive(false);
                    }
                    else if (pos.y * heightScreenRatio > Camera.main.pixelHeight * heightScreenRatio - (Camera.main.pixelHeight * heightScreenRatio - panelOption.GetComponent<RectTransform>().rect.height) / 2 || pos.y * heightScreenRatio < (Camera.main.pixelHeight * heightScreenRatio - panelOption.GetComponent<RectTransform>().rect.height) / 2)
                    {
                        panelOption.SetActive(false);
                        panelCredit.SetActive(false);
                    }
                }
            }
        }
        isOptionPush = false;
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

  

    public void OnKeepPlaying()
    {
        //change PlayerSave

        PlayerPrefs.SetInt("AlwaysPlaying", PlayerPrefs.GetInt("AlwaysPlaying") == 0 ? 1 : 0);
        //call for the change
        ChangeYesNo();
    }

    public void ChangeYesNo()
    {
        if (PlayerPrefs.GetInt("AlwaysPlaying", 0) == 0)
        {
            yes.fontStyle &= ~FontStyles.Underline;
            no.fontStyle = FontStyles.Underline;
        }
        else
        {
            yes.fontStyle = FontStyles.Underline;
            no.fontStyle &= ~FontStyles.Underline;
        }
    }

    public void PlayButtonSound()
    {
        if (clickEvent.IsValid())
            AkSoundEngine.PostEvent(clickEvent.Id, this.gameObject);
    }
}
