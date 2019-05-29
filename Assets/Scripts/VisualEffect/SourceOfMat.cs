using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SourceOfMat : MonoBehaviour
{
    public Material mat;
    
    private Transform tr;
    private Vector3 lastPos;
    
    // Update is called once per frame
    void Update()
    {
        if (tr == null)
            tr = this.transform;

        if (mat != null && lastPos != tr.position)
        {
            lastPos = tr.position;
            mat.SetVector("_SourcePos", (Vector4)lastPos);
        }
    }


    public void ChangeColor(Color newCol)
    {
        mat.SetColor("_SourceCol", newCol);
    }

    public void ChangeIntensity(float newIntensity)
    {
        mat.SetFloat("_SoucePow", newIntensity);
    }

    public void ChangeRange(float newSize)
    {
        mat.SetFloat("_SouceSiz", newSize);
    }

}
