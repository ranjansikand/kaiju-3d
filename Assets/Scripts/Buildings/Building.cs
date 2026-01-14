// Holds data for a building


using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building", order = 0)]
public class Building : ScriptableObject {
    public string buildingName;
    public Resources cost; 


    [SerializeField] private Mesh _mesh;
    public virtual Mesh mesh => _mesh;

    public Material material;

    public virtual bool dragToPlace => false;  // For multi-placement buildings

    public virtual void Built(SpawnedBuilding spawnedBuilding) {}
    public virtual void Destroyed(SpawnedBuilding spawnedBuilding) {}
}
