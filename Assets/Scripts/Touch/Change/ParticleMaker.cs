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
        if (on && currentState != 0)
            particleForEachState[currentState - 1].Play();
        Debug.Log((on && currentState != 0) + "  bool | currentState = " + (currentState - 1));
        previousState = currentState - 1;
    }

    public override void ChangeOnUpdate(float rtpcValue)
    {

    }
}
