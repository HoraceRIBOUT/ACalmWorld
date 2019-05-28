using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    public int offsetBeatweenChange = 2;
    public Vector2 randomMargeValue = Vector2.one;
    private int currentBeatWait = 0;

    //swap
    private bool haveSwap = false;



    public void BarCall(List<Sound_Manager.InstruData> instruDatas, int numberInstruOn)
    {
        if (GameManager.instance.onTransition || GameManager.instance.transitionOnSlowMo != 1)
            return;

        currentBeatWait++;
        if(currentBeatWait >= offsetBeatweenChange)
        {
            if (numberInstruOn == 3 && !haveSwap)
            {
                SwapRandomInstr(instruDatas);
                haveSwap = true;
            }
            else
            {
                ChangeRandomInstr(instruDatas, numberInstruOn);
                haveSwap = false;
            }

            currentBeatWait = Random.Range((int)randomMargeValue.x, (int)randomMargeValue.y);
        }

    }

    private void SwapRandomInstr(List<Sound_Manager.InstruData> listInstrData)
    {


        int offInstr = Random.Range(0, listInstrData.Count);
        for (int i = 0; i < listInstrData.Count; i++)
        {
            if (!listInstrData[offInstr].on)
            {
                offInstr++;
                if (offInstr >= listInstrData.Count)
                    offInstr = 0;
            }
            else
            {
                i = listInstrData.Count + 1;
            }
        }

        int onInstr = Random.Range(0, listInstrData.Count);
        for (int i = 0; i < listInstrData.Count; i++)
        {
            if(onInstr == offInstr || listInstrData[offInstr].on)
            {
                onInstr++;
                if (onInstr >= listInstrData.Count)
                    onInstr = 0;
            }
            else
            {
                i = listInstrData.Count + 1;
            }
        }

        //            if all instr turn on, stop one (randVar = 0) else, random.   if none instr on, no turn off.                                       + 1 because 0 == off 
        int randVar = Random.Range(1, listInstrData[onInstr].switches.Count + 1);


        ChangeInstr(listInstrData, offInstr, 0);
        ChangeInstr(listInstrData, onInstr, randVar);
        //else : already in that state. 
    }

    private void ChangeRandomInstr(List<Sound_Manager.InstruData> listInstrData, int numberInstruOn)
    {
        int randInstr = Random.Range(0, listInstrData.Count);

        //            if all instr turn on, stop one (randVar = 0) else, random.   if none instr on, no turn off.                                       + 1 because 0 == off 
        int randVar = (numberInstruOn == listInstrData.Count) ? 0 : Random.Range(numberInstruOn <= 1 ? 1 : 0, listInstrData[randInstr].switches.Count + 1);
        
        if(listInstrData[randInstr].currentState != randVar)
            ChangeInstr(listInstrData, randInstr, randVar);
        //else : already in that state. 
    }


    private void ChangeInstr(List<Sound_Manager.InstruData> listInstrData, int indexInstrument, int indexVariation)
    {
        Debug.Log("Instr = " + indexInstrument + " with var = " + indexVariation + " info : "+ listInstrData[indexInstrument].switches.Count + " and "+ listInstrData[indexInstrument].gameObjectOfTheInstrument.name);
        Sound_Manager.InstruData instruData = listInstrData[indexInstrument];

        if(indexVariation == 0)
        {
            if (instruData.on)
            {
                //stop the instr
                instruData.on = false;
                instruData.currentState = indexVariation; //so == 0
                Sound_Manager.instance.Mute(indexInstrument);
            }
            //else : Do  nothing
        }
        else
        {
            if (!instruData.on)
            {
                //turn on the instr
                instruData.on = true;
                Sound_Manager.instance.UnMute(indexInstrument);
            }
            instruData.currentState = indexVariation;
            //switch to the right height
            Sound_Manager.instance.Switch(indexInstrument, indexVariation - 1);
        }
        //ok, I SERIOUSLY need to optimize that
        instruData.gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>().ChangeOnSwitch();
    }


}
