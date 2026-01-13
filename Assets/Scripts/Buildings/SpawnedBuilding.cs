// A building that can be put in a cell


using UnityEngine;

public class SpawnedBuilding : MonoBehaviour
{
    private Building _building;
    public Building building {
        get { return _building; }
        set { 
            _building = value; 
            OnBuild();
        }
    }
    public Cell occupiedCell;

    [SerializeField] MeshFilter meshFilter;

    private void OnBuild() {
        meshFilter.mesh = _building.mesh;
        _building.OnBuild();
    }
}