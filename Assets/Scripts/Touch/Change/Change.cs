using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Change : MonoBehaviour
{
    protected MainInstrument mainInstrument = null;

    public abstract void AddEventOnListener(MainInstrument mI);
}
