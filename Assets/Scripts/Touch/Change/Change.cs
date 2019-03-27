using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Change : MonoBehaviour
{
    public abstract void ChangeOnStart();
    public abstract void ChangeOnClick(int currentState, bool on);
    public abstract void ChangeOnUpdate(float rtpcValue);
}
