using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool pause = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Physics.autoSimulation = false;
        }
        else
        {
            instance = this;
            StartTransition();
        }
    }

    private GameManager nextGameManager = null;

    public Sound_Manager snd_mng;
    public Camera mainCamera;
    public Animator animatorMainCam;
    public ApplyMatToScreen shaderHandler;
    public ApplyGlitch glitchHandler;
    public Transform interactiveFolder;
    public Transform decorFolder;



    private float targetLerpValue = 0;
    private float currentLerpValue = 0;
    private float speedForVoiceEffect = 0.5f;

    [Header("Camera movement")]
    public float speed = 0.5f;
    public float animationMaxWeight = 0.3f;

    [Header("Transition")]
    public float transitionDuration = 5;

    [Header("Apparition at start")]
    public float timerAtBeginning = -3f;
    public List<GameObject> allVisualInteractibleObject;

    [Header("Camera movement")]
    public bool onTransition = false;

    [Header("Pause")]
    public UnityEngine.UI.Button pauseBtn;
    public UnityEngine.UI.Button resumeBtn;
    public UnityEngine.UI.Image darkenScreen;

    public void Start()
    {
        if (snd_mng != null) 
            snd_mng = Sound_Manager.instance;
        glitchHandler.PutObjectInRender(allVisualInteractibleObject);
        foreach (GameObject gO in allVisualInteractibleObject)
        {
            gO.SetActive(false);
        }
    }
    
    private void StartTransition()
    {
        onTransition = true;
        timerAtBeginning -= transitionDuration;
        //Move block to far away

        //start moving them back (in "transitionDuration" seconds)
    }

    public void TransitionFinish()
    {
        onTransition = false;
        //
    }


    public void Update()
    {

        //Camera
        if(currentLerpValue != targetLerpValue)
        {
            float signBef = Mathf.Sign(targetLerpValue - currentLerpValue);
            currentLerpValue += Time.deltaTime * speed * signBef;
            if(signBef != Mathf.Sign(targetLerpValue - currentLerpValue))
            {
                currentLerpValue = targetLerpValue;
            }
            shaderHandler.lerpForTarget[0] = currentLerpValue;
            animatorMainCam.SetLayerWeight(1, currentLerpValue * animationMaxWeight);
        }
        
        if (timerAtBeginning < 0)
        {
            timerAtBeginning += Time.deltaTime;
            if(timerAtBeginning >= 0)
            {
                glitchHandler.on = true;
                foreach(GameObject gO in allVisualInteractibleObject)
                {
                    gO.SetActive(true);
                }
            }
        }
        else if(timerAtBeginning != 1)
        {
            timerAtBeginning += Time.deltaTime * 0.5f; //because trauma is set at 2
            if (timerAtBeginning > 1)
            {
                timerAtBeginning = 1;
                glitchHandler.ReleaseThem();
            }
        }

#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
#endif

    }

    public static bool KeepPlaying()
    {
        return (PlayerPrefs.GetInt("AlwaysPlaying") == 0);
    }
    
    public void VoiceGlitch(int numeroGlitch, bool on)
    {
        //Yeah ! Let's get fuckedup !
        if (on)
        {
            StartCoroutine(goUp(numeroGlitch + 2));
        }
        else
        {
            StartCoroutine(goDown(numeroGlitch + 2));
        }
    }

    public IEnumerator goUp(int index)
    {
        while (shaderHandler.lerpForTarget[index] <= 1) 
        {
            shaderHandler.lerpForTarget[index] += Time.deltaTime * speedForVoiceEffect;
            yield return new WaitForSeconds(0.01f);
        }
        shaderHandler.lerpForTarget[index] = 1;
    }

    public IEnumerator goDown(int index)
    {
        while (shaderHandler.lerpForTarget[index] >= 0)
        {
            shaderHandler.lerpForTarget[index] -= Time.deltaTime * speedForVoiceEffect;
            yield return new WaitForSeconds(0.01f);
        }
        shaderHandler.lerpForTarget[index] = 0;
    }


    public void UpdateShaderIntensity(int numberCurrInstru, int numberMaxInstru)
    {
        if (animatorMainCam == null)
            animatorMainCam = mainCamera.GetComponent<Animator>();
        if (numberCurrInstru == 0)
            targetLerpValue = 0;
        else
            targetLerpValue = 0.25f + 0.75f * ((float)numberCurrInstru / (float)numberMaxInstru);
    }

    public void Pause()
    {
        NotifController notif = FindObjectOfType<NotifController>();
        if (notif != null)
            notif.debugText.text += "GM do pause " + Time.realtimeSinceStartup + "";
        pause = !pause;
        snd_mng.Pause(pause);

        pauseBtn.gameObject.SetActive(!pause);
        resumeBtn.gameObject.SetActive(pause);
        darkenScreen.gameObject.SetActive(pause);

        Time.timeScale = pause ? 0 : 1;
    }
    
    public void LaunchTransition()
    {
        onTransition = true;
        StartCoroutine(LoadNextScene());
        //effectttttttttt !
        //Mute all instr
        //wait ? Animation ?
        /////change directionnal light to good one 
        /////make the skies go transparency
        //the whole "decor" have to glitch out before being replace by the new one 

        //(how to do it? 

        ////
        /// First : screen the camera before the deactivation of all element (light already change and gradient already made)
        /// First TWO : deactivate so all element
        /// Second : activate all other element (they have to deactivate themself if there are with a previous gameManager)
        /// Third : make them come by glitching ON the current last frame of the camera 
        /// (3 texture : real camera, last frame, prevous frame)
        /// - if real == previous : take last frame OR previous ?
        /// - if real != previous : take the real frame
        /// - put on previous frame the effective one 
        /// - Condition of the OR : 
        /// 
        /// Finally : destroy the old scene. we only need one
        ////
    }

    public IEnumerator LoadNextScene()
    {
        //screen current 
        yield return new WaitWhile(() => snd_mng.onTransition);
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
         AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SecondCompo", UnityEngine.SceneManagement.LoadSceneMode.Additive);

        while (!operation.isDone)
        {

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Done !");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
    }
}
