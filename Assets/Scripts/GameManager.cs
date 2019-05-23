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
            instance.mainCamera.enabled = false;
            instance.decorFolder.gameObject.SetActive(false);
            instance.interactiveFolder.gameObject.SetActive(false);
            instance = this;
            StartTransition();
        }
    }

    public enum SceneName
    {
        FirstCompo,
        SecondCompo,
    }

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

    [Header("Transition : On")]
    public SceneName nextScene = SceneName.SecondCompo;
    public float transitionOnDuration = 1.5f;
    [Range(0,1)]
    public float transitionOnSlowMo = 1;

    [Header("Transition : Receive")]
    public float transitionRecDuration = 5;

    [Header("Apparition at start")]
    public float timerAtBeginning = -3f;
    private float saveTimerAtBeginning;
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
        saveTimerAtBeginning = timerAtBeginning;
    }
    
    private void StartTransition()
    {
        onTransition = true;
        timerAtBeginning -= transitionRecDuration;
        shaderHandler.lerpForTarget[0] = 1;
        shaderHandler.StartTransitionGlitch();
        //Move block to far away

        //start moving them back (in "transitionRecDuration" seconds)
    }

    public void TransitionFinish()
    {
        onTransition = false;
        shaderHandler.EndTransitionGlitch();
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
            if(transitionOnSlowMo == 1)
                animatorMainCam.SetLayerWeight(1, currentLerpValue * animationMaxWeight);
        }
        
        if (timerAtBeginning < 0)
        {
            timerAtBeginning += Time.deltaTime;
            if(timerAtBeginning >= 0)
            {
                if (onTransition)
                {
                    TransitionFinish();
                    shaderHandler.lerpForTarget[0] = 0;
                }
                glitchHandler.on = true;
                foreach(GameObject gO in allVisualInteractibleObject)
                {
                    gO.SetActive(true);
                }

                snd_mng.StartGlitchAppear();
            }
            else if (onTransition)
            {
                shaderHandler.lerpForTarget[0] = Mathf.Clamp01(-(timerAtBeginning + 1.0f) / (transitionRecDuration));
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
        //TO DO : a starting effect so people know that it's lock ???
        StartCoroutine(LoadNextScene());
        targetLerpValue = 0;
    }

    public IEnumerator LoadNextScene()
    {
        //The transition on (transition on is the transition that start when you click on the plant. 
        //                  (transition receive is the one when you start the scene but are coming from an other scene)
        while (transitionOnSlowMo > 0)
        {
            transitionOnSlowMo -= Time.deltaTime / transitionOnDuration;
            if (transitionOnSlowMo < 0)
                transitionOnSlowMo = 0;

            animatorMainCam.speed = transitionOnSlowMo;
            shaderHandler.lerpForTarget[shaderHandler.lerpForTarget.Count - 1] = 1 - transitionOnSlowMo;
            yield return new WaitForSeconds(0.1f);
        }
        //screen current 
        yield return new WaitForEndOfFrame();
        shaderHandler.Transition_Mat.SetTexture("_LastFrame", ScreenCapture.CaptureScreenshotAsTexture());

        //security
        yield return new WaitWhile(() => snd_mng.onTransition);
        //
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene.ToString(), UnityEngine.SceneManagement.LoadSceneMode.Additive);

        Debug.Log("start operation (load second scene) = "+ nextScene.ToString());
        while (!operation.isDone)
        {

            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Done !");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
    }


    public void OnDestroy()
    {
        for (int index = 0; index < shaderHandler.lerpForTarget.Count; index++)
        {
            //shaderHandler.lerpForTarget[index] = 0;
        }
    }
}
