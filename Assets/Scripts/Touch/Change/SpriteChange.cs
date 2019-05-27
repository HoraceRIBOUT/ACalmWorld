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
        mI.onSwitchEvent.AddListener(ChangeOnClick);
        mainInstrument = mI;
    }

    public void ChangeOnClick()
    {
        if (spriteRdr != null)
        {
            spriteRdr.sprite = spritePerState[mainInstrument.instruData.currentState];
        }
    }
}
