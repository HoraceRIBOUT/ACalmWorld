using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : Change
{
    public SpriteRenderer spriteRdr;

    public List<Sprite> spritePerState;

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onClickEvent.AddListener(ChangeOnClick);
        mainInstrument = mI;
    }

    public void ChangeOnClick()
    {
        int currState = mainInstrument.instruData.currentState;
        if (spriteRdr != null)
        {
            spriteRdr.sprite = spritePerState[currState];
        }
    }
}
