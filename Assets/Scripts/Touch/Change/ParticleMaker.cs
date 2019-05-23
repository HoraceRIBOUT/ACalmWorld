using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : Change
{
    public List<ParticleSystem> particleForEachState = new List<ParticleSystem>();
    private int previousState = -1;

    private ParticleSystem.MainModule currentParticule = new ParticleSystem.MainModule();

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onStartEvent.AddListener(ChangeOnStart);
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onSloMoEvent.AddListener(ChangeOnSlowMo);
        mainInstrument = mI;
    }

    public void ChangeOnStart()
    {
        previousState = -1;
    }

    public void ChangeOnClick()
    {
        if(previousState != -1 )
            particleForEachState[previousState].Stop();
        if (mainInstrument.instruData.on && mainInstrument.instruData.currentState != 0)
            particleForEachState[mainInstrument.instruData.currentState - 1].Play();
        previousState = mainInstrument.instruData.currentState - 1;
    }
    public void ChangeOnClick(int state)
    {
        if(state == 0)
        {
            particleForEachState[particleForEachState.Count - 2/*well, TO DO , normaly : 1 */].Stop();
            currentParticule = new ParticleSystem.MainModule();
        }
        else
        {
            if (state - 2 != -1)
                particleForEachState[state - 2].Stop();
            particleForEachState[state - 1].Play();
            currentParticule = particleForEachState[state - 1].main;

        }
    }

    public void ChangeOnSlowMo()
    {
        currentParticule.simulationSpeed = GameManager.instance.transitionOnSlowMo;
    }
}
