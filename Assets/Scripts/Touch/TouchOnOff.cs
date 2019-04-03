using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchOnOff : MonoBehaviour
{
    public bool on = false;
    public Color changeToColor = Color.green;
    public float ColorIntensity = -48;

    public SpriteRenderer spriteRdr;
    public MeshRenderer meshRdr;
    private Color startColor;

    public AK.Wwise.Event nameEventUnMute;
    public AK.Wwise.Event nameEventMute;
    public List<AK.Wwise.Switch> switchs;
    public int currentPos = 0;
    
    public AK.Wwise.RTPC rtpcId;
    //public int maxSize = 3;

    public GameObject sound_Manager;

    public void Start()
    {
        sound_Manager = Sound_Manager.instance.gameObject;
        if (spriteRdr != null)
        {
            startColor = spriteRdr.color;
        }
        else if (meshRdr != null)
        {
            startColor = meshRdr.material.color;
        }
    }

    private void OnMouseDown()
    {
        //call the function of the child 
        Touched();
    }

    public void Update()
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(rtpcId.Id, gameObject, 0, out ColorIntensity, ref type);

        ChangeColor();
    }
    
    protected void ChangeColor()
    {
        float valueZerOne = (ColorIntensity + 48) / 48;
        //change the color to the right one
        if (spriteRdr != null)
        {
            spriteRdr.color = startColor + changeToColor * valueZerOne;

        }
        else if (meshRdr != null)
        {
            meshRdr.material.color = startColor + changeToColor * valueZerOne;

        }
    }

    public void Touched()
    {
        if (!on)
        {
            on = true;
            currentPos = 0;
            AkSoundEngine.PostEvent(nameEventUnMute.Id, sound_Manager);
            Debug.Log("Unmute");
        }
        else if (currentPos == switchs.Count)
        {
            on = false;
            AkSoundEngine.PostEvent(nameEventMute.Id, sound_Manager);
            Debug.Log("Mute");
            currentPos = 0;
        }

        if(currentPos != switchs.Count && switchs.Count != 0)
        {
            AkSoundEngine.SetSwitch(switchs[currentPos].GroupId, switchs[currentPos].Id, sound_Manager);
            currentPos++;
        }
        else
            Debug.Log("Error : did not have any state ", this.gameObject);
        
    }


}
