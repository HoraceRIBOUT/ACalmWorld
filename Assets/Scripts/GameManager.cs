using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatColor", menuName = "MidnightW/Save", order = 1)]
public class ListColorData : ScriptableObject
{
    public List<Color> matColor;
}

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

    //Depreciated
    public List<Material> toDarken;
    public ListColorData colorSave;

    public float lerpMatColor = -3f;
    public List<GameObject> allVisualInteractibleObject;

    public void Start()
    {
        if (snd_mng != null) 
            snd_mng = Sound_Manager.instance;
        //Depreciated//DarkenMaterial();
        glitchHandler.PutObjectInRender(allVisualInteractibleObject);
        foreach (GameObject gO in allVisualInteractibleObject)
        {
            gO.SetActive(false);
        }
    }

    public void Update()
    {

        //Camera
        if(targetLerpValue != currentLerpValue)
        {
            float signBef = Mathf.Sign(targetLerpValue - currentLerpValue);
            currentLerpValue += Time.deltaTime * speed * signBef;
            if(signBef != Mathf.Sign(targetLerpValue - currentLerpValue))
            {
                targetLerpValue = currentLerpValue;
            }
            shaderHandler.lerpForTarget[0] = currentLerpValue;
            animatorMainCam.SetLayerWeight(1, currentLerpValue * animationMaxWeight);
        }
        
        if (lerpMatColor < 0)
        {
            lerpMatColor += Time.deltaTime;
            if(lerpMatColor >= 0)
            {
                glitchHandler.on = true;
                foreach(GameObject gO in allVisualInteractibleObject)
                {
                    gO.SetActive(true);
                }
            }
        }
        else if(lerpMatColor != 1)
        {
            lerpMatColor += Time.deltaTime * 0.5f; //because trauma is set at 2
            if (lerpMatColor > 1)
            {
                lerpMatColor = 1;
                glitchHandler.ReleaseThem();
            }
            //Depreciated//UpdateMaterialColor();
        }

#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
#endif

    }
    
    public void VoiceGlitch(int numeroGlitch)
    {
        //Yeah ! Let's get fuckedup !
    }


    public void UpdateShaderIntensity(int numberCurrInstru, int numberMaxInstru)
    {
        if (animatorMainCam == null)
            animatorMainCam = mainCamera.GetComponent<Animator>();
        targetLerpValue = (float)numberCurrInstru / (float)numberMaxInstru;
    }

    //Depreciated
#region MaterialDarkToClear
    //Depreciated
    void DarkenMaterial()
    {
        if (colorSave == null)
        {
            colorSave = ScriptableObject.CreateInstance<ListColorData>();
            colorSave.matColor = new List<Color>();
        }
        if (toDarken.Count != 0)
        {
            foreach (Material mat in toDarken)
            {
                colorSave.matColor.Add(mat.color);
                mat.color = Color.black;
            }
        }
    }

    void UpdateMaterialColor()
    {
        if (colorSave == null)
            Debug.Log("No color save");
        if (colorSave.matColor == null)
            Debug.Log("No colorSave matColor ");
        for (int i = 0; i < Mathf.Min(toDarken.Count, colorSave.matColor.Count); i++)
        {
            Color col = colorSave.matColor[i];
            toDarken[i].color = Color.Lerp(Color.black, col, lerpMatColor);
        }
    }
#endregion

}
