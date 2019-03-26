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
    public AK.Wwise.Switch switchGroup;
    public List<AK.Wwise.State> states;
    public int currentPos = 0;
    
    public AK.Wwise.RTPC rtpcId;
    //public int maxSize = 3;

    public void Start()
    {
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
            AkSoundEngine.PostEvent(nameEventUnMute.Id, gameObject);
            Debug.Log("Unmute");
        }
        else if (currentPos == states.Count)
        {
            on = false;
            AkSoundEngine.PostEvent(nameEventMute.Id, gameObject);
            Debug.Log("Mute");
        }

        if(states.Count != 0 || switchGroup == null)
            AkSoundEngine.SetSwitch(switchGroup.Id, states[currentPos].Id, this.gameObject);
        else
            Debug.Log(switchGroup == null ? "Error : did not have switch group " : "Error : did not have any state ", this.gameObject);
        
    }


}
