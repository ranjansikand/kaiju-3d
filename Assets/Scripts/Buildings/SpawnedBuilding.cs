// A building that can be put in a cell


using UnityEngine;

public class SpawnedBuilding : MonoBehaviour
{
    private Building _building;
    public Building building {
        get { return _building; }
        set { 
            _building = value;
        }
    }
    public Cell occupiedCell;

    [SerializeField] public MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    public void OnBuild() {
        meshFilter.mesh = _building.mesh;
        meshRenderer.material = _building.material;
        
        _building.Built(this);
    }
}