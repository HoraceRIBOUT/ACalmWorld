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

    private float targetLerpValue = 0;
    private float currentLerpValue = 0;
    public float speed = 0.5f;
    public float animationMaxWeight = 0.3f;

    public void Start()
    {
        if (snd_mng != null) 
            snd_mng = Sound_Manager.instance;

    }

    public void Update()
    {
        if(targetLerpValue != currentLerpValue)
        {
            float signBef = Mathf.Sign(targetLerpValue - currentLerpValue);
            currentLerpValue += Time.deltaTime * speed * signBef;
            if(signBef != Mathf.Sign(targetLerpValue - currentLerpValue))
            {
                targetLerpValue = currentLerpValue;
            }
            shaderHandler.lerpValue = currentLerpValue;
            animatorMainCam.SetLayerWeight(1, currentLerpValue * animationMaxWeight);
        }
    }

    public void UpdateShaderIntensity(int numberCurrInstru, int numberMaxInstru)
    {
        if (animatorMainCam == null)
            animatorMainCam = mainCamera.GetComponent<Animator>();
        targetLerpValue = (float)numberCurrInstru / (float)numberMaxInstru;
    }

}
