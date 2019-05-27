using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeToTex : Change
{
    public Material targetMaterial;

    public List<Texture> spritePerState;

    public Vector4 tillingAndOffset = new Vector4(0.05f,0.05f,0,0);
    public Vector4 movement = new Vector4(0,0, 0.005f, 0.001f);

    public override void AddEventOnListener(MainInstrument mI)
    {
        mI.onClickEvent.AddListener(ChangeOnClick);
        mI.onSwitchEvent.AddListener(ChangeOnClick);
        mI.onUpdatEvent.AddListener(ChangeOnUpdate);
        mainInstrument = mI;
    }

    public void ChangeOnClick()
    {
        targetMaterial.SetFloat("_Activate", mainInstrument.instruData.on ? 1 : 0);

        targetMaterial.SetTexture("_SecondTex", spritePerState[mainInstrument.instruData.currentState]);
    }

    public void ChangeOnUpdate()
    {
        tillingAndOffset += movement * Time.deltaTime * GameManager.instance.transitionOnSlowMo; // TO DO : rtpc value cuurve
        targetMaterial.SetVector("_SecondTex_ST", tillingAndOffset);
    }

    public void OnDestroy()
    {
        targetMaterial.SetFloat("_Activate", 0);
    }
}