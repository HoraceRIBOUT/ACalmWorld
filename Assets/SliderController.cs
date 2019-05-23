using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public List<GameObject> BarList;
    int actualBarNumber=0;
    public Sprite barOn;
    public Sprite barOff;
    private bool isInitiate = false;
    private int barNumber;
    public GameObject SliderMaster;

    private void Start()
    {
        barNumber = PlayerPrefs.GetInt("VolumeMaster");
        SliderMaster.GetComponent<Slider>().value = PlayerPrefs.GetInt("VolumeMaster");
        onClickSlider();
    }

    public void onClickSlider()
    {
        if (SliderMaster.GetComponent<Slider>().value != PlayerPrefs.GetInt("VolumeMaster") || !isInitiate)
        {
            barNumber = (int)SliderMaster.GetComponent<Slider>().value;
            PlayerPrefs.SetInt("VolumeMaster", barNumber);
            if ((int)SliderMaster.GetComponent<Slider>().value == 0)
            {
                foreach (GameObject bar in BarList)
                {
                    bar.GetComponent<Image>().sprite = barOff;
                }
            }
            else
            {
                for (int i = 1; i < barNumber + 1; i++)
                {
                    BarList[i-1].GetComponent<Image>().sprite = barOn;
                }

                for (int i = barNumber + 1; i < BarList.Count+1; i++)
                {
                    BarList[i-1].GetComponent<Image>().sprite = barOff;
                }
            }
           
            isInitiate = true;
        }
    }
}
