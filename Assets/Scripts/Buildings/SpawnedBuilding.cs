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

    public void OnBuild() {
        meshFilter.mesh = _building.mesh;
        _building.Built(this);
    }

    public void Refresh() {
        _building.Built(this);
    }
}