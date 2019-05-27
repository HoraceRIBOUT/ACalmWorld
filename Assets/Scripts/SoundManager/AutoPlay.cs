using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    





    public void BarCall(List<Sound_Manager.InstruData> instruDatas)
    {
        ChangeRandomInstr(instruDatas);
    }


    private void ChangeRandomInstr(List<Sound_Manager.InstruData> listInstrData)
    {
        int randInstr = Random.Range(0, listInstrData.Count);
        //                                                                    + 1 because 0 == off 
        int randVar = Random.Range(0, listInstrData[randInstr].switches.Count + 1);

        Debug.Log("Instr = " + randInstr + " with var = " + randVar + " info : "+ listInstrData[randInstr].switches.Count + " and "+ listInstrData[randInstr].gameObjectOfTheInstrument.name);
        Sound_Manager.InstruData instruData = listInstrData[randInstr];

        if(randVar == 0)
        {
            //stop the instr
            instruData.on = false;
            instruData.currentState = randVar; //so == 0
            Sound_Manager.instance.Mute(randInstr);
        }
        else
        {
            if (!instruData.on)
            {
                //turn on the instr
                instruData.on = true;
                Sound_Manager.instance.UnMute(randInstr);
            }
            instruData.currentState = randVar;
            //switch to the right height
            Sound_Manager.instance.Switch(randInstr, randVar - 1);
        }
        //ok, I SERIOUSLY need to optimize that
        instruData.gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>().ChangeOnSwitch();
    }


}
