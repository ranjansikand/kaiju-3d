using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building", order = 0)]
public class Building : ScriptableObject {
    public string buildingName;
    public Resources cost; 

    public Mesh mesh;

    public virtual void OnBuild() {}
    public virtual void OnDestroy() {}
}
