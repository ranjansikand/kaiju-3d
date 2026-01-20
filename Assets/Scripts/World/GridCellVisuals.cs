// Holds the visuals for the current environment


using UnityEngine;

public class GridCellVisuals : ScriptableObject
{
    public GameObject gridCell;
    public SpawnedBuilding spawnedBuilding;
    public SpawnedDecor spawnedDecor;
    public Decor[] decor;

    public Material hoverMaterial;
    public Material buildableMaterial;
    public Material notbuildableMaterial;
    public Material normalMaterial;
}
