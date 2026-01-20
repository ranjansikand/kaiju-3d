using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedDecor : MonoBehaviour
{
    private Decor _decor;
    public Decor decor {
        get { return _decor; }
        set { _decor = value; }
    }

    [SerializeField] public MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    public void OnBuild() {
        meshFilter.mesh = decor.mesh;
        meshRenderer.material = decor.material;
    }
}