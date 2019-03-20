using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Wave : MonoBehaviour
{
    public float period = 1;
    public float amplitude = 1;
    
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh myMesh;
    
    private Vector3[] startPos;

    // Start is called before the first frame update
    void OnEnable()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();

        startPos = _meshFilter.mesh.vertices;
    }

    public float __dbg_value = 0;
    // Update is called once per frame
    void Update()
    {
        Vector3[] verticesMove = _meshFilter.mesh.vertices;
        for (int indexVertice = 0; indexVertice < _meshFilter.sharedMesh.vertices.Length; indexVertice++)
        {
            verticesMove[indexVertice].y = startPos[indexVertice].y + Mathf.Sin(indexVertice * Time.time * period + startPos[indexVertice].x + startPos[indexVertice].z) * amplitude;
        }
        _meshFilter.mesh.vertices = verticesMove;
        //vertices = _meshFilter.sharedMesh.vertices;
    }
}

/*
 for (int indexVertice = 0; indexVertice < _meshFilter.sharedMesh.vertices.Length; indexVertice++)
        {
            float offset = Mathf.Sin(Time.time * period) * amplitude;
            if (indexVertice == 1)
                Debug.Log("Before = " + _meshFilter.mesh.vertices[indexVertice].y);
            _meshFilter.mesh.vertices[indexVertice].y = 0;
            if(indexVertice == 1)
                Debug.Log("After = " + _meshFilter.mesh.vertices[indexVertice].y + " offset = " + offset + " somme = " + (_meshFilter.mesh.vertices[indexVertice].y + offset));
        }
        vertices = _meshFilter.mesh.vertices;
     
     */
