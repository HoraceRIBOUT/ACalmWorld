using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sound_Manager))]
public class CustEdit_SndMng : Editor
{
    public override void OnInspectorGUI()
    {
        Sound_Manager myTarget = (Sound_Manager)target;

        //Change id for gameObject name
        if (myTarget.listInstru.Count != 0)
        {
            foreach(Sound_Manager.InstruData instruData in myTarget.listInstru)
            {
                if (instruData.gameObjectOfTheInstrument != null)
                {
                    instruData.id = instruData.gameObjectOfTheInstrument.name;
                }
                else
                {
                    instruData.id = "NO LINK TO GAME OBJECT";
                }
            }
        }

        DrawDefaultInspector();
    }
}