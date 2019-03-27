using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : Change
{

    public List<ParticleSystem> particleForEachState = new List<ParticleSystem>();
    private int previousState = -1;

    public override void ChangeOnStart()
    {
        previousState = -1;
    }

    public override void ChangeOnClick(int currentState, bool on)
    {
        if(previousState != -1 )
            particleForEachState[previousState].Stop();
        if(on)
            particleForEachState[currentState].Play();
        previousState = currentState;
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {

    }
}
