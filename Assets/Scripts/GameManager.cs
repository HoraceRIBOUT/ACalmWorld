using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Sound_Manager snd_mng;
    public Camera mainCamera;
    public Animator animatorMainCam;
    public ApplyMatToScreen shaderHandler;
    public ApplyGlitch glitchHandler;

    public void Start()
    {
        if (snd_mng != null) 
            snd_mng = Sound_Manager.instance;

    }
    
    public void UpdateShaderIntensity(int numberCurrInstru, int numberMaxInstru)
    {
        shaderHandler.lerpValue = (float)numberCurrInstru / (float)numberMaxInstru;
        if (animatorMainCam == null)
            animatorMainCam = mainCamera.GetComponent<Animator>();
        animatorMainCam.SetLayerWeight(1, ((float)numberCurrInstru / (float)numberMaxInstru) * 0.3f);
    }

}
