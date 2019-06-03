using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour
{
    ///TRUE REAL AUTO PLAY FOR COMPO1
    [Header("Here start the real deal")]
    public int[] numberOfBarBeforeNext = new int[5];
    public int[] nextVarToPush = new int[5];

    public List<int> instrumentOff = new List<int>();

    private int lastIndexMelody = -1;

    public enum TypeInst
    {
        melody,
        drums,
        bass,
    }

    [System.Serializable]
    public class BehaviourInstrInAutoPlay
    {
        public List<int> durationOn = new List<int>();
        public List<int> durationOff = new List<int>();
        public TypeInst type = TypeInst.melody;

        public int intForThisOne = -1;
    }
    [Header("Behaviour")]
    [SerializeField]
    public List<BehaviourInstrInAutoPlay> behaviourForEach = new List<BehaviourInstrInAutoPlay>();

    private int indexOfDrums = 4;
    private int indexOfBass = 2;

    [Header("Melody random")]
    public int offsetBeatweenChange = 4;
    public Vector2 randomMargeValue = Vector2.one;
    private int currentBeatWait = 0;


    private List<MainInstrument> mIForEach = new List<MainInstrument>();

    public void Init(List<Sound_Manager.InstruData> instruDatas)
    {
        int numberInstr = instruDatas.Count;
        //init list and array
        mIForEach = new List<MainInstrument>();
        numberOfBarBeforeNext = new int[numberInstr];
        nextVarToPush = new int[numberInstr];

        //a little loop
        for (int i = 0; i < numberInstr; i++)
        {
            instrumentOff.Add(i);
            mIForEach.Add(instruDatas[i].gameObjectOfTheInstrument.GetComponentInChildren<MainInstrument>());

            //search wich one is drums
            if (behaviourForEach[i].type == TypeInst.drums)
                indexOfDrums = i;
            //search wich one is bass
            if (behaviourForEach[i].type == TypeInst.bass)
                indexOfDrums = i;
        }
    }

    public void BarCall(List<Sound_Manager.InstruData> instruDatas, int numberInstruOn)
    {
        if(instrumentOff.Count == instruDatas.Count)
        {
            //Consequence : start
            StartFromZero(instruDatas);
        }
        else
        {
            for (int index = 0; index < numberOfBarBeforeNext.Length; index++)
            {
                if (numberOfBarBeforeNext[index] > 0)
                {
                    numberOfBarBeforeNext[index]--;
                    if (numberOfBarBeforeNext[index] == 0)
                    {
                        Consequence(instruDatas, index);
                    }
                    //else : wait till consequence
                }
            }
        }

        currentBeatWait++;
        if (currentBeatWait >= offsetBeatweenChange)
        {
            RandomLaunchForMelody(instruDatas, 1);
            currentBeatWait = Random.Range((int)randomMargeValue.x, (int)randomMargeValue.y);
        }
        
    }

    private void StartFromZero(List<Sound_Manager.InstruData> instruDatas)
    {
        //TO DO : launch drum. In 2 , launch bass. Random : 4 or 6 --> one of the melody, with a random variation

        //For Drums 
        ChangeInstr(instruDatas, indexOfDrums, GetAVarButNotTheCurrent(instruDatas, indexOfDrums));
        numberOfBarBeforeNext[indexOfDrums] = 8;
        nextVarToPush[indexOfDrums] = GetAVarButNotTheCurrent(instruDatas, indexOfDrums);
        instrumentOff.RemoveAt(indexOfDrums);

        //For Bass
        numberOfBarBeforeNext[indexOfBass] = 2;
        nextVarToPush[indexOfBass] = GetAVarButNotTheCurrent(instruDatas, indexOfBass);
        instrumentOff.RemoveAt(indexOfBass);

        //For the melody ?
        //Codee en dur TO DO : re utilise, factorise
        RandomLaunchForMelody(instruDatas, 4);

    }

    private int GetAVarButNotTheCurrent(List<Sound_Manager.InstruData> instruDatas, int indexOfInstr)
    {
        int randomNumber = Random.Range(1, instruDatas[indexOfInstr].switches.Count);
        if(randomNumber == instruDatas[indexOfInstr].currentState)
        {
            randomNumber++;
            if (randomNumber == instruDatas[indexOfInstr].switches.Count)
                randomNumber = 1;
        }
        return randomNumber;
    }

    private void Consequence(List<Sound_Manager.InstruData> listInstrData, int indexOfTheInstr)
    {
        if (behaviourForEach[indexOfTheInstr].type == TypeInst.drums)
        {
            ChangeInstr(listInstrData, indexOfTheInstr, nextVarToPush[indexOfTheInstr]);
            numberOfBarBeforeNext[indexOfTheInstr] = behaviourForEach[indexOfTheInstr].durationOn[0];
            nextVarToPush[indexOfTheInstr] = GetAVarButNotTheCurrent(listInstrData, indexOfTheInstr);
        }
        else if (behaviourForEach[indexOfTheInstr].type == TypeInst.bass)
        {
            ChangeInstr(listInstrData, indexOfTheInstr, nextVarToPush[indexOfTheInstr]);
            numberOfBarBeforeNext[indexOfTheInstr] = behaviourForEach[indexOfTheInstr].durationOn[0];
            nextVarToPush[indexOfTheInstr] = GetAVarButNotTheCurrent(listInstrData, indexOfTheInstr);
        }
        else //so melody
        {
            if(nextVarToPush[indexOfTheInstr] == 0)
            {
                ChangeInstr(listInstrData, indexOfTheInstr, nextVarToPush[indexOfTheInstr]/*So , zero*/);
                nextVarToPush[indexOfTheInstr] = -1;
                instrumentOff.Add(indexOfTheInstr);
            }
            else
            {
                ChangeInstr(listInstrData, indexOfTheInstr, nextVarToPush[indexOfTheInstr]);
                if (Random.Range(-1.0f, 1.0f) > 0)
                {
                    //Next is Stop
                    nextVarToPush[indexOfTheInstr] = 0;
                    numberOfBarBeforeNext[indexOfTheInstr] = behaviourForEach[indexOfTheInstr].durationOn[behaviourForEach[indexOfTheInstr].intForThisOne];
                }
                else
                {
                    //Next is still playing but not the same
                    nextVarToPush[indexOfTheInstr] = GetAVarButNotTheCurrent(listInstrData, indexOfTheInstr);
                    numberOfBarBeforeNext[indexOfTheInstr] = behaviourForEach[indexOfTheInstr].durationOn[behaviourForEach[indexOfTheInstr].intForThisOne];
                }
            }
        }
    }

    private void RandomLaunchForMelody(List<Sound_Manager.InstruData> instruDatas, int delay)
    {
        //Yep, complexe 
        int indexInInstrOff = Random.Range(0, instrumentOff.Count);
        if (lastIndexMelody == instrumentOff[indexInInstrOff])
            indexInInstrOff = Random.Range(0, instrumentOff.Count);

        int indexOfAnInactiveInstr = instrumentOff[indexInInstrOff];

        if (behaviourForEach[indexOfAnInactiveInstr].intForThisOne == -1)
        {
            int lenght = behaviourForEach[indexOfAnInactiveInstr].durationOn.Count;
            behaviourForEach[indexOfAnInactiveInstr].intForThisOne = (lenght == 1 ? 0 : Random.Range(0, lenght));
        }

        numberOfBarBeforeNext[indexOfAnInactiveInstr] = delay;
        nextVarToPush[indexOfAnInactiveInstr] = GetAVarButNotTheCurrent(instruDatas, indexOfAnInactiveInstr);
        instrumentOff.RemoveAt(indexInInstrOff);

        lastIndexMelody = indexInInstrOff;
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
            //else : Do  nothing : it's already off.
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
        mIForEach[indexInstrument].ChangeOnSwitch();
    }


}
