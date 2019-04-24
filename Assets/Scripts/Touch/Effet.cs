using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effet : MonoBehaviour
{
    public Sound_Manager sound_manager;
    public ParticleMaker particleMaker;

    public void Start()
    {
        sound_manager = Sound_Manager.instance;
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.timerAtBeginning == 1 && !GameManager.instance.pause)
        {
            //call the function of the child 
            Touched();
        }
    }

    public void Touched()
    {
        sound_manager.stateEffect++;
        if(sound_manager.stateEffect > 3)
            sound_manager.stateEffect = 0;
        sound_manager.Effet();
        //add : particleMaker !
        particleMaker.ChangeOnClick(sound_manager.stateEffect);
    }

}
